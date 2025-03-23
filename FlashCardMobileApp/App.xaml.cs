using FlashCardMobileApp.Services;
using Xamarin.Forms;
using System;
using System.Threading.Tasks;
using FlashCardMobileApp.Views;
using System.Diagnostics;

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
                await Shell.Current.GoToAsync("//LoginPage");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during app startup: {ex.Message}");
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
