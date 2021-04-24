using System;

namespace AngryRESTaurant.WebAPI.Contracts
{
    public interface OrderCreateRequest
    {
        Guid OrderId { get; }
        string CustomerName { get; }
        string FoodName { get; }
        
        DateTimeOffset OrderTime { get; }
    }

    public interface OrderCreationAccepted
    {
        Guid OrderId { get; }
        string CustomerName { get; }
        string OrderTaker { get; }
        double ReadyTime { get; }
    }

    public interface OrderCreationRejected
    {
        Guid OrderId { get; }
        string Reason { get; }
    }
}
