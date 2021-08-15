using BaseApp.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseApp.Web.Code.Infrastructure.BaseControllers
{
    [Area("Admin")]
    [Authorize(Policy = Constants.Policy.AdminOrDataEntryOperator)]
    public abstract class ControllerBaseAdminOrOperatorRequired : ControllerBaseNoAuthorize
    {

    }
}
