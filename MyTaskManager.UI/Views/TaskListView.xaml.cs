using MyTaskManager.Shared.Models;
using MyTaskManager.UI.Api;
using Refit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    }
}
