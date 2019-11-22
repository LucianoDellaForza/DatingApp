using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username); //it either returns username that matches or null if it doesnt find it
            //above - Include(p=> p.Photos) ---> includes photoUrl in user (it returned null before this) 

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user; 
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));  // return byte[]
                for (int i = 0; i < computedHash.Length; i++)   //does computedHash match user's typed in password?
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }  
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt); //out - referenca direktna, nije kopija vrednosti

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);    //asinhrono dodavanje usera
            await _context.SaveChangesAsync();  //asinhrono cuvanje promena u bazi

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) {    //using(...) { sve sto je u viticastim se na kraju oslobadja iz memorije }
                passwordSalt = hmac.Key;    //randomly generated key (each time its run)
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //.ComputehASH zahteva byte[], pa preko UTF8 konvertujemo string password u byte[]
            }
            
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username)) //proverava sve usere da li je match sa datim userom
                return true;

            return false;
        }

    }
}