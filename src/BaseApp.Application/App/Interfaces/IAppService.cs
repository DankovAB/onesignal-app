using System;
using System.Threading.Tasks;
using BaseApp.Application.App.Models;

namespace BaseApp.Application.App.Interfaces
{
    public interface IAppService
    {
        Task<ListAppResponse> GetAllAppsAsync();
        Task<AppResponse> GetApp(Guid appId);
        Task CreateApp(CreateAppRequest request);
        Task EditApp(UpdateAppRequest request);
    }
}