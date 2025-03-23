using System;
using Xamarin.Forms;
using FlashCardMobileApp.ViewModels.Admin;
using FlashCardMobileApp.Views.Flashcards;

namespace FlashCardMobileApp.Views.Admin
{
    public partial class AdminHomePage : Shell
    {
        public AdminHomePage()
        {
            InitializeComponent();
            this.BindingContext = new AdminHomeViewModel();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminUsersPage());
        }
    }
}
