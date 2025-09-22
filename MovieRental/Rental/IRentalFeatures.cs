namespace MovieRental.Rental;

public interface IRentalFeatures
{
	Task<Rental> SaveRentalAsync(Rental rental);
	IEnumerable<Rental> GetRentalsByCustomerName(string customerName);
}