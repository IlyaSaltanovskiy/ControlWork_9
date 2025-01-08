namespace ControlWork_9.ViewModel;

public class TransferListItem
{
    public string DateTime { get; set; }
    public string SecondAccountNumber { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    
    public TransferListItem(DateTimeOffset dateTime, string secondAccountNumber, decimal amount, string type)
    {
        DateTime = dateTime.DateTime.ToString("dd.MM.yy HH:mm");
        SecondAccountNumber = secondAccountNumber;
        Amount = amount;
        Type = type;
    }
}
