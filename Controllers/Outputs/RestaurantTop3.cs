﻿namespace RestaurantCrudMongoDb.Controllers.Outputs
{
    public class RestaurantTop3
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Kitchen { get; set; }

        public string City { get; set; }

        public double Stars { get; set; }
    }
}
