using System.ComponentModel.DataAnnotations;

namespace ControlWork_9.Model;

public class TransferModel
{
    public int Id { get; set; }
    [Required]
    public decimal Amount { get; set; }
    public string? SenderAccountNumber { get; set; }
    [Required]
    public string RecipientAccountNumber{ get; set; }

    public DateTimeOffset DateTime { get; set; } = DateTimeOffset.UtcNow;

}