using MyTaskManager.Shared.Models;
using MyTaskManager.UI.Api;
using MyTaskManager.UI.Views;
using Refit;
using System.Windows;

namespace MyTaskManager.UI
{
    public partial class MainWindow : Window
    {
        private readonly ITaskApi _taskApi;
        private readonly IAuthApi _authApi;
        private User _loggedInUser;

        public MainWindow()
        {
            InitializeComponent();

            // Correct Base URL
            _authApi = RestService.For<IAuthApi>("https://localhost:7299");
            _taskApi = RestService.For<ITaskApi>("https://localhost:7299");

            ShowLoginView();
        }

        // ---------- NEW CLEAN METHOD ----------
        public void ShowLoginView()
        {
            TopMenu.Visibility = Visibility.Collapsed;
            MainContent.Content = new LoginView(_authApi, OnLoginSuccess);
        }
        // --------------------------------------

        // Called after successful login
        public void OnLoginSuccess(User user)
        {
            _loggedInUser = user;

            TopMenu.Visibility = Visibility.Visible;

            MainContent.Content = new TaskListView(_taskApi, _loggedInUser);
        }

        // View Tasks
        private void ViewTasks_Click(object sender, RoutedEventArgs e)
        {
            if (_loggedInUser != null)
                MainContent.Content = new TaskListView(_taskApi, _loggedInUser);
            else
                ShowLoginView();
        }

        // Add Task
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (_loggedInUser != null)
                MainContent.Content = new AddTaskView(_taskApi, _loggedInUser);
            else
                ShowLoginView();
        }
    }
}
