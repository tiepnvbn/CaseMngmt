using CaseMngmt.Models.Account;
using CaseMngmt.Models.ApplicationRoles;
using CaseMngmt.Models.ApplicationUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMngmt.Models.Database
{
    internal class DbInitializer
    {
        internal static void Initialize(ApplicationDbContext dbContext)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            dbContext.Database.EnsureCreated();
            if (dbContext.Users.Any()) return;

            //var users = new User[]
            //{
            //new User{ Id = 1, Name = "Bruce Wayne" }
            ////add other users
            //};

            //foreach (var user in users)
            //    dbContext.Users.Add(user);

            //dbContext.SaveChanges();
        }
    }
}
