using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace application.transformers;

public static class JwtDecoder
{
    

    public static string DecodeJwtEmail(string AuthHeader)
    {
        
        var token = AuthHeader.ToString().Split(' ').Last();
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception();
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
                throw new Exception();
            }

            return emailClaim;
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
        
        
    }
}