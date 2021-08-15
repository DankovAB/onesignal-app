using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseApp.Application.App.Models;
using Refit;

namespace BaseApp.Application.App.Interfaces
{
    [Headers("Authorization: Basic",
        "Content-Type: application/json")]
    public interface IOneSignalApi
    {
        [Get("/apps")]
        Task<IReadOnlyCollection<AppResponse>> GetAppsAsync();

        [Get("/apps/{id}")]
        Task<AppResponse> GetAppAsync(Guid id);

        [Post("/apps")]
        Task<AppResponse> CreateAppsAsync([Body] CreateAppRequest request);

        [Put("/apps/{id}")]
        Task<AppResponse> UpdateAppsAsync(Guid id, [Body] CreateAppRequest request);
    }
}