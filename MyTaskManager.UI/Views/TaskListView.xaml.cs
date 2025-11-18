using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MyTaskManager.UI.Api;
using MyTaskManager.Shared.Models;

namespace MyTaskManager.UI.Views
{
    public partial class TaskListView : UserControl
    {
        private readonly ITaskApi _taskApi;
        private readonly User _loggedInUser; // currently logged-in user

        public TaskListView(ITaskApi taskApi, User loggedInUser)
        {
            InitializeComponent();
            _taskApi = taskApi;
            _loggedInUser = loggedInUser;

            // ✅ SERIAL NUMBER FOR EACH ROW
            TasksDataGrid.LoadingRow += (s, e) =>
            {
                e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            };

            LoadTasks();
        }

        private async void LoadTasks()
        {
            try
            {
                var tasks = await _taskApi.GetAllTasksAsync();

                // Show only tasks for the logged-in user
                var userTasks = tasks.FindAll(t => t.OwnerUserId == _loggedInUser.Username);

                TasksDataGrid.ItemsSource = userTasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tasks from API:\n" + ex.Message);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var task = (sender as FrameworkElement).DataContext as TaskItem;
            if (task == null) return;

            // Open edit window
            var editWindow = new EditTaskWindow(task, _taskApi);
            editWindow.ShowDialog();

            // Refresh tasks after save
            LoadTasks();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var task = (sender as FrameworkElement).DataContext as TaskItem;
            if (task == null) return;

            if (MessageBox.Show("Delete this task?", "Confirm", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            try
            {
                await _taskApi.DeleteTaskAsync(task.Id);
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed:\n" + ex.Message);
            }
        }
    }
}
