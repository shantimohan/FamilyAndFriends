using FnF_Tabs_MVVM_XFA.Models;
using FnF_Tabs_MVVM_XFA.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FnF_Tabs_MVVM_XFA.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FamilyPage : ContentPage
	{
        ItemsViewModel familyViewModel;

		public FamilyPage ()
		{
			InitializeComponent ();
            BindingContext = familyViewModel = new ItemsViewModel("Family");
		}

        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs args)
        {
            Item item = (Item)args.Item;
            if (item != null)
                //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
                await Navigation.PushAsync(new ItemDetailPage(familyViewModel, item));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            // CHANGE: Added view model to the call params in FamilyPage
            //await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(familyViewModel.Title, familyViewModel)));
            await Navigation.PushAsync(new ItemDetailPage(familyViewModel));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine("In FamilyPage - OnAppearing");
            Debug.WriteLine($"1: familyViewModel.Items.Count = {familyViewModel.Items.Count}");

            if (familyViewModel.Items.Count == 0)
                familyViewModel.LoadItemsCommand.Execute(null);

            Debug.WriteLine($"2: familyViewModel.Items.Count = {familyViewModel.Items.Count}");
            Debug.WriteLine($"Family Pge - last_id = {App.last_id}");
        }

        async void Image_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GetImage());
        }
    }
}