using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Authentication;

namespace application.transformers;

public static class JwtDecoder
{
    

    public static string DecodeJwtEmail(string authHeader)
    {
        
        var token = authHeader.ToString().Split(' ').Last();
        if (string.IsNullOrEmpty(token))
        {
            throw new AuthenticationException();
        }
        
        try
        {
            // Decode the JWT
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Retrieve the email claim
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (emailClaim == null)
            {
                throw new AuthenticationException();
            }

            return emailClaim;
        }
        catch (Exception ex)
        {
            throw new AuthenticationException();
        }
        
        
    }
}