using AngryRESTaurant.WebAPI.Model;
using AngryRESTaurant.WebAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngryRESTaurant.WebAPI.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AngryRESTaurant.WebAPI.Services
{
    public sealed class OrderingService
    {
        private readonly IRepository<FoodMenu> _foodMenuRepo;
        private readonly IRequestClient<OrderCreateRequest> _orderCreateClient;
        private readonly ILogger<OrderingService> _logger;

        public OrderingService(IRepository<FoodMenu> foodMenuRepo,
                IRequestClient<OrderCreateRequest> orderCreateClient,
                ILogger<OrderingService> logger)
        {
            this._foodMenuRepo = foodMenuRepo;
            this._orderCreateClient = orderCreateClient;
            this._logger = logger;
        }

        //TODO: Should have bundled in an object instead of just Guid , cook time needed also
        public async Task<CallOrderResponse> CustomerCallsOrderAsync(string customerName, string foodName)
        {
            _logger.LogInformation($"{customerName} wants to order {foodName}");

            if (string.IsNullOrEmpty(customerName))
                throw new ArgumentNullException(nameof(customerName));
            if (string.IsNullOrEmpty(foodName))
                throw new ArgumentNullException(nameof(foodName));

            // This can be consumer logic as well , but maybe not now
            var askedFood = this._foodMenuRepo
                    .Search(x => x.Name.Equals(foodName, StringComparison.OrdinalIgnoreCase));

            if (!askedFood.Any())
                throw new InvalidOperationException($"Customer {customerName} did not find {foodName}. He now finds another one");

            // If everything looks good , then customer talks to the waiter
            var (accepted, rejected) =
                await _orderCreateClient.GetResponse<OrderCreationAccepted, OrderCreationRejected>(
                    new
                        {
                            OrderId = Guid.NewGuid(),
                            CustomerName = customerName,
                            FoodName = foodName
                        });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;

                return new CallOrderResponse
                {
                    OrderId = response.Message.OrderId,
                    WaitTime = response.Message.ReadyTime,
                    OrderTaker = response.Message.OrderTaker
                };
            }




            var rejection = await rejected;
            throw new InvalidOperationException(
                $"We cannot take this order, sorry! {rejection.Message.Reason}");
        }

        public async Task<IEnumerable<FoodMenu>> AskForMenuAsync()
        {
            return await this._foodMenuRepo.QueryAllAsync();
        }
    }
}
