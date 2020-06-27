using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPApp.Core
{
    public interface IRepository
    {
        public Task<List<Payment>> GetPaymentData();
    }
}