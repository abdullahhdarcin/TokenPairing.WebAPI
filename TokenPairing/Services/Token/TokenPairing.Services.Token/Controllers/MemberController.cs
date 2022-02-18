using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenPairing.Services.Token.Data;
using TokenPairing.Services.Token.Model;
using TokenPairing.Services.Token.Shared.ControllerBases;
using TokenPairing.Services.Token.Shared.Dtos;

namespace TokenPairing.Services.Token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : CustomBaseController
    {
        private readonly IDataHelper _dataHelper;

        public MemberController(IDataHelper dataHelper)
        {
            _dataHelper = dataHelper;
        }

        [HttpGet]
        [Route("{memberid}")]
        public  List<TokenModel> GetTokens(string memberid)
        {
            try
            {
                return _dataHelper.GetTokenByMemberId(memberid);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public IActionResult UpdateToken(string memberId, string empush_token, string customer_id)
        {
            var token = _dataHelper.UpdateToken(memberId, empush_token, customer_id);

            return StatusCode(token.StatusCode, token);

        }

    }
}
