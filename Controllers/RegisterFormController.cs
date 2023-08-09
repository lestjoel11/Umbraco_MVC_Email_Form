using System.Net.Mail;
using EmailForm.Models;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;

namespace EmailForm.Controllers
{
    public class RegisterFormController : SurfaceController
    {
        private readonly ILogger<RegisterFormController> _logger;

        public RegisterFormController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, ILogger<RegisterFormController> logger) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _logger = logger;
        }

        [HttpPost]
        [ValidateUmbracoFormRouteString]
        public IActionResult SubmitForm(RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Failed to post data");
                return CurrentUmbracoPage();
            }
            _logger.LogInformation("Posting data");
            PostEmail(model);
            TempData["ContactSuccess"] = true;
            return RedirectToCurrentUmbracoPage();
        }

        private static void PostEmail(RegisterFormModel model)
        {
            MailMessage mailMessage = new("admin@example.com", model.EmailAddress)
            {
                Subject = "Successfuly Registered!",
                Body = string.Format("{0} {1} have successfuly been registered with the email {2}", model.FirstName, model.LastName, model.EmailAddress)
            };
            SmtpClient client = new("127.0.0.1", 25);
            client.Send(mailMessage);
        }
    }
}