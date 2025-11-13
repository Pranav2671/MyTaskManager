using MyTaskManager.UI.Api;
using MyTaskManager.UI.Views;
using Refit;
using System.Windows;

namespace MyTaskManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// This window hosts all views inside a single ContentControl.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Refit API client, shared by all views
        private readonly ITaskApi _taskApi;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize Refit client with the API base URL
            _taskApi = RestService.For<ITaskApi>("https://localhost:7299");

            // Load default view (Task List) on startup
            MainContent.Content = new TaskListView(_taskApi);
        }

        /// <summary>
        /// Handles "View Tasks" button click.
        /// Loads TaskListView inside MainContent.
        /// </summary>
        public void ViewTasks_Click(object sender, RoutedEventArgs e)
        {
            // Pass the same _taskApi instance to allow API calls
            MainContent.Content = new TaskListView(_taskApi);
        }

        /// <summary>
        /// Handles "Add Task" button click.
        /// Loads AddTaskView inside MainContent.
        /// </summary>
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Pass the same _taskApi instance to allow API calls
            MainContent.Content = new AddTaskView(_taskApi);
        }
    }
}
