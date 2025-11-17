using MyTaskManager.Shared.Models;
using MyTaskManager.UI.Api;
using Refit;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyTaskManager.UI.Views
{
    public partial class RegisterView : UserControl
    {
        private readonly IAuthApi _authApi;
        private readonly Action<User> _onLoginSuccess;

        public RegisterView(IAuthApi authApi, Action<User> onLoginSuccess)
        {
            InitializeComponent();
            _authApi = authApi;
            _onLoginSuccess = onLoginSuccess;
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            txtMessage.Text = "";

            var request = new RegisterRequest
            {
                Username = txtUsername.Text,
                Password = txtPassword.Password
            };

            try
            {
                await _authApi.RegisterAsync(request);

                txtMessage.Foreground = System.Windows.Media.Brushes.Green;
                txtMessage.Text = "Registration Successful! Please login.";

                // Go back to LoginView inside same ContentControl
                if (this.Parent is ContentControl parentContent)
                {
                    parentContent.Content = new LoginView(_authApi, _onLoginSuccess);
                }
            }
            catch (ApiException ex)
            {
                txtMessage.Foreground = System.Windows.Media.Brushes.Red;
                txtMessage.Text = ex.Content;
            }
            catch (Exception ex)
            {
                txtMessage.Foreground = System.Windows.Media.Brushes.Red;
                txtMessage.Text = ex.Message;
            }
        }
    }
}
