using Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class DepositService
    {
        private readonly DepositRepository _depositRepository;
        public DepositService(DepositRepository depositRepository)
        {
            _depositRepository = depositRepository;
        }

    }
}
