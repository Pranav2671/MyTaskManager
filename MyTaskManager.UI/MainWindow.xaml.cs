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

            // Correct Base URL (must include /api)
            _authApi = RestService.For<IAuthApi>("https://localhost:7299");
            _taskApi = RestService.For<ITaskApi>("https://localhost:7299");

            // Load the login screen first
            LoadLoginView();
        }

        private void LoadLoginView()
        {
            TopMenu.Visibility = Visibility.Collapsed; // hide menu
            MainContent.Content = new LoginView(_authApi, OnLoginSuccess);
        }

        // Called when login is successful
        private void OnLoginSuccess(User user)
        {
            _loggedInUser = user;

            // Show menu after login
            TopMenu.Visibility = Visibility.Visible;

            // Load task list
            MainContent.Content = new TaskListView(_taskApi, _loggedInUser);
        }

        // View Tasks menu button
        private void ViewTasks_Click(object sender, RoutedEventArgs e)
        {
            if (_loggedInUser != null)
                MainContent.Content = new TaskListView(_taskApi, _loggedInUser);
            else
                LoadLoginView();
        }

        // Add Task menu button
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (_loggedInUser != null)
                MainContent.Content = new AddTaskView(_taskApi, _loggedInUser);
            else
                LoadLoginView();
        }
    }
}
