using System.Collections.Generic;
using System.Linq;
using Blog.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Blog.Repositories
{
    public class UserRepository : Repository<User> // Já tem classe base Repository<User> genérica, agora cria uma específica para User
    {
        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection) : base(connection)// base(connection) "Repository<User>" chama o construtor da 
            => _connection = connection;                                  //classe base que precisa da conexão

        public List<User> ReadWithRole()
        {
            var query = @"
                SELECT
                    [User].*,
                    [Role].*
                FROM
                    [User]
                    LEFT JOIN [UserRole] ON [UserRole].[UserId] = [User].[Id]
                    LEFT JOIN [Role] ON [UserRole].[RoleId] = [Role].[Id]";

            var users = new List<User>();
            var items = _connection.Query<User, Role, User>(// cria a lista de Roles
                query,
                (user, role) =>
                {
                    var usr = users.FirstOrDefault(x => x.Id == user.Id);
                    if (usr == null)
                    {
                        usr = user;
                        if (role != null)
                            usr.Roles.Add(role);
                        users.Add(usr);
                    }
                    else
                        usr.Roles.Add(role);

                    return user;
                }, splitOn: "Id");
            return users;
        }
    }
}