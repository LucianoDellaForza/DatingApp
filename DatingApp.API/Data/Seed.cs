using System.Collections.Generic;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            this._context = context;
        }

        public void SeedUsers()
        {
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("password", out passwordHash, out passwordSalt); //"password" je direktno ubaceno (umesto da uzmem od usera) jer ionako je to sifra za svakog usera    
           
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {    //using(...) { sve sto je u viticastim se na kraju oslobadja iz memorije }
                passwordSalt = hmac.Key;    //randomly generated key (each time its run)
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //.ComputehASH zahteva byte[], pa preko UTF8 konvertujemo string password u byte[]
            }
            
        }
    }
}