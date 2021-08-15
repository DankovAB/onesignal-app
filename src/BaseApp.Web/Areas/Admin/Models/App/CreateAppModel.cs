using BaseApp.Common;
using BaseApp.Common.Extensions;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Infrastructure.LogOn;
using BaseApp.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseApp.Web.Areas.Admin.Models.App
{
    public class CreateAppModel : ValidatableModelBase
    {
        [Required]
        [Display(Name = "The name of app",
            Description = "Required: The name of your new app, as displayed on your apps list on the dashboard. This can be renamed later.")]
        public string Name { get; set; }

        [Display(Name = "Enviroment",
            Description = "iOS: Either sandbox or production")]
        public Enums.ApnsEnvironmentTypes? ApnsEnv { get; set; }

        [Display(Name = "iOS p12 certificate",
            Description = "iOS: Your apple push notification p12 certificate file, converted to a string and Base64 encoded.")]
        public IFormFile ApnsP12 { get; set; }

        [Display(Name = "iOS p12 certificate password",
            Description = "iOS Required if adding p12 certificate - Password for the apns_p12 file")]
        public string ApnsP12Password { get; set; }

        [Display(Name = "Server Auth Key",
            Description = "Android: Your FCM Google Push Server Auth Key")]
        public string GcmKey { get; set; }

        [Display(Name = "Project number",
            Description = "Android: Your FCM Google Project number. Also know as Sender ID.")]
        public string AndroidGcmSenderId { get; set; }

        [Display(Name = "The website URL",
            Description = "Chrome (All Browsers except Safari) (Recommended): The URL to your website. This field is required if you wish to enable web push and specify other web push parameters.")]
        public string ChromeWebOrigin { get; set; }
        
        [Display(Name = "Chrome notification icon",
            Description = "Chrome (All Browsers except Safari): Your default notification icon. Should be 256x256 pixels, min 80x80.")]
        public string ChromeWebDefaultNotificationIcon { get; set; }

        [Display(Name = "Subdomain",
            Description = "Chrome (All Browsers except Safari): A subdomain of your choice in order to support Web Push on non-HTTPS websites. This field must be set in order for the chrome_web_gcm_sender_id property to be processed.")]
        public string ChromeWebSubDomain { get; set; }
        
        [Display(Name = "The Site Name",
            Description = "All Browsers (Recommended): The Site Name. Requires both chrome_web_origin and safari_site_origin to be set to add or update it.")]
        public string SiteName { get; set; }
        
        [Display(Name = "The Hostname",
            Description = "Safari (Recommended): The hostname to your website including http(s)://")]
        public string SafariSiteOrigin { get; set; }
        
        [Display(Name = "Safari p12 certificate file",
            Description = "Safari: Your apple push notification p12 certificate file for Safari Push Notifications, converted to a string and Base64 encoded.")]
        public IFormFile SafariApnsP12 { get; set; }

        [Display(Name = "Safari p12 certificate password",
            Description = "Safari: Password for safari_apns_p12 file")]
        public string SafariApnsP12Password { get; set; }

        [Display(Name = "Safari notification icon",
            Description = "Safari: A url for a 256x256 png notification icon. This is the only Safari icon URL you need to provide.")]
        public string SafariIcon256256 { get; set; }

        [Display(Name = "Messaging Auth Key",
            Description = "Not for web push. Your Google Push Messaging Auth Key if you use Chrome Apps / Extensions.")]
        public string ChromeKey { get; set; }

        [Display(Name = "Notification data",
            Description = "iOS: Notification data (additional data) values will be added to the root of the apns payload when sent to the device. Ignore if you're not using any other plugins or not using OneSignal SDK methods to read the payload.")]
        public string AdditionalDataIsRootPayload { get; set; }

        [Display(Name = "The Organization Id",
            Description = "The Id of the Organization you would like to add this app to.")]
        public string OrganizationId { get; set; }

        protected override IEnumerable<ValidationResult> Validate(IUnitOfWork unitOfWork, Func<LoggedUserForValidationModel> getLoggedUser, ValidationContext validationContext)
        {
            if (ApnsP12?.Length > 0 && string.IsNullOrWhiteSpace(ApnsP12Password))
            {
                yield return new ValidationResult("Password is required when adding p12 certificate", new[] { this.GetPropertyName(m => m.ApnsP12Password) });
            }

            if (SafariApnsP12?.Length > 0 && string.IsNullOrWhiteSpace(SafariApnsP12Password))
            {
                yield return new ValidationResult("Password is required when adding safari p12 certificate", new[] { this.GetPropertyName(m => m.SafariApnsP12Password) });
            }
        }
    }
}
