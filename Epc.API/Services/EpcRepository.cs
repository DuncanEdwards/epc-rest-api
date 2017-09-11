﻿using Epc.API.Entities;
using System;
using System.Linq;

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

        public User GetUserByEmailAddress(string email)
        {
           return  _epcContext.Users.FirstOrDefault(u => (u.Email == email));
        }

        #endregion

    }
}