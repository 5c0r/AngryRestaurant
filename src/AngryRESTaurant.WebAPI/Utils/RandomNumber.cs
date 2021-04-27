using System;

namespace AngryRESTaurant.WebAPI.Utils
{
    public class RandomNumber
    {
        public static int RandomInt()
        {
            return new Random().Next(0, 10);
        }
    }
}