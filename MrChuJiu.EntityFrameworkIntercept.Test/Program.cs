using Microsoft.EntityFrameworkCore;
using MrChuJiu.EntityFrameworkIntercept.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MrChuJiu.EntityFrameworkIntercept.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            ContextConfig.Configure(build =>
            {
                build
                    .IgnoreEntity<BlogTwo>()
                    .IgnoreProperty<BlogTwo>(x => x.Name)
                    .IgnoreTable("Blogs")
                    .IgnoreProperty("Name");

            });
            using (var context = new BlogsContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Add(
                    new Blog
                    {
                        Id = 1,
                        Name = "EF Blog"
                    });

                context.SaveChanges();
            }
            using (var context = new BlogsContext())
            {
                var blog = context.Blogs.Single();
                blog.Name = "EF Core Blog";
                context.SaveChanges();
            }
            using (var context = new BlogsContext())
            {
                context.Add(
                     new BlogOne
                     {
                         Id = 1,
                         Name = "EF Blog",
                         Title = "EF Blog Title"
                     });

                context.SaveChanges();
            }
            using (var context = new BlogsContext())
            {
                context.Add(
                     new BlogTwo
                     {
                         Id = 1,
                         Name = "EF Blog",
                         Title = "EF Blog Title",
                         Content = "EF Blog Content"
                     });

                context.SaveChanges();
            }
    }
    }

    public class BlogsContext : InterceptDbContext
    {
        public BlogsContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DataSource=blogs.db");

        public override Task AfterIntercept(List<ChangeEntry> ChangeEntry)
        {
            foreach (var item in ChangeEntry)
            {
                switch (item.EntityState)
                {
                    case EntityState.Deleted:
                        Console.WriteLine($"{item.TableName} 执行命令删除  本次被执行的字段 { JsonConvert.SerializeObject(item.NewValue) }");
                        break;
                    case EntityState.Modified:
                        Console.WriteLine($"{item.TableName} 执行命令修改  本次被执行的字段 { JsonConvert.SerializeObject(item.usedValue) } == { JsonConvert.SerializeObject(item.NewValue) } ");
                        break;
                    case EntityState.Added:
                        Console.WriteLine($"{item.TableName} 执行命令添加  本次被执行的字段 { JsonConvert.SerializeObject(item.NewValue) }");
                        break;
                }
            }
            return Task.CompletedTask;
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogOne> BlogOnes { get; set; }
        public DbSet<BlogTwo> BlogTwos { get; set; }

    }
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class BlogOne
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
    public class BlogTwo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
