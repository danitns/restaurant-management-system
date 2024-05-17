using Microsoft.AspNetCore.Authentication;
using Restaurant.Common.DTOs;
using System.Security.Claims;

namespace Restaurant.Web.Code.Utils
{
    static public class LoginUtils
    {
        public static async Task LogIn(CurrentUserDTO user, HttpContext httpContext)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.MobilePhone, user.Phone)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(
                    scheme: "RestaurantCookies",
                    principal: principal);
        }

        public static async Task LogOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(scheme: "RestaurantCookies");
        }
    }
}
