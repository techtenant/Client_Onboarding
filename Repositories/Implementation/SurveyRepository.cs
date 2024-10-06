using ClientOboarding.Datas;
using ClientOboarding.Entities;
using ClientOboarding.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClientOboarding.Repositories.Implementation
{
   public class SurveyRepository : ISurveyRepository
   {
      private readonly ApplicationDbContext _context;
      private readonly ILogger<SurveyRepository> _logger;

      public SurveyRepository( ApplicationDbContext context, ILogger<SurveyRepository> logger )
      {
         _context = context;
         _logger = logger;
      }

      public async Task<Survey> GetByIdAsync( int id )
      {
         try
         {
            return await _context.Surveys.FindAsync( id );
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error retrieving survey by ID" );
            throw;
         }
      }

      public async Task<IEnumerable<Survey>> GetByUserIdAsync( Guid userId )
      {
         try
         {
            return await _context.Surveys.Where( s => s.UserId == userId ).ToListAsync();
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error retrieving surveys by user ID" );
            throw;
         }
      }

      public async Task CreateAsync( Survey survey )
      {
         try
         {
            await _context.Surveys.AddAsync( survey );
            await _context.SaveChangesAsync();
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error creating survey" );
            throw;
         }
      }

      public async Task UpdateAsync( Survey survey )
      {
         try
         {
            _context.Surveys.Update( survey );
            await _context.SaveChangesAsync();
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error updating survey" );
            throw;
         }
      }

      public async Task DeleteAsync( int id )
      {
         try
         {
            var survey = await _context.Surveys.FindAsync( id );
            if( survey != null )
            {
               _context.Surveys.Remove( survey );
               await _context.SaveChangesAsync();
            }
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error deleting survey" );
            throw;
         }
      }
   }

}
