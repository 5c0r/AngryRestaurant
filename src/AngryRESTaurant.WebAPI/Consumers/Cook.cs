using System;
using System.Threading.Tasks;
using AngryRESTaurant.WebAPI.Contracts;
using AngryRESTaurant.WebAPI.Model;
using AngryRESTaurant.WebAPI.Repository;
using AngryRESTaurant.WebAPI.Utils;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AngryRESTaurant.WebAPI.Consumers
{
    // I can only do "fast food"
    public sealed class Cook : IConsumer<FastFoodOrdered>
    {
        private readonly ILogger<Cook> _logger;
        private readonly IRepository<OrderStatus> _orderRepository;

        private readonly string name = "Gordon";

        public Cook(ILogger<Cook> logger, IRepository<OrderStatus> orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<FastFoodOrdered> context)
        {
            // TODO: Error handling ( validation , domain error )
            // TODO: Validation orderId name foodname ?1
            // But this should be business/domain error only , as those things are already took care of in validation
            _logger.Log(LogLevel.Debug, $"{name} grabbed a cook order from queue!");
            _orderRepository.UpsertAsync(new OrderStatus()
            {
                Id = context.Message.OrderId,
                FoodName = context.Message.FoodName,
                ExpectedReadyTime = DateTimeOffset.Now.AddSeconds(5),
                Status = OrderStatuses.Cooking
            });

            await Task.Delay(TimeSpan.FromSeconds(RandomNumber.RandomInt()));

            _logger.Log(LogLevel.Debug, $"{name} is done! He shouted to the Server-queue {context.Message.FoodName}!!");

            // It's looks messy to put here, maybe that's why a "state machine" looks better IMNSHO
            _orderRepository.UpsertAsync(new OrderStatus()
            {
                Id = context.Message.OrderId,
                FoodName = context.Message.FoodName,
                ExpectedReadyTime = DateTimeOffset.Now.AddSeconds(5),
                Status = OrderStatuses.ReadyToServed
            });

            // TODO: Not sure if this is correct
            await context.Publish<FoodReady>(new
            {
                OrderId = context.Message.OrderId,
                CustomerName = context.Message.CustomerName,
                FoodName = context.Message.FoodName,
                ReadyTime = DateTimeOffset.UtcNow
            });
        }
    }

    // I can only do "a la carte" . But you will not be used now
    public sealed class SlowFoodCooker : IConsumer<SlowFoodOrdered>
    {
        private readonly ILogger<Cook> _logger;
        private readonly IRepository<OrderStatus> _orderRepository;

        private readonly string CookName = "Not Gordon";

        public SlowFoodCooker(ILogger<Cook> logger, IRepository<OrderStatus> orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<SlowFoodOrdered> context)
        {
            _logger.LogDebug($"{CookName} going to cook an order !");
            await Task.Delay(TimeSpan.FromSeconds(20));

            _logger.LogDebug($"{CookName} finished an order {context.Message.OrderId} !");

            // TODO: Not sure if this is correct . Test this out first
            await context.Publish<FoodReady>(new
            {
                context.Message.OrderId,
                context.Message.CustomerName,
                context.Message.FoodName,
                DateTimeOffset.UtcNow
            });
        }
    }
}
