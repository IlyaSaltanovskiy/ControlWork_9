using ControlWork_9.Infrastructure;
using ControlWork_9.Model;
using ControlWork_9.Response;
using ControlWork_9.ViewModel;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;

namespace ControlWork_9.Service;

public class TransferService : ITransferService
{
    private readonly PaymentDBContext _dbContext;
    private readonly IAuthenticationService _authenticationService;

    public TransferService(PaymentDBContext dbContext, IAuthenticationService authenticationService)
    {
        _dbContext = dbContext;
        _authenticationService = authenticationService;
    }

    public async Task<BaseApiResponse<bool>> SendMoney(TransferModelData tmd)
    {
        if (tmd.Amount <= 0)
        {
            return new BaseApiResponse<bool>(false, "Amount must be positive");
        }

        var recipientModel =
            _dbContext.Users.FirstOrDefault(x => x.AccountNumber == tmd.RecipiantAccountNumber);
        if (recipientModel is null)
            return new BaseApiResponse<bool>(false, "Resipiant not found");

        var senderModel =
            _dbContext.Users.FirstOrDefault(x => x.Id == tmd.SenderId);
        if (senderModel is not null && senderModel.Balance < tmd.Amount)
            return new BaseApiResponse<bool>(false, "Balance is not enough");

        recipientModel.Balance += tmd.Amount;
        _dbContext.Entry(recipientModel).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        TransferModel transferModel = new TransferModel()
        {
            Amount = tmd.Amount,
            RecipientAccountNumber = tmd.RecipiantAccountNumber,
            SenderAccountNumber = senderModel?.AccountNumber
        };
        await _dbContext.TransferModels.AddAsync(transferModel);
        await _dbContext.SaveChangesAsync();


        if (senderModel != null)
        {
            senderModel.Balance -= tmd.Amount;
            _dbContext.Entry(senderModel).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }


        return new BaseApiResponse<bool>(true);
    }

    public List<TransferModel> GetTransferByAccountNumber(string accnum)
    {
        var accList = _dbContext.TransferModels
            .Where(x => x.SenderAccountNumber == accnum || x.RecipientAccountNumber == accnum)
            .ToList();
        return accList;
    }

    public List<TransferListItem> GetTransferByUserId(int id)
    {
        UserModel userModel = _authenticationService.GetUserModel(id);
        var list = GetTransferByAccountNumber(userModel.AccountNumber).OrderByDescending(x => x.DateTime);
        var items = list.Select(x => new TransferListItem(x.DateTime,
                (x.SenderAccountNumber == userModel.AccountNumber ? x.RecipientAccountNumber : x.SenderAccountNumber) ??
                "Анонимное пополнение",
                x.Amount,
                x.SenderAccountNumber == userModel.AccountNumber
                    ? "Пополнение от др. польз."
                    : "Перевод на другой счет"))
            .ToList();
        return items;
    }

    public List<TransferModel> getAllTransfer()
    {
        return _dbContext.TransferModels.OrderByDescending(x => x.DateTime).ToList();
    }
}