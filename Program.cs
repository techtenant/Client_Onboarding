
using ClientOboarding.Datas;
using ClientOboarding.Repositories.Implementation;
using ClientOboarding.Repositories.Interface;
using ClientOboarding.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace ClientOboarding
{
   public class Program
   {
      public static void Main( string[] args )
      {
         var builder = WebApplication.CreateBuilder( args );

         // Add services to the container.

         builder.Services.AddControllers();
         // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
         builder.Services.AddDbContext<ApplicationDbContext>( options =>
    options.UseNpgsql( builder.Configuration.GetConnectionString( "DefaultConnection" ) ) );
         builder.Services.AddScoped<IUserRepository, UserRepository>();
         builder.Services.AddScoped<ISurveyRepository, SurveyRepository>();
         builder.Services.AddSingleton<JwtService>();
         builder.Services.AddEndpointsApiExplorer();
         builder.Services.AddSwaggerGen();
         builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
    .AddJwtBearer( options =>
    {
       options.TokenValidationParameters = new TokenValidationParameters
       {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = builder.Configuration[ "Jwt:Issuer" ],
          ValidAudience = builder.Configuration[ "Jwt:Audience" ],
          IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( builder.Configuration[ "Jwt:Key" ] ) )
       };
    } );
         builder.Services.AddRateLimiter( options =>
         {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>( context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                       AutoReplenishment = true,
                       PermitLimit = 10,
                       QueueLimit = 0,
                       Window = TimeSpan.FromMinutes( 1 )
                    } ) );
         } );

         var app = builder.Build();

         // Configure the HTTP request pipeline.
         if( app.Environment.IsDevelopment() )
         {
            app.UseSwagger();
            app.UseSwaggerUI();
         }

         app.UseHttpsRedirection();

         app.UseAuthorization();


         app.MapControllers();

         app.Run();
      }
   }
}
