using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApp.Services
{
    public class AuthService
    {
        public static byte[] Key = Encoding.Default.GetBytes(Guid.NewGuid().ToString());


        public string? Authenticate(string login, string password)
        {
            if(login == "admin" && password == "admin")
            {

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, "Read"),
                    new Claim(ClaimTypes.Role, "Delete")
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor();
                tokenDescriptor.Subject = new ClaimsIdentity(claims);
                tokenDescriptor.Expires = DateTime.Now.AddMinutes(30);
                tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature);

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            return null;
        }

    }
}
