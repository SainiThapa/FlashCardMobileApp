using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FlashCardMobileApp.ViewModels.Admin;

namespace FlashCardMobileApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminResetPasswordsPage : ContentPage
    {
        public AdminResetPasswordsPage()
        {
            InitializeComponent();
            BindingContext = new AdminResetPasswordsViewModel();
        }
    }
}
