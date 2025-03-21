using System;
using Xamarin.Forms;
using FlashCardMobileApp.ViewModels.Admin;

namespace FlashCardMobileApp.Views.Admin
{
    public partial class AdminHomePage : Shell
    {
        public AdminHomePage()
        {
            InitializeComponent();
            this.BindingContext = new AdminHomeViewModel();
        }
    }
}
