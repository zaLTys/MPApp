using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPApp.Core
{
    public class FeeCalculator
    {
        private readonly IRepository _repository;

        public FeeCalculator(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Payment>> LoadData()
        {
            return await _repository.GetPaymentData();
        }
    }
}