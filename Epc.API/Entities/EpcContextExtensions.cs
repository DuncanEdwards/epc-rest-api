using Csv;
using Epc.API.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Epc.API.Entities
{
    public static class EpcContextExtensions
    {

        #region Private Methods

        private static void CreateTestUsersIfNecessary(EpcContext context)
        {

            Random random = new System.Random();

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
                }
            };

            var usersFromCsv = CsvReader.ReadFromText(File.ReadAllText(@"Data Scripts/Users.csv"));
            foreach (var userFromCsv in usersFromCsv)
            { 
                var randomAgentCode = ((random.Next(0, 2) == 0) ? "AGENT" : "ASSESSOR");
                var user = new User()
                {
                    Id = new Guid(userFromCsv["id"]),
                    Type = context.UserTypes.FirstOrDefault(userType => userType.Code == randomAgentCode),
                    FirstName = userFromCsv["first_name"],
                    Surname = userFromCsv["last_name"],
                    Email = userFromCsv["email"],
                    Password = PasswordHelper.HashPassword(userFromCsv["password"]),
                    RememberToken = null,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                    IsActivated = true
                };
                users.Add(user);
            }

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
            context.SaveChanges();
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
