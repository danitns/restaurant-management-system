using Restaurant.BusinessLogic.Base;
using Restaurant.BusinessLogic.Implementation.Users;
using Restaurant.Common.DTOs;
using Restaurant.Web.Code.Base;
using System.Security.Claims;

namespace Restaurant.Web.Code.ExtensionMethods
{
    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddScoped<ControllerDependencies>();

            return services;
        }

        public static IServiceCollection AddTicketAppBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ServiceDependencies>();
            services.AddScoped<UserService>();
            return services;
        }

        public static IServiceCollection AddTicketAppCurrentUser(this IServiceCollection services)
        {
            services.AddScoped(s =>
            {

                var accessor = s.GetService<IHttpContextAccessor>();
                var httpContext = accessor.HttpContext;
                if (httpContext != null)
                {
                    var claims = httpContext.User.Claims;

                    var userIdClaim = claims?.FirstOrDefault(c => c.Type == "Id")?.Value;
                    var isParsingSuccessful = Guid.TryParse(userIdClaim, out Guid id);
                    var emailClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var roleClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                    return new CurrentUserDTO
                    {
                        Id = id,
                        IsAuthenticated = httpContext.User.Identity.IsAuthenticated,
                        Name = httpContext.User.Identity.Name,
                        Email = emailClaim,
                        Role = roleClaim,

                    };
                }
                return new CurrentUserDTO() { };
            });

            return services;
        }
    }
}
