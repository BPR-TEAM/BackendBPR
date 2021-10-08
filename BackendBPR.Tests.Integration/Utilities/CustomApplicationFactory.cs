﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SEP6.DB;
using TMDbLib.Client;

namespace SEP6.Tests.Integration.Utilities
{
    public class CustomApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
    {
        private SqliteConnection Connection;
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                Connection = new SqliteConnection("DataSource=:memory:");
                Connection.Open();

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<MoviesDbContext>));
                services.Remove(descriptor);

                services.AddDbContext<MoviesDbContext>(options =>
                {
                    options.UseSqlite(Connection);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MoviesDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomApplicationFactory<TStartup>>>();

                    
                    db.Database.EnsureCreated();
                    try
                    {
                        DatabaseSeeder.SeedDatabase(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Connection.CloseAsync();
        }
    }
}