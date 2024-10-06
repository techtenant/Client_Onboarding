using ClientOboarding.Dtos;
using ClientOboarding.Entities;
using ClientOboarding.Repositories.Interface;
using ClientOboarding.Services;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using ClientOboarding.Datas;

namespace ClientOboarding.Controllers
{
   [Route( "api/[controller]" )]
   [ApiController]
   public class AuthController : ControllerBase
   {
      private readonly IUserRepository _userRepository;
      private readonly ILogger<AuthController> _logger;
      private readonly JwtService _jwtService;

      public AuthController( IUserRepository userRepository, ILogger<AuthController> logger, JwtService jwtService )
      {
         _userRepository = userRepository;
         _logger = logger;
         _jwtService = jwtService;
      }

      [HttpPost( "signup" )]
      [ValidateModel]
      public async Task<IActionResult> SignUp( UserSignupDto userDto )
      {
         try
         {
            var existingUser = await _userRepository.GetByEmailAsync( userDto.Email );
            if( existingUser != null )
            {
               return BadRequest( "User already exists" );
            }

            var user = new User
            {
               Email = userDto.Email,
               PasswordHash = BCrypt.Net.BCrypt.HashPassword( userDto.Password )
            };

            await _userRepository.CreateAsync( user );
            return Ok( "User created successfully" );
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error during signup" );
            return StatusCode( 500, "An error occurred during signup" );
         }
      }

      [HttpPost( "login" )]
      [ValidateModel]
      public async Task<IActionResult> Login( UserLoginDto userDto )
      {
         try
         {
            var user = await _userRepository.GetByEmailAsync( userDto.Email );
            if( user == null || !BCrypt.Net.BCrypt.Verify( userDto.Password, user.PasswordHash ) )
            {
               return Unauthorized( "Invalid credentials" );
            }

            var token = _jwtService.GenerateToken( user );
            return Ok( new { Token = token } );
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error during login" );
            return StatusCode( 500, "An error occurred during login" );
         }
      }

      [HttpPost( "reset-password" )]
      public async Task<IActionResult> ResetPassword( string email )
      {
         try
         {
            var user = await _userRepository.GetByEmailAsync( email );
            if( user == null )
            {
               return NotFound( "User not found" );
            }

            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours( 24 );
            await _userRepository.UpdateAsync( user );

            // Send email with reset token (implementation not done)
            return Ok( "Password reset instructions sent" );
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error during password reset request" );
            return StatusCode( 500, "An error occurred during password reset request" );
         }
      }

      [HttpPost( "reset-password-confirm" )]
      [ValidateModel]
      public async Task<IActionResult> ResetPasswordConfirm( ResetPasswordDto resetPasswordDto )
      {
         try
         {
            var user = await _userRepository.GetByEmailAsync( resetPasswordDto.Email );
            if( user == null || user.ResetToken != resetPasswordDto.Token || user.ResetTokenExpiry < DateTime.UtcNow )
            {
               return BadRequest( "Invalid or expired reset token" );
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword( resetPasswordDto.NewPassword );
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _userRepository.UpdateAsync( user );

            return Ok( "Password reset successful" );
         }
         catch( Exception ex )
         {
            _logger.LogError( ex, "Error during password reset confirmation" );
            return StatusCode( 500, "An error occurred during password reset confirmation" );
         }
      }
   }
}