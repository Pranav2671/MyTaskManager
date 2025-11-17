using MyTaskManager.Shared.Models;
using MyTaskManager.UI.Api;
using Refit;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyTaskManager.UI.Views
{
    public partial class LoginView : UserControl
    {
        private readonly IAuthApi _authApi;
        private readonly Action<User> _onLoginSuccess;

        public LoginView(IAuthApi authApi, Action<User> onLoginSuccess)
        {
            InitializeComponent();
            _authApi = authApi;
            _onLoginSuccess = onLoginSuccess;
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            txtMessage.Text = "";

            var request = new LoginRequest
            {
                Username = txtUsername.Text,
                Password = txtPassword.Password
            };

            try
            {
                var user = await _authApi.LoginAsync(request);

                if (user != null)
                {
                    // This should trigger MainWindow to load TaskListView
                    _onLoginSuccess?.Invoke(user);
                }
                else
                {
                    txtMessage.Text = "Invalid username or password";
                }
            }
            catch (ApiException ex)
            {
                txtMessage.Text = "API Error: " + ex.StatusCode + Environment.NewLine +
        ex.Content + Environment.NewLine +
        ex.Message;  // Show API error
            }
            catch (Exception ex)
            {
                txtMessage.Text = "Error: "+ex.Message;  // Show other exceptions
            }
        }


        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Load RegisterView inside the same ContentControl
            if (this.Parent is ContentControl parentContent)
            {
                parentContent.Content = new RegisterView(_authApi, _onLoginSuccess);
            }
        }
    }
}
