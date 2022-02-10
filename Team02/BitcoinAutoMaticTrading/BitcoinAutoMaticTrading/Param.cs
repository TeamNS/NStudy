using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using RestSharp;

namespace BitcoinAutoMaticTrading
{
    public class Param
    {
        private string upbitAccessKey;
        private string upbitSecretKey;

        private DateTime dt_1970_01_01;   //timestamp를 계산하기 위한 변수
        private const string baseUrl = "https://api.upbit.com";


        public Param(string upbitAccessKey, string upbitSecretKey)
        {
            //APIClass에서 받은 키를 입력
            this.upbitAccessKey = upbitAccessKey;
            this.upbitSecretKey = upbitSecretKey;

            this.dt_1970_01_01 = new DateTime(1970, 01, 01);
        }

        public string Get(string path, Dictionary<string, string> parameters, Method method)
        {

            StringBuilder queryStringSb = GetQueryString(parameters);

            var tokenSb = JWT_param(queryStringSb.ToString()); // 입력받은 변수를 JWT토큰으로 변환
            var token = tokenSb.ToString();

            queryStringSb.Insert(0, "?");      // 링크에 ?를 붙임으로 파라미터를 사용한다는 의미
            queryStringSb.Insert(0, path);

            var client = new RestClient(baseUrl);       // RestSharp 클라이언트 생성
            var request = new RestRequest(queryStringSb.ToString(), method);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", token);

            queryStringSb.Clear(); queryStringSb = null;
            tokenSb.Clear(); tokenSb = null;
            parameters.Clear(); parameters = null;

            var response = client.ExecuteAsync(request);

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
        public StringBuilder GetQueryString(Dictionary<string, string> parameters)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in parameters)
            {
                builder.Append(pair.Key).Append("=").Append(pair.Value).Append("&");
            }

            if (builder.Length > 0)
            {
                builder.Length = builder.Length - 1; // 마지막 &를 제거하기 위함.
            }
            return builder;
        }
        public StringBuilder JWT_param(string queryString)
        {

            SHA512 sha512 = SHA512.Create();
            byte[] queryHashByteArray = sha512.ComputeHash(Encoding.UTF8.GetBytes(queryString));
            string queryHash = BitConverter.ToString(queryHashByteArray).Replace("-", "").ToLower();

            TimeSpan diff = DateTime.Now - dt_1970_01_01;
            var nonce = Convert.ToInt64(diff.TotalMilliseconds);

            var payload = new JwtPayload
                    {
                        { "access_key", this.upbitAccessKey },
                        { "nonce", nonce  },
                        { "query_hash", queryHash },
                        { "query_hash_alg", "SHA512" }
                    };

            byte[] keyBytes = Encoding.Default.GetBytes(this.upbitSecretKey);
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes);
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, "HS256");
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
