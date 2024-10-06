using ClientOboarding.Entities;

namespace ClientOboarding.Repositories.Interface
{
   public interface ISurveyRepository
   {
      Task<Survey> GetByIdAsync( int id );
      Task<IEnumerable<Survey>> GetByUserIdAsync( Guid userId );
      Task CreateAsync( Survey survey );
      Task UpdateAsync( Survey survey );
      Task DeleteAsync( int id );
   }
}
