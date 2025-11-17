using Refit;

namespace MyTaskManager.UI.Api
{
    internal static class ApiServiceFactory
    {
        // Base URL of your running API (NO /api at the end)
        private static readonly string ApiBaseUrl = "https://localhost:7299";

        private static ITaskApi _taskApiClient;
        private static IAuthApi _authApiClient;

        // --- Task API Client ---
        public static ITaskApi TaskApiClient
        {
            get
            {
                if (_taskApiClient == null)
                {
                    _taskApiClient = RestService.For<ITaskApi>(ApiBaseUrl);
                }
                return _taskApiClient;
            }
        }

        // --- Auth API Client ---
        public static IAuthApi AuthApiClient
        {
            get
            {
                if (_authApiClient == null)
                {
                    _authApiClient = RestService.For<IAuthApi>(ApiBaseUrl);
                }
                return _authApiClient;
            }
        }
    }
}
