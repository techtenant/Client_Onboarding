using ClientOboarding.Datas;
using ClientOboarding.Entities;
using ClientOboarding.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClientOboarding.Repositories.Implementation
{
   public class UserRepository : IUserRepository
   {
      private readonly ApplicationDbContext _context;
      private readonly ILogger<UserRepository> _logger;

      public UserRepository( ApplicationDbContext context, ILogger<UserRepository> logger )
      {
         _context = context;
         _logger = logger;
      }

      public async Task<User> GetByIdAsync( int id )
      {
         try
         {
            return await _context.Users.FindAsync( id );
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error retrieving user by ID" );
            throw;
         }
      }

      public async Task<User> GetByEmailAsync( string email )
      {
         try
         {
            return await _context.Users.FirstOrDefaultAsync( u => u.Email == email );
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error retrieving user by email" );
            throw;
         }
      }

      public async Task CreateAsync( User user )
      {
         try
         {
            await _context.Users.AddAsync( user );
            await _context.SaveChangesAsync();
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error creating user" );
            throw;
         }
      }

      public async Task UpdateAsync( User user )
      {
         try
         {
            _context.Users.Update( user );
            await _context.SaveChangesAsync();
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error updating user" );
            throw;
         }
      }

      public async Task DeleteAsync( int id )
      {
         try
         {
            var user = await _context.Users.FindAsync( id );
            if( user != null )
            {
               _context.Users.Remove( user );
               await _context.SaveChangesAsync();
            }
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error deleting user" );
            throw;
         }
      }
   }
}
