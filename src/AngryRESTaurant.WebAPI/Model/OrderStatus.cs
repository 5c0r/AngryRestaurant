using System;

namespace AngryRESTaurant.WebAPI.Model
{
    public sealed class OrderStatus : IHaveGuid
    {
        public Guid Id { get; set; }
        public string FoodName { get; set; }
        public DateTimeOffset ExpectedReadyTime { get; set; }

        // Check OrderStatuses
        public string Status { get; set; }
    }

    public static class OrderStatuses
    {
        public static readonly string Cooking = "Cooking";
        public static readonly string ReadyToServed = "ReadyToServed";
        public static readonly string Served = "Served";
        public static readonly string Paid = "Paid";
    }
}