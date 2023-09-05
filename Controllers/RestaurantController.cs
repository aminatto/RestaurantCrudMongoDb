using Microsoft.AspNetCore.Mvc;
using Mongo.Api.Domain.Entities;
using Mongo.Api.Domain.Enums;
using Mongo.Api.Domain.ValueObjects;
using RestaurantCrudMongoDb.Controllers.Inputs;
using RestaurantCrudMongoDb.Controllers.Outputs;
using RestaurantCrudMongoDb.Data.Repositories;
using RestaurantCrudMongoDb.Domain.ValueObjects;
using ZstdSharp.Unsafe;

namespace RestaurantCrudMongoDb.Controllers
{
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantRepository _restaurantRepository;

        public RestaurantController(RestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpPost("restaurant")]
        public ActionResult CreateRestaurant([FromBody] RestaurantInclusion restaurantInclusion)
        {
            var kitchen = EKitchenHelper.ConvertFromInteger(restaurantInclusion.Kitchen); //TODO: método com bug --> consertar.

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

            _restaurantRepository.Insert(restaurant);

            return Ok(
                new
                {
                    data = "Restaurant Added Successfuly"
                });
        }

        [HttpGet("restaurant/all")]
        public async Task<ActionResult> GetAll()
        {
            var restaurants = await _restaurantRepository.GetAll();

            var list = restaurants.Select(x => new RestaurantList
            {
                Id = x.Id,
                Name = x.Name,
                Kitchen = (int)x.Kitchen,
                City = x.Address.City,
            });

            return Ok(
                new
                {
                    data = list
                });
        }

        [HttpGet("restaurant/{id}")]
        public ActionResult GetById(string id)
        {
            var restaurant = _restaurantRepository.GetById(id);

            if (restaurant == null)
                return NotFound();

            var view = new RestaurantExibition
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Kitchen = (int)restaurant.Kitchen,
                Address = new AddressExibition
                {
                    PublicPlace = restaurant.Address.PublicPlace,
                    Number = restaurant.Address.Number,
                    City = restaurant.Address.City,
                    PostalCode = restaurant.Address.PostalCode,
                    State = restaurant.Address.State,
                }
            };

            return Ok(
                new
                {
                    data = view
                });
        }

        [HttpGet("restaurant")]
        public ActionResult GetByName([FromQuery] string name)
        {
            var restaurants = _restaurantRepository.GetByName(name);

            if (restaurants == null)
                return NotFound();

            var list = restaurants.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Kitchen = (int)x.Kitchen,
                Address = new AddressExibition
                {
                    PublicPlace = x.Address.PublicPlace,
                    Number = x.Address.Number,
                    City = x.Address.City,
                    PostalCode = x.Address.PostalCode,
                    State = x.Address.State,
                }
            });

            return Ok(
                new
                {
                    data = list
                });
        }

        [HttpPut("restaurant")]
        public ActionResult Edit([FromBody] RestaurantWholeEdit restaurantWholeEdit)
        {
            var restaurant = _restaurantRepository.GetById(restaurantWholeEdit.Id);

            if (restaurant == null)
                return NotFound();

            var kitchen = EKitchenHelper.ConvertFromInteger(restaurantWholeEdit.Kitchen);
            restaurant = new Restaurant(restaurantWholeEdit.Id, restaurantWholeEdit.Name, kitchen);
            var address = new Address(
                restaurantWholeEdit.PublicPlace,
                restaurantWholeEdit.Number,
                restaurantWholeEdit.City,
                restaurantWholeEdit.State,
                restaurantWholeEdit.PostalCode);

            restaurant.AssignAddress(address);

            if (!restaurant.Validate())
            {
                return BadRequest(
                    new
                    {
                        errors = restaurant.ValidationResult.Errors.Select(x => x.ErrorMessage)
                    });
            }

            if (!_restaurantRepository.EditWholeDocument(restaurant))
            {
                return BadRequest(
                    new
                    {
                        errors = "No documents edited."
                    });
            }

            return Ok(
                new
                {
                    data = "Restaurant Edited Successfuly"
                });
        }

        [HttpPatch("restaurant/{id}")]
        public ActionResult EditKitchen(string id, [FromBody] RestaurantPartialEdit restaurantPartialEdit)
        {
            var restaurant = _restaurantRepository.GetById(id);
            if (restaurant == null)
                return NotFound();

            var kitchen = EKitchenHelper.ConvertFromInteger(restaurantPartialEdit.Kitchen);

            if (!_restaurantRepository.EditKitchen(id, kitchen))
            {
                return BadRequest(new
                {
                    errors = "No document edited."
                });
            }

            return Ok(
                new
                {
                    data = "Restaurant Edited SuccessFully."
                });
        }

        [HttpPatch("restaurant/{id}/rate")]
        public ActionResult RateRestaurant(string id, [FromBody] RatingInclusion ratingInclusion)
        {
            var restaurant = _restaurantRepository.GetByName(id);

            if (restaurant == null)
                return NotFound();

            var rating = new Rating(ratingInclusion.Stars, ratingInclusion.Comment);

            if (!rating.Validate())
            {
                return BadRequest(
                    new
                    {
                        errors = rating.ValidationResult.Errors.Select(x => x.ErrorMessage)
                    });
            }

            _restaurantRepository.Rate(id, rating);

            return Ok(
                new
                {
                    data = "Restaurant rated successfully."
                });
        }

        [HttpGet("restaurant/top3")]
        public async Task<ActionResult> GetTop3Restaurants()
        {
            var top3 = await _restaurantRepository.GetTop3();

            var list = top3.Select(x => new RestaurantTop3
            {
                Id = x.Key.Id,
                Name = x.Key.Name,
                Kitchen = (int)x.Key.Kitchen,
                City = x.Key.Address.City,
                Stars = x.Value
            });

            return Ok(
                new
                {
                    data = list
                });
        }

        [HttpDelete("restaurant/{id}")]
        public ActionResult Delete(string id)
        {
            var restaurant = _restaurantRepository.GetById(id);

            if(restaurant == null)
                return NotFound();

            (var totalRestaurantRemoved, var totalRatingsRemoved) = _restaurantRepository.Delete(id);

            return Ok(
                new
                {
                    data = $"Total documents removed: {totalRestaurantRemoved} restaurant with {totalRatingsRemoved} ratings."
                });
        }

    }
}
