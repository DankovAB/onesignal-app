using System;
using System.IO;
using BaseApp.Application.App.Models;
using BaseApp.Common.Extensions;
using BaseApp.Web.Areas.Admin.Models.App;
using Microsoft.AspNetCore.Http;

namespace BaseApp.Web.Code.Mappers.Admin
{
    public class AppMapperAdmin : MapperBase
    {
        public AppMapperAdmin()
        {
            CreateMap<ListAppViewModel, ListAppResponse>()
                .Map(m => m.Items, r => r.Items);

            CreateMap<AppResponse, AppViewModel>();

            CreateMap<AppResponse, EditAppModel>()
                .Ignore(m => m.AndroidGcmSenderId)
                .Ignore(m => m.AdditionalDataIsRootPayload)
                .Ignore(m => m.OrganizationId)
                .Ignore(m => m.ApnsP12Password)
                .Ignore(m => m.SafariApnsP12Password)

                .Map(m => m.ApnsP12, r => r.ApnsCertificates)
                .Map(m => m.SafariApnsP12, r => r.SafariApnsCertificate);

            CreateMap<EditAppModel, CreateAppRequest>()
                .Map(m => m.ApnsP12, r => FileToBase64String(r.ApnsP12))
                .Map(m => m.ApnsEnv, r => r.ApnsEnv.HasValue ? r.ApnsEnv.ToDescription() : null);

            CreateMap<EditAppModel, UpdateAppRequest>()
                .Map(m => m.ApnsP12, r => FileToBase64String(r.ApnsP12))
                .Map(m => m.ApnsEnv, r => r.ApnsEnv.HasValue ? r.ApnsEnv.ToDescription() : null);
        }

        private static string FileToBase64String(IFormFile file)
        {
            if (file.Length == 0)
            {
                return null;
            }

            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
    }
}
