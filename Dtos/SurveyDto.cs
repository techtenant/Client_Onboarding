namespace ClientOboarding.Dtos
{
   public class SurveyDto
   {
      public Guid Id { get; set; }
      public Guid UserId { get; set; }
      public Dictionary<string, string>? Responses { get; set; }
   }
}
