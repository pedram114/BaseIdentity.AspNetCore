using System.Threading.Tasks;
using AutoMapper;
using BaseApp.Identity.Model;
using BaseApp.Identity.Services.Interfaces;
using BaseApp.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Identity.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AccountsController(ApplicationDbContext  appDbContext,UserManager<ApplicationUser> userManager,IMapper mapper
        ,IUserService userService)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _userManager = userManager;
            _userService = userService;
        }
        [HttpGet("accounts")]
        public async Task<IActionResult> Get()
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
