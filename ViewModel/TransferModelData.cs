using System.ComponentModel.DataAnnotations;

namespace ControlWork_9.ViewModel;

public class TransferModelData
{
    [Required(ErrorMessage = "Recipiant is required")]
    public required string RecipiantAccountNumber { get; set; }
    
    public int? SenderId { get; set; }
    
    [Required(ErrorMessage = "Amount is required")]
    public required decimal Amount { get; set; }

}