using Newtonsoft.Json;

namespace BaseApp.Application.App.Models
{
    public class CreateAppRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("apns_env")]
        public string ApnsEnv { get; set; }

        [JsonProperty("apns_p12")]
        public string ApnsP12 { get; set; }

        [JsonProperty("apns_p12_password")]
        public string ApnsP12Password { get; set; }

        [JsonProperty("gcm_key")]
        public string GcmKey { get; set; }

        [JsonProperty("android_gcm_sender_id")]
        public string AndroidGcmSenderId { get; set; }

        [JsonProperty("chrome_web_origin")]
        public string ChromeWebOrigin { get; set; }

        [JsonProperty("chrome_web_default_notification_icon")]
        public string ChromeWebDefaultNotificationIcon { get; set; }

        [JsonProperty("chrome_web_sub_domain")]
        public string ChromeWebSubDomain { get; set; }

        [JsonProperty("site_name")]
        public string SiteName { get; set; }

        [JsonProperty("safari_site_origin")]
        public string SafariSiteOrigin { get; set; }

        [JsonProperty("safari_apns_p12")]
        public string SafariApnsP12 { get; set; }

        [JsonProperty("safari_apns_p12_password")]
        public string SafariApnsP12Password { get; set; }

        [JsonProperty("safari_icon_256_256")]
        public string SafariIcon256256 { get; set; }

        [JsonProperty("chrome_key")]
        public string ChromeKey { get; set; }

        [JsonProperty("additional_data_is_root_payload")]
        public bool? AdditionalDataIsRootPayload { get; set; }

        [JsonProperty("organization_id")]
        public string OrganizationId { get; set; }
    }
}