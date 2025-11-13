using System;
using System.Windows;
using System.Windows.Controls;
using MyTaskManager.Shared.Models;
using Refit;
using System.Threading.Tasks;
using MyTaskManager.UI.Api;
using TaskStatus = MyTaskManager.Shared.Models.TaskStatus;


namespace MyTaskManager.UI.Views
{
    /// <summary>
    /// Interaction logic for AddTaskView.xaml
    /// </summary>
    public partial class AddTaskView : UserControl
    {

        private readonly ITaskApi _taskApi;


        public AddTaskView(ITaskApi taskApi)
        {
            InitializeComponent();
            _taskApi = taskApi;
        }
        public AddTaskView() : this(RestService.For<ITaskApi>("https://localhost:7299"))
        {
        }

        //Event handler for Save button 
        private async void AddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Get Values from UI
                string title = TitleTextBox.Text;
                DateTime? dueDate = DueDatePicker.SelectedDate;
                TaskStatus status = (TaskStatus)Enum.Parse(typeof(TaskStatus), ((ComboBoxItem)StatusComboBox.SelectedItem).Tag.ToString());



                //Create new TaskItem object
                TaskItem newTask = new TaskItem
                {
                    Title = title,
                    Status = status,
                    DueDate = dueDate,
                    OwnerUserId = "user1",
                    CreatedAt = DateTime.UtcNow
                };

                //Call API to save task
                TaskItem createdTask = await _taskApi.CreateTaskAsync(newTask);

                //Notify User
                MessageBox.Show($"Task '{createdTask.Title}' created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                //Optionaly clear feilds
                TitleTextBox.Clear();
                StatusComboBox.SelectedIndex = 0;
                DueDatePicker.SelectedDate = null;
            }
            catch(Exception ex)
            {
                //Show error
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFields_Click(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Text = string.Empty;
            StatusComboBox.SelectedIndex = 0; // Reset to first option (like Pending)
            DueDatePicker.SelectedDate = null;
        }

    }
}
