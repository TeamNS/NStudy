using System.Collections.Generic;
using AccountData;
using AccountModels;
using JWT.Generator;
using Microsoft.AspNetCore.Mvc;


namespace Login.Controllers
{
    [Route("Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly AccountRepo _repository = new AccountRepo();

        [HttpPost("Regist")]
        public ActionResult<TokenInfo> RegistAccount(AccountInfo info)
        {
            if (_repository.RegistAccount(info.ID, info.PW))
            {
                var accountInfo = _repository.GetAccountInfo(info.ID);
                var accountResult = new TokenInfo()
                {
                    Token = new JwtGenerator().GenerateJwtToken(info.ID)
                };

                return Ok(new { message = "Regeist Success" });

            }
            else
            {
                return Ok(new { message = "Regeist fail" });
            }
        }

        [HttpPost]
        public ActionResult<TokenInfo> LoginAccount(AccountInfo info)
        {
            var accountInfo = _repository.GetAccountInfo(info.ID);
            if (accountInfo != null)
            {
                if (accountInfo.PW.CompareTo(info.PW) != 0)
                {
                    return Unauthorized();
                }

                var tokenInfo = new TokenInfo()
                {
                    Token = new JwtGenerator().GenerateJwtToken(info.ID)
                };

                return Ok(tokenInfo);

            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult <IEnumerable<AccountInfo>> GetAccountList()
        {          
            return Ok(_repository.GetAccountList());
        }
    }
}