using Xamarin.Forms;
using FlashCardMobileApp.ViewModels;

namespace FlashCardMobileApp.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();

            BindingContext = new ForgotPasswordViewModel();
        }
    }
}
