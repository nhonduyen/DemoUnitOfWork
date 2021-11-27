namespace Recruiter.Core.Common.Constants
{
    public static class StatusCode
    {
        public const string ERROR = "error";
        public const string SUCCESS = "success";
        public const string AUTHENTICATION_ERROR = "authentication_error";
        public const string INVALID_REQUEST_ERROR = "invalid_request_error";
        public const string PERMISSION_ERROR = "permission_error";
        public const string API_ERROR = "api_error";
        public const string REALTIME_ERROR = "realtime_error";

        public const string PERMISSION_INDEPENDENCE_ERROR = "permission_independence_required";
        public const string FILE_ALREADY_EXIST = "file_already_exist";
        public const int METHOD_NOT_ALLOW = 304;
        public const string METHOD_NOT_ALLOW_ERROR = "method_not_allow";

        public const int ACCESS_DENIED = 403;
    }
}
