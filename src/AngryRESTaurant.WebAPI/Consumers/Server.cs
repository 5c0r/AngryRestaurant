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
    // I am server, I BRING FOOD to the customer , or to the freaking doorkickers
    // But only if I have the fooood
    public class Server : IConsumer<FoodReady>
    {
        private readonly ILogger<Server> _logger;
        private readonly IRepository<OrderStatus> _orderRepository;
        private readonly string name = "Andy";

        public Server(ILogger<Server> logger, IRepository<OrderStatus> orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<FoodReady> context)
        {
            // TODO: I am still unsure why we had to validate message here...

            _logger.LogDebug($"{name} grab the food, bringing the food to customer");
            // Server is running to give the order to..customer

            await Task.Delay(TimeSpan.FromSeconds(RandomNumber.RandomInt()));

            await _orderRepository.UpsertAsync(new OrderStatus()
            {
                Id = context.Message.OrderId,
                FoodName = context.Message.FoodName,
                ExpectedReadyTime = DateTimeOffset.Now.AddSeconds(5),
                Status = OrderStatuses.Served
            });

            _logger.LogDebug($"{name} served the fooooood !! Hurray !");

            await Task.CompletedTask;
        }
    }
}
