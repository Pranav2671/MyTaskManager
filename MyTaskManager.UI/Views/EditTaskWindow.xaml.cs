using MyTaskManager.UI.Api;
using System;
using System.Windows;
using System.Windows.Controls;
using TaskStatus = MyTaskManager.Shared.Models.TaskStatus;
using MyTaskManager.Shared.Models;

namespace MyTaskManager.UI.Views
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditTaskWindow : Window
    {
        private readonly ITaskApi _taskApi; // Refit API client
        private readonly TaskItem _task;    // Current task being edited

        public EditTaskWindow(TaskItem task, ITaskApi taskApi)
        {
            InitializeComponent();
            _task = task;
            _taskApi = taskApi;

            // Pre-fill existing task data into controls
            TitleTextBox.Text = _task.Title;

            // Due date is optional (can be null)
            if (_task.DueDate.HasValue)
                DueDatePicker.SelectedDate = _task.DueDate.Value;

            // Select correct status (handles null or invalid values gracefully)
            StatusComboBox.SelectedItem = GetStatusComboItem(_task.Status);
        }

        /// <summary>
        /// Helper: returns the ComboBoxItem that matches the current task status.
        /// </summary>
        private ComboBoxItem? GetStatusComboItem(TaskStatus? status)
        {
            if (status == null) return null;

            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (string.Equals(item.Content.ToString(),
                    status.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Triggered when Save button is clicked.
        /// Updates the task through the API.
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // ✅ Validate title (required)
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Please enter a valid title.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Update the title
                _task.Title = TitleTextBox.Text.Trim();

                // Update due date only if selected, else set to null
                _task.DueDate = DueDatePicker.SelectedDate;

                // ✅ Safely update task status
                var selectedStatusItem = (ComboBoxItem)StatusComboBox.SelectedItem;
                if (selectedStatusItem != null)
                {
                    object contentValue = selectedStatusItem.Content;

                    // Handle both string and integer values safely
                    if (contentValue is string strValue)
                    {
                        if (Enum.TryParse<TaskStatus>(strValue, true, out var parsed))
                            _task.Status = parsed;
                    }
                    else if (contentValue is int intValue)
                    {
                        _task.Status = (TaskStatus)intValue;
                    }
                }


                // Call API to save updated task
                await _taskApi.UpdateTaskAsync(_task.Id.ToString(), _task);


                MessageBox.Show($"✅ Task '{_task.Title}' updated successfully!",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true; // Close dialog on success
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error updating task:\n{ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Triggered when Cancel button is clicked.
        /// Closes window without saving.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
