using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    //ovo je ustvari entity framework i svaku novu klasu za bazu moramo obavestiti ovaj framework
    public class DataContext : DbContext    //DbContext predstavlja sesiju sa data bazom - preko entity frameworka saljemo query bazi koja vraca trazene podatke
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){ } 
       
        //ovde dodajem "migracije" ---> Da bi napravio novi table u sqlite bazi kuca se u terminalu posebno za svaku migraciju, sto onda u folderu Migrations izbaci sta se radilo
        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }     
        public DbSet<Photo> Photos { get; set; } 
    }
}