using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MPApp.Core.Models;

namespace MPApp.Core
{
    public class TextRepository : IRepository
    {
        public async Task<List<Payment>> GetPaymentDataAsync(string fileName)
        {
            return Task.Run(() => GetPaymentData(fileName)).Result.ToList();
        }

        public async Task<List<RuleSet>> GetRulesAsync(string fileName)
        {
            return Task.Run(() => GetRules(fileName)).Result.ToList();
        }

        private IEnumerable<Payment> GetPaymentData(string fileName)
        {
            string line;
            List<Payment> payments = new List<Payment>();

            System.IO.StreamReader sr =
                new System.IO.StreamReader($@"c:\{fileName}.txt");
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (!String.IsNullOrWhiteSpace(line))
                {
                    string[] data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    yield return new Payment(Convert.ToDateTime(data[0]), data[1], Convert.ToDecimal(data[2]));
                }
            }

            sr.Close();

        }

        private IEnumerable<RuleSet> GetRules(string fileName)
        {
            string line;
            List<RuleSet> rules = new List<RuleSet>();

            System.IO.StreamReader sr =
                new System.IO.StreamReader($@"c:\{fileName}.txt");
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (!String.IsNullOrWhiteSpace(line))
                {
                    string[] data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    yield return new RuleSet(data[0], Convert.ToDecimal(data[1]), Convert.ToDecimal(data[2]));
                }
            }

            sr.Close();

        }
    }
}