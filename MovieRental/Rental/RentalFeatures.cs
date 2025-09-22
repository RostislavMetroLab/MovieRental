using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.PaymentProviders;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		private readonly PaymentProviderFactory _paymentProviderFactory;

		public RentalFeatures(MovieRentalDbContext movieRentalDb, PaymentProviderFactory paymentProviderFactory)
		{
			_movieRentalDb = movieRentalDb;
			_paymentProviderFactory = paymentProviderFactory;
		}

		public async Task<Rental> SaveRentalAsync(Rental rental)
		{
			var provider = _paymentProviderFactory.GetProvider(rental.PaymentMethod);
			if (provider == null)
				throw new InvalidOperationException($"Payment provider '{rental.PaymentMethod}' not found.");

			// ToDo: Implement price calculateion
			double price = rental.DaysRented * 10;

			bool paymentSuccess = await provider.Pay(price);

			if (!paymentSuccess)
				throw new InvalidOperationException("Payment failed.");

			_movieRentalDb.Rentals.Add(rental);
			await _movieRentalDb.SaveChangesAsync();
			return rental;
		}

		public async Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName)
		{
			return await _movieRentalDb.Rentals
				.Include(r => r.Customer)
				.Where(r => r.Customer != null && r.Customer.Name.ToLower() == customerName.ToLower())
				.ToListAsync();
		}

	}
}
