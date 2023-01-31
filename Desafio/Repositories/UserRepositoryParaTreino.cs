using System;
using System.Collections.Generic;
using Blog.Models;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;

namespace Blog.Repositories
{
    public class UserRepositoryParaTreino
    {
        private readonly SqlConnection _connection;

        // public UserRepository(SqlConnection connection)
        // {
        //     _connection = connection;
        // }
        // igual o de cima 
        public UserRepositoryParaTreino(SqlConnection connection)
            => _connection = connection;

        public IEnumerable<User> Get()
            => _connection.GetAll<User>();

        public User Get(int id)
            => _connection.Get<User>(id);
        
        public void Create(User user)
            => _connection.Insert<User>(user);

        public void Update(User user)
        {
            if (user.Id != 0)
                _connection.Update<User>(user);
        }  

        public void Delete(User user)
        {
            if (user.Id != 0)
                _connection.Delete<User>(user);
            
        }    

        public void Delete(int id)
        {
            if (id != 0)
                return;
                
            var user = _connection.Get<User>(id);    
            _connection.Delete<User>(user);
            
        }             

    }    
}