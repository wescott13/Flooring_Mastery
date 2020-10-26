using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models.Interfaces;
using Flooring.Models.Responses;

namespace Flooring.BLL.Rules
{
    public class AddOrderRules
    {
        public static string OrderDate(DateTime date)
        {
            DateTime pDate = DateTime.Today;
            if (date <= pDate)
            {
                string message = "Error:  Date must be in the future.";
                return message;
            }
            else
            {
                string message = "";
                return message;
            }
        }
        public static string OrderArea(decimal area)
        {
            decimal minArea = 100;
            if (area < minArea)
            {
                string message = "Error:  Area must be greater than 100 Square Feet.";
                return message;
            }
            else
            {
                string message = "";
                return message;
            }
        }
    }
}
