using Epc.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Epc.API.Entities
{
    public static class EpcContextExtensions
    {

        #region Private Methods

        private static void CreateTestUsersIfNecessary(EpcContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            //Users
            var users = new List<User>() {
                new User()
                {
                    Id = new Guid("f49cae7e-a80f-4288-b825-91f6f23f6476"),
                    Type = context.UserTypes.First(userType => userType.Code == "ADMIN"),
                    FirstName = "Duncan",
                    Surname = "Edwards",
                    Email = "dun_edwards@yahoo.com",
                    Password = PasswordHelper.HashPassword("duncan1"),
                    RememberToken = null,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                },
                new User()
                {
                    Id = new Guid("5f8b723d-1013-4bd1-bffa-4a120cdd7df2"),
                    Type = context.UserTypes.First(userType => userType.Code == "AGENT"),
                    FirstName = "Bart",
                    Surname = "Simpson",
                    Email = "bart_simpson@yahoo.com",
                    Password = PasswordHelper.HashPassword("damnedifyoudo"),
                    RememberToken = null,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                },
                new User()
                {
                    Id = new Guid("0875ec38-f812-43c3-a7ed-0336cd940162"),
                    Type = context.UserTypes.First(userType => userType.Code == "AGENT"),
                    FirstName = "Lisa",
                    Surname = "Simpson",
                    Email = "lisa_simpson@yahoo.com",
                    Password = PasswordHelper.HashPassword("iloveschool"),
                    RememberToken = null,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                },
                 new User()
                {
                    Id = new Guid("05cd4786-634a-41f3-87e4-af52817a2b3f"),
                    Type = context.UserTypes.First(userType => userType.Code == "AGENT"),
                    FirstName = "Marge",
                    Surname = "Simpson",
                    Email = "marge_simpson@yahoo.com",
                    Password = PasswordHelper.HashPassword("bluehair"),
                    RememberToken = null,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

        }

        private static void RepopulateUserTypesIfNecessary(EpcContext context)
        {
            if (context.UserTypes.Any())
            {
                return;
            }

            //User Types
            var userTypes = new List<UserType>()
            {
                new UserType()
                {
                    Id = Guid.NewGuid(),
                    Code = "ADMIN",
                    Name = "Administrator"
                },
                new UserType()
                {
                    Id = Guid.NewGuid(),
                    Code = "AGENT",
                    Name = "Agent"
                },
                new UserType()
                {
                    Id = Guid.NewGuid(),
                    Code = "ASSESSOR",
                    Name = "Assessor"
                }
            };
            context.UserTypes.AddRange(userTypes);
        }


        #endregion

        #region Extension Methods


        public static void EnsureSeedDataForContext(this EpcContext context)
        {
            context.Database.EnsureCreated();
            RepopulateUserTypesIfNecessary(context);
            CreateTestUsersIfNecessary(context);
            context.SaveChanges();
        }

        #endregion

    }
}
