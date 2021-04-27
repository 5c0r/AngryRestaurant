using System;
using MediatR;

namespace AngryRESTaurant.WebAPI.Commands
{
    public struct CustomerOrderRequest : IRequest<CustomerOrderResponse>
    {
        public Guid FoodId { get; }
        public string CustomerName { get; }

        public CustomerOrderRequest(Guid foodId, string customerName)
        {
            FoodId = foodId;
            CustomerName = customerName;
        }
    }

    public struct CustomerOrderResponse
    {
        public Guid OrderId { get; }
        public DateTimeOffset PingTime { get; }

        public CustomerOrderResponse(Guid orderId, DateTimeOffset pingTime)
        {
            OrderId = orderId;
            PingTime = pingTime;
        }
    }
}