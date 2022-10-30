using DeliveryDatesGenerator;
using DeliveryDatesGenerator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace MathemWebApi.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class DeliveryDatesController : ControllerBase
    {
        private readonly IDeliveryDatesCalculator deliveryDatesCalculator;

        public DeliveryDatesController(IDeliveryDatesCalculator _deliveryDatesCalculator)
        {
            deliveryDatesCalculator = _deliveryDatesCalculator;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(IEnumerable<DeliveryDateDetail>), 200)]
        [ProducesResponseType(typeof(void), 401)]
        public async Task<IActionResult> GetDeliveryDates([FromQuery(Name = "postalcode")] string postalCode, [FromBody] HashSet<Product> products)
        {
            if(postalCode.isInvalidSwedishPostalCode())
            {
                return BadRequest("Please provide correct Swedish postalcode!");
            }

            if(ModelState.IsValid)
            {
                var deliveryDates = deliveryDatesCalculator.GetDeliveryDates(postalCode, products);

                return Ok(deliveryDatesCalculator.SortDeliveryDates(deliveryDates));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}