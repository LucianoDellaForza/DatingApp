using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext    //DbContext predstavlja sesiju sa data bazom - preko entity frameworka saljemo query bazi koja vraca trazene podatke
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){ } 

        public DbSet<Value> Values { get; set; }      
    }
}