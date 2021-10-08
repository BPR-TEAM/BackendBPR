﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SEP6.DB;

namespace SEP6.Tests.Integration
{
    public class DatabaseSeeder
    {
        public static MoviesDbContext MoviesDb;

        public static void SeedDatabase(MoviesDbContext db,bool isEmpty = false)
        {
            MoviesDb = db;
            MoviesDb.Database.EnsureCreated();
            MoviesDb.SaveChanges();

            if (isEmpty) return;
            
            SeedUsers();
            SeedMovies();
            
        }

        private static void SeedUsers()
        {
            var user = new User()
            {
                Id = 1,
                Birthday = "string",
                Country = "string",
                Email = "string",
                Name = "string",
                Password = "zLrOw8/K02No68FhB74dZ3c/U6O7m42VizjLIg1rB2Y=",
                Username = "string",
                Token = "ssss",
                Followers = new List<User>(),
                Follows = new List<User>(),
                PasswordSalt = new byte[16]{220,8,73,233,50,49,208,8,59,205,85,106,38,84,76,200}
            };
            var userFollower = new User()
            {
                Id = 2,
                Birthday = "string",
                Country = "string",
                Email = "string",
                Name = "string",
                Password = "string",
                Username = "stringy",
                Token = "hhhh",
                Followers = new List<User>(),
                Follows = new List<User>(),
                PasswordSalt = new byte[0]
            };
            user.Followers.Add(userFollower);
            MoviesDb.Users.AddRange(user, userFollower);
            MoviesDb.SaveChanges();
        }

        private static void SeedMovies()
        {
            var movie = new Movie() {Title = "Avengers: Age of Ultron", Id = 2395427, Year = 2015};
            MoviesDb.AddRange(movie);
           MoviesDb.AddRange(new Person() {Id = 751648, Name = "Joe Russo", Birth = 1971},
                new Person() {Id = 262635, Name = "Chris Evans", Birth = 1981});
            MoviesDb.SaveChanges();
            MoviesDb.Add(new TopLists()
            {
                Id = 1,
                UserId = 1,
                Name = "Boa Lista",
                Movies = new List<Movie>() {movie}

            });
            MoviesDb.Add(new Director() {PersonId = 751648, MovieId = 2395427});
            MoviesDb.SaveChanges();
        }
        
        private static void Clear<T>(DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}