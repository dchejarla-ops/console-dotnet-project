namespace Ecart.Payments
{
    public interface IPaymentProcessor
    {
        bool ProcessPayment(decimal amount);
    }
}
