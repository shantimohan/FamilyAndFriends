using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using FnF_Tabs_MVVM_XFA.Models;
using FnF_Tabs_MVVM_XFA.Views;

namespace FnF_Tabs_MVVM_XFA.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel(string title)
        {
            Title = title;
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<ItemDetailPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    //TODO: #2 This event is run twice for one call from ItemDetailPage.Save_Clicked event.
            //    Debug.WriteLine("In MessageCenter.Subscribe for AddItem");
            //    var newItem = item as Item;
            //    Items.Add(newItem);
            //    await App.FnfDatabase.AddItemAsync(newItem);
            //    //await DataStore.AddItemAsync(newItem);
            //});
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await App.FnfDatabase.GetItemsAsync(Title, true);  // await DataStore.GetItemsAsync(Title, true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}