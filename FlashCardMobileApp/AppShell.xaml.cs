using System;
using System.Collections.Generic;
using System.Windows.Input;
using FlashCardMobileApp.ViewModels;
using FlashCardMobileApp.Views;
using FlashCardMobileApp.Views.Flashcards;
using Xamarin.Forms;

namespace FlashCardMobileApp
{
    public partial class AppShell : Shell
    {
        public ICommand LogoutCommand => new Command(async () =>
        {
            App.AuthService.Logout();
            await Shell.Current.GoToAsync("///LoginPage");

        });

        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute("Flashcards/CreateFlashcardPage", typeof(CreateFlashcardPage));
            Routing.RegisterRoute(nameof(YourFlashCardsPage), typeof(YourFlashCardsPage));
            Routing.RegisterRoute(nameof(RandomQuizPage), typeof(RandomQuizPage));
            Routing.RegisterRoute(nameof(EditFlashcardPage), typeof(EditFlashcardPage));

            BindingContext = this;
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
            if (!confirm) return;

            // Clear authentication and navigate to LoginPage
            App.AuthService.Logout();
            await Shell.Current.GoToAsync("//LoginPage");
        }

    }
}
