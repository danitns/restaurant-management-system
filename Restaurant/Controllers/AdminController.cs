using Microsoft.AspNetCore.Mvc;
using Restaurant.BusinessLogic.Implementation.Admin;
using Restaurant.Web.Code.Base;

namespace Restaurant.Web.Controllers
{
	public class AdminController : BaseController
	{
		private readonly AdminService Service;

		public AdminController(ControllerDependencies dependencies, AdminService service) : base(dependencies)
		{
			Service = service;
		}

		public async Task<IActionResult> Index()
		{
			var model = await Service.GetAdminDashboardData();
			return View(model);
		}

		public async Task<IActionResult> UpdateUserRole(Guid userId, bool isApproved)
		{
			await Service.UpdateUserRole(userId, isApproved);
			return RedirectToAction("Index");
		}
	}
}
