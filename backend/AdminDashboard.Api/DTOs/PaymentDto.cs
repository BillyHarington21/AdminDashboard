public class PaymentDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}