using Microsoft.AspNetCore.Mvc;
using TokenPairing.Services.Token.Shared.Dtos;

namespace TokenPairing.Services.Token.Shared.ControllerBases
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
