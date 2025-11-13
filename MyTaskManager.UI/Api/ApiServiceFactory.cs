using Refit;
using System;

namespace MyTaskManager.UI.Api
{
    internal class ApiServiceFactory
    {
        //Base URL of your running API
        private static readonly string ApiBaseUrl = "https://localhost:7299/api";

        private static ITaskApi? _taskApiClient;

        public static ITaskApi TaskApiClient
        {
            get
            {
                if(_taskApiClient == null)
                {
                    //Create a Refit client for ITaskApi using base URL
                     _taskApiClient = RestService.For<ITaskApi>(ApiBaseUrl);
                }
                return _taskApiClient;
            }
        }
    }
}
