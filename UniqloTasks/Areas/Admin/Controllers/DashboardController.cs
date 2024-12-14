using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniqloTasks.Views.Account.Enums;

namespace UniqloTasks.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = nameof(Roles.Admin))]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
