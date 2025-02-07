using CatRescueApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

// This is a test fixture, a class that contains shared setup and teardown logic for tests.
public class TestFixture : IDisposable
{
    public ApplicationDbContext DbContext { get; }

    public TestFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // ensure each test uses a unique database instance
            .Options; // create a new instance of the DbContextOptionsBuilder

        DbContext = new ApplicationDbContext(options); // create a new instance of the ApplicationDbContext
        DbContext.Database.EnsureCreated(); // create the in-memory database
    }

    public void Dispose()
    {
        // clean up the in-memory database
        Console.WriteLine("Disposing DB...");
        DbContext.Database.EnsureDeleted(); // delete the in-memory database
        DbContext.Dispose(); // dispose of the in-memory database
        Console.WriteLine("DB disposed");
    }

}