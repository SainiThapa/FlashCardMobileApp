using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        public ICommand BackToLoginCommand => new Command(async () => await Shell.Current.GoToAsync("//LoginPage"));

        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
            {
                await DisplayAlert("Validation Error", "First Name is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(LastNameEntry.Text))
            {
                await DisplayAlert("Validation Error", "Last Name is required.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                await DisplayAlert("Validation Error", "Email is required.", "OK");
                return;
            }

            if (!Regex.IsMatch(EmailEntry.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                await DisplayAlert("Validation Error", "Email should end with @<mailservice>.com.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Validation Error", "Password is required.", "OK");
                return;
            }

            if (!Regex.IsMatch(PasswordEntry.Text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
            {
                await DisplayAlert("Validation Error", "Password must contain at least 8 characters with uppercase, lowercase letters, numbers, and special symbols.", "OK");
                return;
            }

            // Register the user
            var success = await RegisterAsync(FirstNameEntry.Text, LastNameEntry.Text, EmailEntry.Text, PasswordEntry.Text);
            if (success)
            {
                // Automatically log in after successful registration
                var loginSuccess = await App.AuthService.LoginAsync(EmailEntry.Text, PasswordEntry.Text);
                if (loginSuccess)
                {
                    await Shell.Current.GoToAsync("///WelcomePage");
                }
                else
                {
                    await DisplayAlert("Error", "Registration succeeded, but login failed. Please try logging in manually.", "OK");
                    await Shell.Current.GoToAsync("//LoginPage");
                }
            }
            else
            {
                await DisplayAlert("Error", "Registration failed. Email may already be in use.", "OK");
            }
        }
        private async Task<bool> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            try
            {
                var client = new HttpClient();
                var registerUrl = "http://192.168.1.84:5000/api/AccountApi/register"; // Adjust URL as needed
                var registerData = new
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password
                };
                var content = new StringContent(JsonConvert.SerializeObject(registerData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(registerUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Registration successful for {email}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Registration failed: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return false;
            }
        }
    }
}