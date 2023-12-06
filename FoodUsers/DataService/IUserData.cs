using UserMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserMicroservice.DataService
{
    public interface IUserData
    {
        LoginResponse Authenticate(LoginRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
        ResponseGlobal Register(RegisterRequest model);
        void Update(int id, UpdateRequest model);
        void Delete(int id);
    }
}
