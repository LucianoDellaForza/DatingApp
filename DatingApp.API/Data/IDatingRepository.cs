using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class; //for adding User or Photo //Add method of type T (User, Photo npr), that takes T entity as paramater where T is type of class (just in case)
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();   //when we save changes to db, there will be 0 changes or more than 0 changes
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);

        Task<Photo> GetMainPhotoForUser(int userId);
    }
}