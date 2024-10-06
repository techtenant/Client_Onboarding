using ClientOboarding.Dtos;
using ClientOboarding.Entities;
using ClientOboarding.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ClientOboarding.Controllers
{
   [Route( "api/[controller]" )]
   [ApiController]
   public class SurveyController : ControllerBase
   {
      private readonly ISurveyRepository _surveyRepository;
      private readonly ILogger<SurveyController> _logger;

      public SurveyController( ISurveyRepository surveyRepository, ILogger<SurveyController> logger )
      {
         _surveyRepository = surveyRepository;
         _logger = logger;
      }

      [HttpPost]
      public async Task<IActionResult> SubmitSurvey( SurveyDto surveyDto )
      {
         try
         {
            var survey = new Survey
            {
               UserId = surveyDto.UserId,
               Responses = JsonSerializer.Serialize( surveyDto.Responses )
            };

            await _surveyRepository.CreateAsync( survey );
            return Ok( "Survey submitted successfully" );
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error submitting survey" );
            return StatusCode( 500, "An error occurred while submitting the survey" );
         }
      }
   }
}
