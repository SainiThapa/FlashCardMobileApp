using FlashCardMobileApp.Services;
using FlashCardMobileApp;
using Xamarin.Forms;
using System;

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
            bool isLoggedIn = await App.AuthService.IsLoggedIn();
            if (!isLoggedIn)
            {
                Console.WriteLine("No valid token, navigating to LoginPage");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                Console.WriteLine("Valid token found, navigating to Welcomepage");
                await Shell.Current.GoToAsync("//WelcomePage");
            }
        }
    }
}