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
	public partial class FriendsPage : ContentPage
	{
        ItemsViewModel friendsViewModel;

        public FriendsPage ()
		{
			InitializeComponent ();
            BindingContext = friendsViewModel = new ItemsViewModel("Friends");
        }

        private async void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs args)
        {
            Item item = (Item)args.Item;
            if (item != null)
                //await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
                await Navigation.PushAsync(new ItemDetailPage(friendsViewModel, item));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            // CHANGE: Added view model to the call params in FriendsPage
            //await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(friendsViewModel.Title, friendsViewModel)));
            await Navigation.PushAsync(new ItemDetailPage(friendsViewModel));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine("In FriendsPage - OnAppearing");

            if (friendsViewModel.Items.Count == 0)
                friendsViewModel.LoadItemsCommand.Execute(null);

            Debug.WriteLine($"friendsViewModel.Items.Count = {friendsViewModel.Items.Count}");
            Debug.WriteLine($"Friend Page - last_id = {App.last_id}");
        }
    }
}