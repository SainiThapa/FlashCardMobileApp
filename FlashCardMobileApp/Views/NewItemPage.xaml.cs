using FlashCardMobileApp.ViewModels;
using Xamarin.Forms;

namespace FlashCardMobileApp.Views
{
    public partial class NewItemPage : ContentPage
    {
        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}