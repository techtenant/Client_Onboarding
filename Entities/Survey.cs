using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClientOboarding.Entities
{
   [Table( "surveys", Schema = "client_onboarding" )]
   public class Survey
   {
      [Key]
      [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
      public Guid Id { get; set; }

      [ForeignKey( "User" )]
      public Guid UserId { get; set; }

      public User User { get; set; }

      [Column( TypeName = "text" )]
      public string Responses { get; set; } // JSON string of survey responses
   }
}
