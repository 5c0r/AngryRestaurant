using System.Threading.Tasks;
using AngryRESTaurant.WebAPI.Contracts;
using AngryRESTaurant.WebAPI.Model;
using AngryRESTaurant.WebAPI.Repository;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AngryRESTaurant.WebAPI.Consumers
{
    public sealed class Waiter : IConsumer<OrderCreateRequest>
    {
        private readonly ILogger<Waiter> _logger;
        private readonly IRepository<FoodMenu> _foodMenuRepo;
        private readonly string Name = "Bill";

        public Waiter(ILogger<Waiter> logger, IRepository<FoodMenu> foodMenuRepo)
        {
            _logger = logger;
            _foodMenuRepo = foodMenuRepo;
        }

        public async Task Consume(ConsumeContext<OrderCreateRequest> context)
        {
            _logger?.Log(LogLevel.Debug, $" Getting order from {context.Message.CustomerName}");

            if (context.RequestId != null)
            {

                _logger?.Log(LogLevel.Debug, $"This guy happily took every order ");
                await context.RespondAsync<OrderCreationAccepted>(new
                {
                    OrderId = context.Message.OrderId,
                    CustomerName = context.Message.CustomerName,
                    ReadyTime = 4,
                    OrderTaker = this.Name
                });

                await context.Publish<FastFoodOrdered>(new
                {
                    OrderId = context.Message.OrderId,
                    CustomerName = context.Message.CustomerName,
                    FoodName = context.Message.FoodName
                });

            }
        }
    }
}
