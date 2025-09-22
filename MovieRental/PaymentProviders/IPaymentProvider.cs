namespace MovieRental.PaymentProviders
{
    public interface IPaymentProvider
    {
        string Name { get; }
        Task<bool> Pay(double price);
    }
}