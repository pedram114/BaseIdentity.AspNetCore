using System.Threading.Tasks;
using AutoMapper;
using BaseApp.Identity.Model;
using BaseApp.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Identity.Api
{
    [Route("api/[controller]")]
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AccountController(ApplicationDbContext  appDbContext,UserManager<ApplicationUser> userManager,IMapper mapper
        ,IUserService userService)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _userManager = userManager;
            _userService = userService;
        }
        [HttpGet]
        [Route("accounts")]
        public async Task<IActionResult> Accounts()
        {
            var customers =await _userService.GetUsersTaskAsync();
            return new OkObjectResult(new
                {
                    customers

                }
            );
        }

    }
}
