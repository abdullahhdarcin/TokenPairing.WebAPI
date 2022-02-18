using System.Collections.Generic;
using System.Threading.Tasks;
using TokenPairing.Web.Models;

namespace TokenPairing.Web.Services.Interfaces
{
    public interface IMemberService
    {
        Task<List<TokenViewModel>> GetTokenByMemberId(string member);

        Task<Response<TokenViewModel>> UpdateToken(TokenViewModel tokenViewModel);
    }
}
