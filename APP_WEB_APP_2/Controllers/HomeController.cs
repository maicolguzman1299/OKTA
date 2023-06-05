using APP_WEB_APP_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Okta.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace APP_WEB_APP_2.Controllers
{
    public class HomeController : Controller
    {

        private readonly IOktaClient _oktaClient;

        public HomeController(IOktaClient oktaClient = null)
        {
            _oktaClient = oktaClient;
        }

        public IActionResult Index()
        {
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Me()
        {
            var username = User.Claims
                .FirstOrDefault(x => x.Type == "preferred_username")
                ?.Value.ToString();

            var viewModel = new MeViewModel
            {
                Username = username,
                SdkAvailable = _oktaClient != null
            };

            if (!viewModel.SdkAvailable)
            {
                return View(viewModel);
            }

            if (!string.IsNullOrEmpty(username))
            {
                var user = await _oktaClient.Users.GetUserAsync(username);
                dynamic userInfoWrapper = new ExpandoObject();
                userInfoWrapper.Profile = user.Profile;
                userInfoWrapper.PasswordChanged = user.PasswordChanged;
                userInfoWrapper.LastLogin = user.LastLogin;
                userInfoWrapper.Status = user.Status.ToString();
                viewModel.UserInfo = userInfoWrapper;

                viewModel.Groups = (await user.Groups.ToList()).Select(g => g.Profile.Name).ToArray();
            }

            return View(viewModel);
        }
    }
}
