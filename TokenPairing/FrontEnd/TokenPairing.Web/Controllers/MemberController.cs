using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenPairing.Web.Models;
using TokenPairing.Web.Services.Interfaces;

namespace TokenPairing.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public IActionResult Index(string MEMBER_ID)
        {
            if (string.IsNullOrWhiteSpace(MEMBER_ID))
            {
                return View();
            }

            //var result = await _memberService.GetTokenByMemberId(MEMBER_ID);

            return RedirectToAction("Response", MEMBER_ID);
        }

        public async Task<IActionResult> Response(string MEMBER_ID)
        {
            if (string.IsNullOrWhiteSpace(MEMBER_ID))
            {
                return RedirectToAction("Index");
            }

            var result = await _memberService.GetTokenByMemberId(MEMBER_ID);

            return View(result);
        }
        public async Task<IActionResult> Update(TokenViewModel tokenViewModel)
        {
            if (string.IsNullOrWhiteSpace(tokenViewModel.MEMBER_ID) || string.IsNullOrWhiteSpace(tokenViewModel.CUSTOMER_ID) || string.IsNullOrWhiteSpace(tokenViewModel.EMPUSH_TOKEN))
            {
                return View();
            }

            var result = await _memberService.UpdateToken(tokenViewModel);

            if (result.StatusCode == 400)
            {
                return View("Error", result);
            }

            if(result.StatusCode == 422)
            {
                return View("Error", result);
            }

            if (result.StatusCode == 404)
            {
                return View("Error", result);
            }
            else

                return View("Paired");
        }

        public IActionResult Paired(TokenViewModel tokenViewModel)
        {

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }



    }
}
