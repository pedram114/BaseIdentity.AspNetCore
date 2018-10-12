using System.IO;
using System.Reflection;
using BaseApp.Identity.Model;
 using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace BaseApp.Identity
 {
     public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
     {
         public ApplicationDbContext(DbContextOptions options) : base(options)
         {
                 
         }
         public static string GetConnectionString()  
         {  
             return Startup.ConnectionString;  
         }
         public DbSet<ExternalData> ExternalData { set; get; }
         public DbSet<ApplicationRole> ApplicationRole { set; get; }
         public DbSet<AccessAction> AccessActions { set; get; }
      
     }
     public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
     {
         public ApplicationDbContext CreateDbContext(string[] args)
         {
             var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
             builder.UseSqlServer(Startup.ConnectionString,
                 optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name));

             return new ApplicationDbContext(builder.Options);
         }
     }
    
 }