using ClientOboarding.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClientOboarding.Datas
{
   public class ApplicationDbContext : DbContext
   {
      public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options )
          : base( options )
      {
      }

      public DbSet<User> Users { get; set; }
      public DbSet<Survey> Surveys { get; set; }
   }
}
