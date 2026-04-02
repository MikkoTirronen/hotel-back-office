using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence;

public class HotelDbContextFactory : IDesignTimeDbContextFactory<HotelDbContext>
{
    public HotelDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=HotelDb;User Id=sa;Password=Super@86secret;TrustServerCertificate=True;");
        return new HotelDbContext(optionsBuilder.Options);
    }
}