using Epc.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Services
{
    public interface IEpcRepository
    {
        User GetUser(Guid userId);

        User GetUserByEmailAddress(string email);

        User GetUserByRememberToken(Guid rememberToken);

        void UpdateUser(Guid userId);

        bool Save();

    }
}
