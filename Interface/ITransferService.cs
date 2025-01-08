using ControlWork_9.Model;
using ControlWork_9.Response;
using ControlWork_9.ViewModel;

namespace WebApi.Interfaces;

public interface ITransferService
{
    Task<BaseApiResponse<bool>> SendMoney(TransferModelData tmd);
    List<TransferModel> GetTransferByAccountNumber(string accnum);
    List<TransferListItem> GetTransferByUserId(int id);
    List<TransferModel> getAllTransfer();
}