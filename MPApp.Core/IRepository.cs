﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MPApp.Core.Models;

namespace MPApp.Core
{
    public interface IRepository
    {
        public Task<List<Payment>> GetPaymentDataAsync(string fileName);
        public Task<List<RuleSet>> GetRulesAsync(string fileName);
    }
}