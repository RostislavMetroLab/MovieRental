namespace MovieRental.Rental;

public interface IRentalFeatures
{
	Task<Rental> SaveRentalAsync(Rental rental);
	Task<IEnumerable<Rental>> GetRentalsByCustomerNameAsync(string customerName);
}