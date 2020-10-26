using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Interfaces
{
    public interface IProductsRepository
    {
        Products LoadProduct(string product);
        List<String> GetAllProducts();
        Dictionary<string, Products> GetAllProducts2();

        
    }
}
