using System;

namespace AngryRESTaurant.WebAPI.Model
{
    public struct CallOrderResponse
    {
        public Guid OrderId { get; set; }
        public double WaitTime { get; set; }

        public string OrderTaker { get; set; }
    }
}