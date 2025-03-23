using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FlashCardMobileApp.ViewModels.Admin;
using System;
using System.Threading.Tasks;
using FlashCardMobileApp.Models;
using System.Diagnostics;

namespace FlashCardMobileApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(UserId), "userId")]
    public partial class AdminViewUserPage : ContentPage
    {
        private readonly UserDetailViewModel _viewModel;
        public string UserId { get; set; }

        public AdminViewUserPage(string userId)
        {
            InitializeComponent();
            _viewModel = new UserDetailViewModel();
            this.BindingContext = _viewModel;
            _viewModel.LoadUserDetails(userId);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!string.IsNullOrEmpty(UserId))
            {
                Debug.WriteLine($"Received UserId: {UserId}");
                _viewModel.LoadUserDetails(UserId);
            }
            else
            {
                Debug.WriteLine("UserId is null or empty.");
            }
        }
    }
}
