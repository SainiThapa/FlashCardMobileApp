using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FlashCardMobileApp.ViewModels.Admin;
using FlashCardMobileApp.Models;
using FlashCardMobileApp.Services;
using System.Diagnostics;

namespace FlashCardMobileApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminCategoriesPage : ContentPage
    {
        private readonly AdminCategoriesViewModel _viewModel;
        private readonly ApiService _apiservice;
        public AdminCategoriesPage()
        {
            InitializeComponent();
            _viewModel = new AdminCategoriesViewModel();
            BindingContext = _viewModel;
            _apiservice = new ApiService();
        }

        private async void OnEditCategoryClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Category;

            if (category == null) return;
            Debug.WriteLine("Not null category");
            string newCategoryName = await DisplayPromptAsync("Edit Category", "Enter new category name:", initialValue: category.Name);
            if (!string.IsNullOrEmpty(newCategoryName) && newCategoryName != category.Name)
            {
                category.Name = newCategoryName;
                bool success = await _apiservice.UpdateCategoryAsync(category);

                if (success)
                {
                    await DisplayAlert("Success", "Category updated successfully", "OK");
                    _viewModel.LoadCategories(); // Refresh the categories list
                }
                else
                {
                    await DisplayAlert("Error", "Failed to update category", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Category name cannot be empty or unchanged", "OK");
            }
        }

        // Delete Category Button Clicked
        private async void OnDeleteCategoryClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var category = button?.CommandParameter as Category;

            if (category == null) return;
            Debug.WriteLine("Category ID is");
            Debug.WriteLine(category.Id);
            bool confirm = await DisplayAlert("Delete", $"Are you sure you want to delete the category {category.Name}?", "Yes", "No");
            if (confirm)
            {
                Debug.WriteLine("Confirmed request");
                bool success = await _apiservice.DeleteCategoryAsync(category);
                Debug.WriteLine($"Sucess or not : {success}");
                if (success)
                {
                    await DisplayAlert("Success", "Category deleted successfully", "OK");
                    _viewModel.LoadCategories(); // Refresh the categories list
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete category", "OK");
                }
            }
        }
    }
}
