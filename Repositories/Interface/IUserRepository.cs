using ClientOboarding.Entities;

namespace ClientOboarding.Repositories.Interface
{
   public interface IUserRepository
   {
      Task<User> GetByIdAsync( int id );
      Task<User> GetByEmailAsync( string email );
      Task CreateAsync( User user );
      Task UpdateAsync( User user );
      Task DeleteAsync( int id );
   }
}
