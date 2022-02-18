using System.Collections.Generic;
using System.Threading.Tasks;
using TokenPairing.Services.Token.Model;
using TokenPairing.Services.Token.Shared.Dtos;

namespace TokenPairing.Services.Token.Data
{
    public interface IDataHelper
    {
        List<TokenModel> GetTokenByMemberId(string member);

        Response<TokenModel> UpdateToken(string memberId, string empush_token, string customer_id);
    }
}
