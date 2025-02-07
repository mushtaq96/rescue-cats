using CatRescueApi.Data;
using Microsoft.EntityFrameworkCore;

// This is a test fixture, a class that contains shared setup and teardown logic for tests.
public class TestFixture
{
    //  entry point for creating isolated DbContext instances
    public ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // ensure each test uses a unique database instance
            .Options; // create a new instance of the DbContextOptionsBuilder

        var context = new ApplicationDbContext(options); // create a new instance of the ApplicationDbContext
        context.Database.EnsureCreated(); // create the in-memory database
        return context;
    }

}