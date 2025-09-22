using Microsoft.AspNetCore.Mvc;
using MovieRental.Movie;
using MovieRental.Rental;

namespace MovieRental.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {

        private readonly IRentalFeatures _features;

        public RentalController(IRentalFeatures features)
        {
            _features = features;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Rental.Rental rental)
        {
            var savedRental = await _features.SaveRentalAsync(rental);
            return Ok(savedRental);
        }


        [HttpGet("customer/{customerName}")]
        public async Task<IActionResult> GetRentalsByCustomer(string customerName)
        {
            var rentals = await _features.GetRentalsByCustomerNameAsync(customerName);

            if (rentals == null || !rentals.Any())
            {
                return NotFound($"No rentals found for customer: {customerName}");
            }

            return Ok(rentals);
        }
    }
}
