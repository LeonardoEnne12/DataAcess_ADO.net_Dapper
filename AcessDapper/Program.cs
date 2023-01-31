using System;
using Microsoft.Data.SqlClient;
using Dapper;
using AcessDapper.Models;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace AcessDapper
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=Curso;User ID=sa;Password=1q2w3e4r@#$";

            using (var connection = new SqlConnection(connectionString))
            {
                // UpdateCategory(connection);
                // CreateCategory(connection);
                // DeleteCategory(connection);
                // CreateManyCategories(connection);
                // ListCategories(connection);
                // ExecuteProcedure(connection);
                // ExecuteReadProcedure(connection);
                // ExecuteScalar(connection);
                // ReadView(connection);
                // OneToOne(connection);
                // OneToMany(connection);
                // QueryMultiple(connection);
                // SelectIn(connection);
                // Like(connection,"api");
                Transaction(connection);
            }
            
        }
    
        static void ListCategories(SqlConnection connection)
        {
                var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");

                foreach(var item in categories)
                {
                    Console.WriteLine($"{item.Id} - {item.Title}");
                }
                
        }
        static void CreateCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            // Sql injection "category.Url = "INSERT INTO"; " tentativa de inserir algo pelo category.Url
            // Não se deve concatenar string

            //Se @Id fosse @teste linha 59 
            var insertSql = @"INSERT INTO [Category] 
                                Values(
                                    @Id, 
                                    @Title,
                                    @op,
                                    @Summary,
                                    @Order,
                                    @Description,
                                    @Featured)";

            var rows = connection.Execute(insertSql,new 
                {
                    category.Id, // teste = category.Id se tiver nome diferente
                    category.Title,
                    op = category.Url, // Na pratica
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                });
                Console.WriteLine($"{rows} - linhas inseridas");
        }
        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";

            var rows = connection.Execute(updateQuery,new
            {
                id = new Guid("03dea4fd-79a6-46df-aa69-511feabe745a"),
                title = "Amazon AWS2"
            });

            Console.WriteLine($"{rows} - registros atualizadas");
        }
        static void DeleteCategory(SqlConnection connection)
        {
            var updateQuery = "DELETE FROM [Category] WHERE [Id]=@id";

            var rows = connection.Execute(updateQuery,new
            {
                id = new Guid("7039dd86-5fee-480f-ab5a-dd4838dd1227")
            });

            Console.WriteLine($"{rows} - categorias deletadas");            
        }
        static void CreateManyCategories(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria Nova";
            category2.Url = "categoria-nova";
            category2.Description = "Categoria nova";
            category2.Order = 9;
            category2.Summary = "Categoria";
            category2.Featured = true;


            var insertSql = @"INSERT INTO [Category] 
                                Values(
                                    @Id, 
                                    @Title,
                                    @Url,
                                    @Summary,
                                    @Order,
                                    @Description,
                                    @Featured)"; 


            var rows = connection.Execute(insertSql,new[]
            {
                new
                {
                    category.Id, 
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                },

                new
                {
                    category2.Id, 
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured
                }
            });
            
            Console.WriteLine($"{rows} - linhas inseridas");
        }
        static void ExecuteProcedure(SqlConnection connection)
        {
            var procedure = "[spDeleteStudent]";

            var pars = new { StudentId = "1924f403-9bfe-4ec5-807f-227a0700470b" };

            var rows = connection.Execute(procedure, pars, commandType: CommandType.StoredProcedure);

            Console.WriteLine($"{rows} - linhas afetadas");

        }
        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "[spGetCoursesByCategory]";

            var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };

            var courses = connection.Query(procedure, pars, commandType: CommandType.StoredProcedure);

                foreach(var course in courses)
                {
                    Console.WriteLine($"{course.Id} - {course.Title}");
                }

        }
        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO [Category] OUTPUT inserted.[Id]
                                Values(
                                    NEWID(), 
                                    @Title,
                                    @Url,
                                    @Summary,
                                    @Order,
                                    @Description,
                                    @Featured)";

            var id = connection.ExecuteScalar<Guid>(insertSql,new 
                {
                    category.Title,
                    category.Url, 
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                });
                Console.WriteLine($"{id}");


        }
        static void ReadView(SqlConnection connection)
        {
            var sql = "SELECT * FROM [vwCourses]";
            var courses = connection.Query(sql);
            foreach(var item in courses)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }            
        }
        static void OneToOne(SqlConnection connection)
        {
            var sql = @"
                SELECT 
                    * 
                FROM 
                    [CareerItem] 
                INNER JOIN 
                    [Course] ON [CareerItem].[CourseId] = [Course].[Id]";

            var items = connection.Query<CareerItem, Course, CareerItem>(
                sql,
                (careerItem, course)=>{
                    careerItem.Course =course;
                    return careerItem;
                }, splitOn: "Id" //Essa parte é do curso "Acesso à dados com .NET, C#, Dapper e SQL Server (Imersão)"
            );

            
            foreach(var item in items)
            {
                Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
            } 

        }
        static void OneToMany(SqlConnection connection)
        {
            var sql = @"
            SELECT 
                [Career].[Id],
                [Career].[Title],
                [CareerItem].[CareerId],
                [CareerItem].[Title]
            FROM 
                [Career] 
            INNER JOIN 
                [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
            ORDER BY
                [Career].[Title]";

            var careers = new List<Career>();
            var items = connection.Query<Career,CareerItem,Career>(
                sql,
                (career, item)=>{

                    var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();
                    if (car == null)
                    {
                        car = career;
                        car.Items.Add(item);
                        careers.Add(car);
                    }
                    else
                    {
                        car.Items.Add(item);
                    }

                    return career;
                }, splitOn: "CareerId" //Essa parte é do curso "Acesso à dados com .NET, C#, Dapper e SQL Server (Imersão)"
            );

            
            foreach(var itens in careers)
            {
                Console.WriteLine($"{itens.Title} ");
                foreach(var item in itens.Items)
                {
                    Console.WriteLine($" - {item.Title} ");
                }
            } 
        }
        static void QueryMultiple(SqlConnection connection)
        {
            var query = "SELECT * FROM [Category];SELECT * FROM [Course]";

            using(var multi = connection.QueryMultiple(query))
            {
                var categories = multi.Read<Category>();
                var courses = multi.Read<Course>();

            foreach(var category in categories)
            {
                Console.WriteLine(category.Title);
            }

            foreach(var course in courses)
            {
                Console.WriteLine(course.Title);
            }

            }
        }
        static void SelectIn(SqlConnection connection)
        {
            var query = @"SELECT * FROM Career WHERE [Id] IN @Id";

            var items = connection.Query<Career>(query, new
            {
                Id = new[]{
                    "4327ac7e-963b-4893-9f31-9a3b28a4e72b",
                    "e6730d1c-6870-4df3-ae68-438624e04c72"
                }
            });

            foreach (var item in items)
            {
                Console.WriteLine(item.Title);
            }
         
        }
        static void Like(SqlConnection connection, string term)
        {
            var query = @"SELECT * FROM [Course] WHERE [Title] LIKE @exp";

            var items = connection.Query<Course>(query, new
            {
                exp = $"%{term}%" // %começa com term termina% com term %contem% o term
            });

            foreach (var item in items)
            {
                Console.WriteLine(item.Title);
            }
        }
        static void Transaction(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Minha categoria que não";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO 
                    [Category] 
                VALUES(
                    @Id, 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured)";

            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                var rows = connection.Execute(insertSql, new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                }, transaction);

                //transaction.Commit();
                transaction.Rollback();

                Console.WriteLine($"{rows} linhas inseridas");
            }
        }
    }
}
