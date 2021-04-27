using System;
using System.Collections.Generic;
using MediatR;

namespace AngryRESTaurant.WebAPI.Commands
{
    public struct CourierOrderRequest : IRequest<CourierOrderResponse>
    {
        public string CourierName { get; }
        public string CourierService { get; }
        public IEnumerable<Guid> FoodIds { get; }

        public CourierOrderRequest(string courierName, string courierService, IEnumerable<Guid> foodIds)
        {
            CourierName = courierName;
            CourierService = courierService;
            FoodIds = foodIds;
        }
    }

    public readonly struct CourierOrderResponse
    {
        public bool IsSuccess { get; }
        public Guid OrderTrackingId { get; }
        public DateTimeOffset ExpectedTime { get; }
        public int MarginalMinute { get; }
        public string Note { get; }

        public CourierOrderResponse(bool isSuccess, Guid orderTrackingId, DateTimeOffset expectedTime, int marginalMinute, string note)
        {
            IsSuccess = isSuccess;
            OrderTrackingId = orderTrackingId;
            ExpectedTime = expectedTime;
            MarginalMinute = marginalMinute;
            Note = note;
        }
    }
}