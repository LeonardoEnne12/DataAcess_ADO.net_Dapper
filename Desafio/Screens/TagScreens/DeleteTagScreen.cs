using System;
using Blog.Models;
using Blog.Repositories;

namespace Blog.Screens.TagScreens
{
    public static class DeleteTagScreen
    {
        public static void Load()
        {
            Console.Clear();
            Console.WriteLine("Excluir uma tag");
            Console.WriteLine("--------------");
            
            Console.Write("Id: ");
            var id = Console.ReadLine();
            
            
            Delete(new Tag {
                Id = int.Parse(id)
            });
            Console.ReadKey();
            MenuTagScreen.Load();
        }

        private static void Delete(Tag id)
        {
            try
            {
                var repository = new Repository<Tag>(Database.Connection);

                repository.Delete(id);

                Console.WriteLine("Tag excluída!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Não foi possível excluir a tag");
                Console.WriteLine(ex.Message);
            }
        }
    }
}