using System;

namespace Ecart.Payments
{
    public class CreditCardProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of {amount:C}...");
            Console.WriteLine("Payment successful.");
            return true;
        }
    }
}
