using FlashCardMobileApp.Services;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;

namespace FlashCardMobileApp
{
    public partial class App : Application
    {
        public static AuthenticationService AuthService { get; } = new AuthenticationService();
        public static ApiService ApiService { get; } = new ApiService();

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            try
            {
                bool isLoggedIn = await App.ApiService.IsLoggedIn();

                if (!isLoggedIn)
                {
                    Console.WriteLine("No valid token, navigating to LoginPage");
                    await Shell.Current.GoToAsync("//LoginPage");
                    return;
                }

                Console.WriteLine("Valid token found. Checking user role...");

                // Check if the user is Admin
                bool isAdmin = await App.ApiService.IsAdmin();

                if (isAdmin)
                {
                    Console.WriteLine("Admin detected, navigating to AdminHomePage");
                    (Shell.Current as AppShell)?.SetFlyoutForAdmin(true);
                    await Shell.Current.GoToAsync("//AdminHomePage");
                }
                else
                {
                    Console.WriteLine("User detected, navigating to WelcomePage");
                    (Shell.Current as AppShell)?.SetFlyoutForAdmin(false);
                    await Shell.Current.GoToAsync("//WelcomePage");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during app startup: {ex.Message}");
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
