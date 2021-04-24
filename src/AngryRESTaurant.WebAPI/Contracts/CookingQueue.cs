using System;

namespace AngryRESTaurant.WebAPI.Contracts
{
    public interface FastFoodOrdered
    {
        public Guid OrderId { get; }
        public string CustomerName { get; }
        public string FoodName { get; }
    }

    public interface SlowFoodOrdered
    {
        public Guid OrderId { get; }
        public string CustomerName { get; }
        public string FoodName { get; }
    }

    public interface FoodReady
    {
        public Guid OrderId { get; }
        public string CookName { get; }
        public string FoodName { get; }
        public DateTimeOffset ReadyTime { get; }
    }

    public interface FoodCancelled
    {
        public Guid OrderId { get; }
        public string Reason { get; }
    }
}