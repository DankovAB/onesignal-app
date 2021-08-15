namespace BaseApp.Common
{
    public class Constants
    {
        public const int PageSizeDefault = 14;
        public const int CountryUSA_Id = 223;
        public const int MinPasswordLength = 7;

        public class Roles
        {
            public const string Admin = "admin";
            public const string DataOperator = "dataEntryOperator";
        }

        public class Policy
        {
            public const string Admin = "admin";
            public const string DataEntryOperator = "dataEntryOperator";
            public const string AdminOrDataEntryOperator = "AdminOrDataEntryOperator";
        }
    }
}
