using Azure;
using ControlWork_9.Model;
using Microsoft.EntityFrameworkCore;

namespace ControlWork_9.Infrastructure;

public class PaymentDBContext : DbContext
{
   
    public required DbSet<UserModel> Users { get; set; }
    public required DbSet<TransferModel> TransferModels { get; set; }

    public PaymentDBContext(DbContextOptions<PaymentDBContext> options) : base(options)
    {
    }
}