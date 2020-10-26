using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Interfaces
{
    public interface IStateTaxRepository
    {
        StateTax LoadState(string state);
        List<string> GetAllStateTax();
    }
}
