using Epc.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Services
{
    public interface IEpcRepository
    {
        User GetUserByEmailAddress(string email);

    }
}
