using System;
using BackendBPR.Database;
using Microsoft.EntityFrameworkCore;

namespace BackendBPR.Tests.Integration.Utilities
{
    public class DatabaseSeeder
    {
        public static OrangeBushContext OBDB;

        public static void SeedDatabase(OrangeBushContext db,bool isEmpty = false)
        {
            OBDB = db;
            OBDB.Database.EnsureCreated();
            OBDB.SaveChanges();

            if (isEmpty) return;
            //Seeds here
            SeedUsers();
            // SeedMovies();
            
        }

        private static void SeedUsers()
        {
            var user = new User()
            {
                Id = 1,
                Birthday = new DateTime(2000,3,4),
                Country = "string",
                Email = "string",
                FirstName = "string",
                PasswordHash = "zLrOw8/K02No68FhB74dZ3c/U6O7m42VizjLIg1rB2Y=",
                Username = "string",
                Token = "ssss",
                PasswordSalt = new byte[16]{220,8,73,233,50,49,208,8,59,205,85,106,38,84,76,200}
            };

            OBDB.Users.Add(user);
            OBDB.SaveChanges();
        }
        
        private static void Clear<T>(DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}