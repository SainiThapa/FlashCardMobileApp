using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FlashCardMobileApp.ViewModels.Admin;

namespace FlashCardMobileApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateUserCardPage : ContentPage
    {
        public CreateUserCardPage(string userId)
        {
            InitializeComponent();
            BindingContext = new CreateUserCardViewModel(userId);
        }
    }
}
