using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FlashCardMobileApp.Models;
using FlashCardMobileApp.Views;
using FlashCardMobileApp.ViewModels;

namespace FlashCardMobileApp.Views
{
    public partial class BrowsePage : ContentPage
    {
        private readonly BrowseViewModel _viewModel;

        public BrowsePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new BrowseViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (await App.AuthService.IsLoggedIn())
            {
                await _viewModel.OnAppearing();
            }
            else
            {
                Console.WriteLine("Not logged in, skipping BrowseViewModel.OnAppearing");
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}