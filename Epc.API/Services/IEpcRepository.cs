using Epc.API.Entities;
using Epc.API.Helpers.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Services
{
    public interface IEpcRepository
    {

        #region Users

        User GetUser(Guid userId);

        User GetUserByEmailAddress(string email);

        User GetUserByRememberToken(Guid rememberToken);

        void UpdateUser(Guid userId);

        PagedList<User> GetUsers(UsersResourceParameters usersResourceParameters);
        
        #endregion

        bool Save();

    }
}
