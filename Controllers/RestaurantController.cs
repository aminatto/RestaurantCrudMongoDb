using Microsoft.AspNetCore.Mvc;
using Mongo.Api.Domain.Entities;
using Mongo.Api.Domain.Enums;
using Mongo.Api.Domain.ValueObjects;
using RestaurantCrudMongoDb.Controllers.Inputs;

namespace RestaurantCrudMongoDb.Controllers
{
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        [HttpPost("restaurant")]
        public ActionResult CreateRestaurant([FromBody] RestaurantInclusion restaurantInclusion)
        {
            var kitchen = EKitchenHelper.ConvertFromInteger(restaurantInclusion.Kitchen);

            var restaurant = new Restaurant(restaurantInclusion.Name, kitchen);
            var address = new Address(restaurantInclusion.PublicPlace, restaurantInclusion.Number, restaurantInclusion.City,
                restaurantInclusion.State, restaurantInclusion.PostalCode);

            restaurant.AssignAddress(address);

            if (!restaurant.Validate())
            {
                return BadRequest(
                    new
                    {
                        errors = restaurant.ValidationResult.Errors.Select(e => e.ErrorMessage)
                    });
            }

            _restaurantRepository.Add(restaurant);

            return Ok(
                new
                {
                    data = "Restaurant Added Successfuly"
                });
        }

    }
}
