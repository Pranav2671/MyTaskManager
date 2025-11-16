using MyTaskManager.Shared.Models;
using MyTaskManager.UI.Api;
using Refit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyTaskManager.UI.Views
{
    public partial class TaskListView : UserControl
    {
        private readonly ITaskApi _taskApi;

        public ObservableCollection<TaskItem> Tasks { get; set; }

        public TaskListView(ITaskApi taskApi)
        {
            InitializeComponent();
            _taskApi = taskApi;

            Tasks = new ObservableCollection<TaskItem>();
            TasksDataGrid.ItemsSource = Tasks;

            this.Loaded += async (s, e) => await LoadTasksAsync();

            TasksDataGrid.CellEditEnding += TasksDataGrid_CellEditEnding;
        }

        private async Task LoadTasksAsync()
        {
            try
            {
                var tasksFromApi = await _taskApi.GetAllTasksAsync();

                Tasks.Clear();
                foreach (var task in tasksFromApi)
                {
                    Tasks.Add(task);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Failed to load tasks: {ex.Message}");
            }
        }

        private async void TasksDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Status")
            {
                var task = e.Row.Item as TaskItem;
                if (task != null)
                {
                    try
                    {
                        // TODO: You will add update logic later
                        MessageBox.Show($"Status for '{task.Title}' updated successfully.");
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"Failed to update status: {ex.Message}");
                    }
                }
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as TaskItem;

            if (task == null) return;   // FIXED: Your previous code returned when task was NOT null (wrong)

            var editWindow = new EditTaskWindow(task, _taskApi);
            if (editWindow.ShowDialog() == true)
            {
                await LoadTasksAsync();
            }
        }

        // -----------------------------
        // DELETE BUTTON CLICK HANDLER
        // -----------------------------
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            // Get the TaskItem bound to this row
            var task = button.DataContext as TaskItem;
            if (task == null) return;

            // Confirmation dialog
            var result = MessageBox.Show(
                $"Are you sure you want to delete the task \"{task.Title}\"?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                // Call your Refit API to delete
                await _taskApi.DeleteTaskAsync(task.Id);

                MessageBox.Show("Task deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh the task list
                await LoadTasksAsync();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Failed to delete task:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
