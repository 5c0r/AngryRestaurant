using System;
using System.Threading.Tasks;
using AngryRESTaurant.WebAPI.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AngryRESTaurant.WebAPI.Consumers
{
    // I am an Ignore Waiter, I don't want to take order that has Rabbit inside it
    public sealed class IgnorantWaiter : IConsumer<OrderCreateRequest>
    {
        private readonly ILogger<IgnorantWaiter> _logger;
        private readonly Random _random = new Random();
        private readonly string Name = "Charlie";

        public IgnorantWaiter(ILogger<IgnorantWaiter> logger)
        {
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<OrderCreateRequest> context)
        {
            _logger?.Log(LogLevel.Debug, $" Getting order from {context.Message.CustomerName}");

            if (context.Message.FoodName.Contains("rabbit", StringComparison.OrdinalIgnoreCase))
            {
                if (context.RequestId != null)
                {
                    _logger?.Log(LogLevel.Debug,
                        $"Ignorant waiter don't want people to eat Rabbit. So he reject any order");
                    await context.RespondAsync<OrderCreationRejected>(new
                    {
                        OrderId = context.Message.OrderId,
                        Reason = "Rabbit is not for eating"
                    });
                }
            }

            if (context.RequestId != null)
            {
                _logger?.Log(LogLevel.Debug, $"This guy now took an order ");
                await context.RespondAsync<OrderCreationAccepted>(new
                {
                    OrderId = context.Message.OrderId,
                    CustomerName = context.Message.CustomerName,
                    ReadyTime = _random.Next(5,10),
                    OrderTaker = this.Name
                });
            }
        }
    }
}
