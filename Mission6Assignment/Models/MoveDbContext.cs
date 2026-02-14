using Microsoft.EntityFrameworkCore;

namespace Mission6Assignment.Models;

public class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) //constructor
    {
    }
    
    public DbSet<AddMovie> Movies { get; set; }
}