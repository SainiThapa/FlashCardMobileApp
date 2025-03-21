using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FlashCardMobileApp.ViewModels.Admin;

namespace FlashCardMobileApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminCategoriesPage : ContentPage
    {
        public AdminCategoriesPage()
        {
            InitializeComponent();
            BindingContext = new AdminCategoriesViewModel();
        }
    }
}
