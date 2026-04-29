using System;

namespace Ecart.Payments
{
    public class PaypalProcessor : IPaymentProcessor
    {
        public bool ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount:C}...");
            Console.WriteLine("Payment successful.");
            return true;
        }
    }
}
