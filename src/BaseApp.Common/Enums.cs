using System.ComponentModel;

namespace BaseApp.Common
{
    public class Enums
    {
        public enum SchedulerActionTypes
        {
            ResetPasswordEmail = 1,

            ExampleAction = 1000
        }

        public enum MenuItemTypes
        {

        }

        public enum ApnsEnvironmentTypes
        {
            [Description("sandbox")]
            Sandbox = 1,

            [Description("production")]
            Production = 2
        }
    }
}
