using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Web.Mvc;
using App.Models.Project;
using App.Services;
using App.Utilities;
using MongoDB.Bson;

namespace App.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        public ProjectController(IProjectService projectService, IUserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        [HttpGet]
        [Route("projects")]
        public ActionResult Listing()
        {
            string ownerEmail = CookieManager.GetAuthenticatedUserDetails().Email;
            List<ProjectListModel> projects = _projectService.Get(ownerEmail);
            return Json(new { projects = projects }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("project/new")]
        public ActionResult Create(ProjectModel model)
        {
            string ownerEmail = CookieManager.GetAuthenticatedUserDetails().Email;
            var projectId = _projectService.Create(model);
            _userService.GetUser(ownerEmail);
            _projectService.AddMember(projectId, _userService.GetUser(ownerEmail));
            return Json(new { projectId = projectId }, JsonRequestBehavior.AllowGet);
        }
    }
}