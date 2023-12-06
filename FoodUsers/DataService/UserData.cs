using UserMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;
using UserMicroservice.Authorization;
using MySqlConnector;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace UserMicroservice.DataService
{
    public class UserData : IUserData
    {
        private IJwtUtils _jwtUtils;

        private readonly IConfiguration Configuration;

        public UserData(
           IJwtUtils jwtUtils,  IConfiguration _configuration)
        {
            _jwtUtils = jwtUtils;
            Configuration = _configuration;

        }


        private MySqlConnection GetConnection()
        {
            string connString = Configuration.GetConnectionString("Default");
            return new MySqlConnection(connString);
        }
        public LoginResponse Authenticate(LoginRequest model)
        {
            var user = GetUserByUsername(model.Username);

            // validate
            if (user == null || !BCryptNet.Verify(model.Password, user.UserPassword))
                return new LoginResponse { rspns = new ResponseGlobal { Message="Login failed!", rspEnum =ResponseEnum.BadRequest} };

            // authentication successful
            var response = new LoginResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Id = user.U_Id,
                rspns = new ResponseGlobal
                {
                    Message = "Successfull login!",
                    rspEnum = ResponseEnum.OK
                }
                
            };
   
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }
        public ResponseGlobal Register(RegisterRequest model)
        {
            int result = 0;
            // validate
            if (existUsername(model.Username))
                return new ResponseGlobal
                {
                    rspEnum = ResponseEnum.BadRequest,
                    Message = "Username '" + model.Username + "' is already taken"
                };
            // hash password
            model.Password = BCryptNet.HashPassword(model.Password);


            using (MySqlConnection conn = GetConnection())
            {
                string insertQuery = @"INSERT INTO foodproject.user
                                    (
                                    Username,
                                    UserPassword,
                                    FirstName,
                                    LastName,
                                    Mail)
                                    VALUES
                                    (@Username, @Password, @FirstName,@LastName, @Mail);";

                 result = conn.Execute(insertQuery, model);
            }

            if (result != 0)
            {
                return new ResponseGlobal
                {
                    rspEnum = ResponseEnum.OK,
                    Message = "Successfull registration!"
                };
            }

            return new ResponseGlobal
            {
                rspEnum = ResponseEnum.BadRequest,
                Message = "System error!"
            };

        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }


        public void Update(int id, UpdateRequest model)
        {
            throw new NotImplementedException();
        }
        public User GetUserByUsername(string username)
        {
            using var con = GetConnection();
            var user = con.Query<User>("SELECT * FROM user WHERE username = '" + username + "'").ToList();
            return user.FirstOrDefault();
        }

        public bool existUsername(string username)
        {
            using var con = GetConnection();
            var count = con.Execute("select  count(*)  from foodproject.user where user.Username ='" + username + "'");
            return count > 1;
        }
    }
}
