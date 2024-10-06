using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClientOboarding.Entities
{
   [Table( "users", Schema ="client_onboarding" )]
   public class User
   {
      [Key]
      [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
      public Guid Id { get; set; }

      [Required]
      [EmailAddress]
      [Column( TypeName = "varchar(255)" )]
      public string Email { get; set; }

      [Required]
      [Column( TypeName = "varchar(255)" )]
      public string PasswordHash { get; set; }

      [Column( TypeName = "varchar(255)" )]
      public string ResetToken { get; set; }

      public DateTime? ResetTokenExpiry { get; set; }
   }
}
