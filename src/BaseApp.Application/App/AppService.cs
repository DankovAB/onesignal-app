using BaseApp.Application.App.Interfaces;
using BaseApp.Application.App.Models;
using System;
using System.Threading.Tasks;

namespace BaseApp.Application.App
{
    public class AppService : IAppService
    {
        private readonly IOneSignalApi _oneSignalApi;

        public AppService(IOneSignalApi oneSignalApi)
        {
            _oneSignalApi = oneSignalApi;
        }

        public async Task<ListAppResponse> GetAllAppsAsync()
        {
            var response = await _oneSignalApi.GetAppsAsync();

            return new ListAppResponse
            {
                Items = response,
                TotalCount = response.Count
            };
        }

        public async Task<AppResponse> GetApp(Guid appId)
        {
            return await _oneSignalApi.GetAppAsync(appId);
        }

        public async Task CreateApp(CreateAppRequest request)
        {
            await _oneSignalApi.CreateAppsAsync(request);
        }

        public async Task EditApp(UpdateAppRequest request)
        {
            await _oneSignalApi.UpdateAppsAsync(request.Id, request);
        }
    }
}