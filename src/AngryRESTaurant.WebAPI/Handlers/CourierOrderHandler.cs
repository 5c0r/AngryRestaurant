using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AngryRESTaurant.WebAPI.Commands;
using AngryRESTaurant.WebAPI.Model;
using AngryRESTaurant.WebAPI.Repository;
using Baseline;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AngryRESTaurant.WebAPI.Handlers
{
    // Already injected using AddMediatR
    public sealed class CourierOrderHandler : IRequestHandler<CourierOrderRequest, CourierOrderResponse>
    {
        private readonly IRepository<FoodMenu> _menuRepository;
        private readonly ILogger<CourierOrderHandler> _logger;

        public CourierOrderHandler(IRepository<FoodMenu> menuRepository, ILogger<CourierOrderHandler> logger)
        {
            _menuRepository = menuRepository;
            _logger = logger;
        }

        public async Task<CourierOrderResponse> Handle(CourierOrderRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Input error
                ValidateOrderRequest(request);

                // Business rules error
                await ValidateFoodMenuAsync(request.FoodIds);


                return new CourierOrderResponse(true,
                    Guid.NewGuid(), DateTimeOffset.Now, 5, string.Empty);
            }
            catch (Exception e)
            {
                return new CourierOrderResponse(false,
                    Guid.Empty, DateTimeOffset.Now, 0, e.Message);
            }
        }

        private void ValidateOrderRequest(CourierOrderRequest request)
        {

        }

        private async Task ValidateFoodMenuAsync(IEnumerable<Guid> foodIds)
        {
            var results = await _menuRepository.GetByIdsAsync(foodIds);
            if (results.Count() != foodIds.Count())
            {
                throw new ValidationException("Invalid food list");
            }
        }
    }
}