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
    /// <summary>
    /// Interaction logic for TaskListView.xaml
    /// </summary>
    public partial class TaskListView : UserControl
    {

        //Refit API client
        private readonly ITaskApi _taskApi;

        //ObservableCollection automatically updates the UI when items change
        public ObservableCollection<TaskItem> Tasks { get; set; }

        public TaskListView(ITaskApi taskApi)
        {
            InitializeComponent();
            _taskApi = taskApi;

            Tasks = new ObservableCollection<TaskItem>();

            // Bind the task collection to the ListBox in XAML
            TasksDataGrid.ItemsSource = Tasks;

            // 🔹 Hook into Loaded event instead of calling async in constructor
            this.Loaded += async (s, e) => await LoadTasksAsync();

            TasksDataGrid.CellEditEnding += TasksDataGrid_CellEditEnding;

        }


        /// <summary>
        /// Calls the API to fetch all tasks asynchronously.
        /// </summary>
        private async Task LoadTasksAsync()
        {
            try
            {
                var tasksFromApi = await _taskApi.GetAllTasksAsync();

                Tasks.Clear(); // Clear existing items
                foreach (var task in tasksFromApi)
                {
                    Tasks.Add(task);
                }
            }
            catch (System.Exception ex)
            {
                // Show a simple error message if API call fails
                System.Windows.MessageBox.Show($"Failed to load tasks: {ex.Message}");
            }
        }

        /// <summary>
        /// Called when user changes task status in the dropdown.
        /// </summary>

        private async void TasksDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Status")
            {
                var task = e.Row.Item as TaskItem;
                if (task != null)
                {
                    try
                    {
                        //Update task on API
                        
                        System.Windows.MessageBox.Show($"Status for '{task.Title}' updated successfully.");
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Failed to update status: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Called when user clicks the Edit button.
        /// Opens an EditTaskWindow (to edit title, due date, etc.)
        /// </summary>

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as TaskItem;

            if (task != null) return;

            var editWindow = new EditTaskWindow(task, _taskApi); //Well create this window next
            if (editWindow.ShowDialog() == true)
            {
                // If user saved changes, refresh the task list
                await LoadTasksAsync();
            }



        }
    }
}
