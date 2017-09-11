﻿using Epc.API.Entities;
using Microsoft.EntityFrameworkCore;
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
           return  _epcContext.Users.Include("Type").FirstOrDefault(u => (u.Email == email));
        }

        #endregion

    }
}
