using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngryRESTaurant.WebAPI.Contracts
{
    public interface OrderCancelRequest
    {
        public string Reason { get; }
    }
}
