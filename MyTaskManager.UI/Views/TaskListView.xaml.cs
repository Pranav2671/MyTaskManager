using System;
using System.Windows;
using System.Windows.Controls;
using MyTaskManager.UI.Api;
using MyTaskManager.Shared.Models;

namespace MyTaskManager.UI.Views
{
    public partial class TaskListView : UserControl
    {
        private readonly ITaskApi _taskApi;

        public TaskListView(ITaskApi taskApi)
        {
            InitializeComponent();
            _taskApi = taskApi;

            LoadTasks();
        }

        private async void LoadTasks()
        {
            try
            {
                var tasks = await _taskApi.GetAllTasksAsync();
                TasksDataGrid.ItemsSource = tasks;
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
            if (task == null)
                return;

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
