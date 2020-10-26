using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;
using Flooring.Models.Interfaces;
using Flooring.Models.Responses;

namespace Flooring.Data.Data
{
    public class StateTaxesRepository : IStateTaxRepository
    {
        string stateTaxesPath = @".\Taxes.txt";
        Dictionary<string, StateTax> taxes;
        public StateTaxesRepository()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            string dirTaxes = @".\Taxes";
            Directory.SetCurrentDirectory(dirTaxes);

            taxes = new Dictionary<string, StateTax>();
            var fileStateTaxes = File.ReadAllLines(stateTaxesPath);
            foreach (var row in fileStateTaxes)
            {
                string[] columns = row.Split(',');
                
                try
                {
                    StateTax stateTaxes = new StateTax();
                    stateTaxes.StateAbbreviation = columns[0];
                    stateTaxes.StateName = columns[1];
                    stateTaxes.TaxRate = Convert.ToDecimal(columns[2]);

                    taxes.Add(stateTaxes.StateAbbreviation, stateTaxes);  //stateAbbreviation is my key
                }
                catch (Exception ex)
                {        
                }
            }
        }
        public List<string> GetAllStateTax()
        {
            return taxes.Keys.ToList();  //simplifies foreach
        }

        public StateTax LoadState(string state)
        {
            if (taxes.ContainsKey(state))
            {
                return taxes[state];
            }
            else
            {
                return null;     
            }
        }
    }
}

