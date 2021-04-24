using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngryRESTaurant.WebAPI.Model
{
    public interface IHaveGuid
    {
        public Guid Id { get; set; }
    }

    public sealed class FoodMenu : IHaveGuid, IFoodMenuPayload
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDifficult { get; set; }
        public double CookingTime { get; set; }
    }

    public interface IFoodMenuPayload
    {
        public string Name { get; set; }
        public bool IsDifficult { get; set; }
        public double CookingTime { get; set; }
    }

    public sealed class FoodMenuInputModel : IFoodMenuPayload
    {
        public string Name { get; set; }
        public bool IsDifficult { get; set; }
        public double CookingTime { get; set; }
    }
}