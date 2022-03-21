using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IdentityModel.Tokens.Jwt;
using RestSharp;

namespace DotNetCore
{
    public class NoParam
    {
        private string upbitAccessKey;
        private string upbitSecretKey;

        private DateTime dt_1970_01_01;
        private const string baseUrl = "https://api.upbit.com";

        public NoParam(string upbitAccessKey, string upbitSecretKey)
        {
            //APIClass에서 받은 키를 입력
            this.upbitAccessKey = upbitAccessKey;
            this.upbitSecretKey = upbitSecretKey;

            this.dt_1970_01_01 = new DateTime(1970, 01, 01);
        }

        public string Get(string path, Method method)
        {
            var tokenSb = JWT_NoParam();
            var token = tokenSb.ToString();

            tokenSb.Clear();
            tokenSb = null;

            var client = new RestClient(baseUrl);       // RestSharp 클라이언트 생성
            var request = new RestRequest(baseUrl + path, method);
            request.AddHeader("Content-Type", "application/json"); // 컨텐츠 타입이 json이라고 서버측에 알려줌
            request.AddHeader("Authorization", token); // 인증을 위해 JWT토큰을 넘겨줌

            token = null;

            var response = client.ExecuteAsync(request); // 요청을 서버측에 보내 응답을 받음

            response.Wait();

            try
            {
                if (response.IsCompletedSuccessfully)
                {
                    return response.Result.Content; 
                }
                else
                {
                    return null; 
                }
            }
            catch
            {
                return null; 
            }
        }


        public StringBuilder JWT_NoParam()
        {
            // 이 소스는 
            // https://docs.upbit.com/docs
            // 위 링크의 TABLE OF CONTENTS 참조
            TimeSpan diff = DateTime.Now - dt_1970_01_01;
            var nonce = Convert.ToInt64(diff.TotalMilliseconds);
            var payload = new JwtPayload {
                { "access_key", this.upbitAccessKey },
                { "nonce",  nonce  }
            };

            byte[] keyBytes = Encoding.Default.GetBytes(this.upbitSecretKey);
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes);
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, "HS256"); // HMAC SHA256 방식의 줄임말
            var header = new JwtHeader(credentials);
            var secToken = new JwtSecurityToken(header, payload);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(secToken);

            StringBuilder returnStr = new StringBuilder();
            returnStr.Append("Bearer "); // 띄어쓰기 한칸 있어야함
            returnStr.Append(jwtToken);

            return returnStr;
        }

    }
}
