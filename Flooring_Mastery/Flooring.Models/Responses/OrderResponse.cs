using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Responses
{
    public class OrderResponse : Response
    {
        public Order Order { get; set; }  //holds new order information
    }
}
