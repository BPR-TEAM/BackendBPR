using System.Collections.Generic;
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
            SeedPlantsWithTags();  
            SeedMeasurementDefintionsAndUserPlant();  
            SeedAdvices();
            SeedDashboard();            
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

         private static void SeedPlantsWithTags()
        {
            var tag = new Tag {
              Id = 1,
              Name = "Passion Fruit"
            };

            var tag2 = new Tag {
              Id = 2,
              Name = "Passion Banana Fruit"
            };

            OBDB.Tags.AddRange(tag,tag2);

            var plant = new Plant()
            {
                Id = 1,
                CommonName = "Common Name",
                ScientificName = "Plant",
                Url = "go here",
                Description = "yo thats a plant that exists in places and lives a certain lifespan",
                Tags = new List<Tag>(){
                    tag, tag2
                }
            };

            var plant1 = new Plant()
            {
                Id = 2,
                CommonName = "Non-Ass Name",
                ScientificName = "Planties",
                Url = "go here",
                Description = "yo thats a plant that exists in places and lives a certain lifespan",
                Tags = new List<Tag>(){
                    tag
                }
            };

            OBDB.Plants.AddRange(plant,plant1);

            OBDB.SaveChanges();
        }
        
        private static void SeedMeasurementDefintionsAndUserPlant(){
            var plant = new UserPlant(){
                Id = 5,
                UserId = 1,
                PlantId = 1,
                Name = "Banana Passion Fruit"                
            };

            OBDB.UserPlants.Add(plant);

            var co2 = new MeasurementDefinition(){
                Name = "CO2",
                Id = 1,
                PlantId = 1
            };

             var humidity = new CustomMeasurementDefinition(){
                Name = "Humidity",
                Id = 2,
                PlantId = 1,
                UserPlantId = 5
            };
        
            OBDB.MeasurementDefinitions.AddRange(co2,humidity);
            OBDB.SaveChanges();
        }

        private static void SeedAdvices(){            
            var advice = new Advice(){
                Description = "Water tour plants regularly",
                TagId = 1,
                Id = 1
            };

            var generalAdvice = new Advice(){
                Description = "Water tour plants regularly",
                Id = 2
            };

            var generalAdvice1 = new Advice(){
                Description = "6-8h of sun regularly",
                Id = 3
            };

             var generalAdvice2 = new Advice(){
                Description = "6-8h of sun regularly",
                Id = 4
            };

            var userAdvice = new UserAdvice(){
                UserId = 1,
                AdviceId = 1,
                Type = AdviceRole.Creator
            };

            var userAdvice1 = new UserAdvice(){
                UserId = 1,
                AdviceId = 2,
                Type = AdviceRole.Like
            };

            var userAdvice2 = new UserAdvice(){
                UserId = 1,
                AdviceId = 3,
                Type = AdviceRole.Like
            };

            OBDB.Advices.AddRange(advice,generalAdvice,generalAdvice1,generalAdvice2);
            OBDB.UserAdvices.AddRange(userAdvice,userAdvice1,userAdvice2);
            OBDB.SaveChanges();
        }

        private static void SeedDashboard(){
            var plant = new UserPlant(){
                Id = 4,
                UserId = 1,
                PlantId = 1,
                Name = "Man Passion Fruit"                
            };
            var board = new Board(){
                PlantId = 1,
                DashboardId = 1,
                Type = "CO2-BarGraph"
            };
            var dashboard = new Dashboard(){
                Id = 1,
                UserId = 1,
                UserPlants = new List<UserPlant>(){plant},
                Name = "My garden",
                Description = "Garden that I own in the kitchen"
            };
            OBDB.UserPlants.Add(plant);
            OBDB.Dashboards.Add(dashboard);
            OBDB.Boards.Add(board);
            OBDB.SaveChanges();
        }

        
        private static void Clear<T>(DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}