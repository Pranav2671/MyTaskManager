using MyTaskManager.UI.Api;
using System;
using System.Windows;
using System.Windows.Controls;
using TaskStatus = MyTaskManager.Shared.Models.TaskStatus;
using MyTaskManager.Shared.Models;

namespace MyTaskManager.UI.Views
{
    public partial class EditTaskWindow : Window
    {
        private readonly ITaskApi _taskApi;
        private readonly TaskItem _task;

        public EditTaskWindow(TaskItem task, ITaskApi taskApi)
        {
            InitializeComponent();
            _task = task;
            _taskApi = taskApi;

            TitleTextBox.Text = _task.Title;

            if (_task.DueDate.HasValue)
                DueDatePicker.SelectedDate = _task.DueDate.Value;

            var item = GetStatusComboItem(_task.Status);
            if (item != null)
                StatusComboBox.SelectedItem = item;
        }

        private ComboBoxItem GetStatusComboItem(TaskStatus status)
        {
            foreach (var obj in StatusComboBox.Items)
            {
                var item = obj as ComboBoxItem;
                if (item == null) continue;

                if (string.Equals(item.Content.ToString(), status.ToString(), StringComparison.OrdinalIgnoreCase))
                    return item;
            }
            return null;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Please enter a valid title.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _task.Title = TitleTextBox.Text.Trim();
                _task.DueDate = DueDatePicker.SelectedDate;

                var selectedItem = StatusComboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    var text = selectedItem.Content.ToString();
                    if (Enum.TryParse<TaskStatus>(text, true, out var parsed))
                    {
                        _task.Status = parsed;
                    }
                }

                await _taskApi.UpdateTaskAsync(_task.Id, _task);

                MessageBox.Show("Task updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating task:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
