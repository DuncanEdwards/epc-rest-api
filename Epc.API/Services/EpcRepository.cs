using Epc.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Epc.API.Helpers.Paging;
using Epc.API.Helpers;

namespace Epc.API.Services
{
    public class EpcRepository : IEpcRepository
    {

        #region Private fields

        EpcContext _epcContext;

        #endregion

        #region Public Constructor

        public EpcRepository(EpcContext epcContext)
        {
            _epcContext = epcContext;
        }
        
        #endregion

        #region Public Methods

        #region Users

        public User GetUser(Guid userId)
        {
            return _epcContext.Users.Include("Type").FirstOrDefault(u => (u.Id == userId));
        }

        public User GetUserByEmailAddress(string email)
        {
           return  _epcContext.Users.Include("Type").FirstOrDefault(u => (u.Email == email));
        }

        public User GetUserByRememberToken(Guid rememberToken)
        {
            return _epcContext.Users.Include("Type").FirstOrDefault(u => (u.RememberToken == rememberToken));
        }

        public void UpdateUser(Guid userId)
        {

        }

        public PagedList<User> GetUsers(UsersResourceParameters usersResourceParameters) 
        {
            //Apply sort first
            var collectionBeforePaging =
                _epcContext.Users.Include("Type").ApplySort(usersResourceParameters.OrderBy);

            //Surname filter
            if (!String.IsNullOrEmpty(usersResourceParameters.Type))
            {
                var whereClause = usersResourceParameters.Type.Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging.Where(u => u.Type.Name == whereClause);
            }

            //Search
            if (!String.IsNullOrEmpty(usersResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause = usersResourceParameters.SearchQuery.Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(u =>
                        (u.Email.ToLowerInvariant().Contains(searchQueryForWhereClause) ||
                          u.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause) ||
                          u.Surname.ToLowerInvariant().Contains(searchQueryForWhereClause)));
            }

            //Page results
            return PagedList<User>.Create(
                collectionBeforePaging,
                usersResourceParameters.PageNumber,
                usersResourceParameters.PageSize);
        }

        #endregion

        public bool Save()
        {
            return (_epcContext.SaveChanges() >= 0);
        }




        #endregion

    }
}
