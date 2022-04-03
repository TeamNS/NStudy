using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCoreService.Models
{
    public class JwtManager
    {
        // 암호문으로 HMACSHA256 방식으로 생성된 메시지 인증코드를 사용한다.
        private static readonly string baseSecret = "6Oifg2JwpBo285P5utb+tyH8TShGdZ/5lSwMfnYyIbTz6tP4E16odrF0WxTCWhk/udKJGj+Z9Fp63bO/JpSZNw==";

        public static string GenerateToken(string username)
        {
            // 암호문을 바이트 배열로 변환
            byte[] secretArray = Convert.FromBase64String(baseSecret);

            // 암호를 바탕으로 대칭키 알고리즘에 사용될 보안키 객체 생성.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secretArray);

            // 토큰 생성에 필요한 클레임과 서명 방식을 기술하는 객체
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                // ClaimsIdentity(Claim[] claims) 생성자 호출
                Subject = new ClaimsIdentity(
                    // 클레임 배열
                    new[] {
                        // Claim(string type, string value) 클레임의 유형과 값을 이용하여 클레임 객채를 생성한다.
                        // ClaimTypes는 클레임의 이름이 의미하는 바를 통일하도록 미리 정한 것이다. 
                        // ClaimTypes.Name은 수혜자의 이름을 나타내기 위한 클레임 유형이다.
                        new Claim(ClaimTypes.Name, username)}),
                Expires = DateTime.UtcNow.AddMinutes(30),

                // JWT의 검증서명을 만드는데 필요한 객체를 지정한다. 보안키, 알고리즘이 지정되었다
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            // JWT 처리자 객체로 토큰 객체를 생성하고 검증하는 역할.
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            // 토큰 객체 생성
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            // 토큰 객체를 직렬화하여 반환.
            return handler.WriteToken(token);
        }

        private static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken == null)
                    return null;

                byte[] secretArray = Convert.FromBase64String(baseSecret);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(secretArray)
                };

                SecurityToken securityToken;

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);

                return principal;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static string ValidateToken(string token)
        {
            ClaimsPrincipal entity = GetPrincipal(token);

            if (entity == null) return null;

            ClaimsIdentity identity;

            try
            {
                identity = (ClaimsIdentity)entity.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            // Claim nameClaim = entity.FindFirst(ClaimTypes.Name); 도 동일한 효과
            Claim nameClaim = identity.FindFirst(ClaimTypes.Name);

            return nameClaim.Value;

        }
    }
}
