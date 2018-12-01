using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FnF_Tabs_MVVM_XFA.Models;
using System.Diagnostics;
using FnF_Tabs_MVVM_XFA.ViewModels;
using System.Linq;

namespace FnF_Tabs_MVVM_XFA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        //TODO: #6 - Remove ItemDetailsPage and Change name of this page to ItemDetailsPage
        public Item Item { get; set; }
        public ItemsViewModel itemsViewModel;    //CHANGE: Added view model as field
        private bool newItem;

        // CHANGE: Added view model to the constructor params
        public NewItemPage(ItemsViewModel ivmIn, Item itemIn = null)
        {
            InitializeComponent();

            this.newItem = itemIn == null ? true : false;

            Item = new Item
            {
                Name = $"New {(ivmIn.Title == "Family" ? "Family Member" : "Friend")}",
                DateOfBirth = App.DefaultDate,
                Anniversary = App.DefaultDate,
                Category = ivmIn.Title,
            };

            if (newItem)
            {
                this.Title = Item.Name;
            }
            else
            {
                Item.Id = itemIn.Id;
                Item.Name = itemIn.Name;
                Item.DateOfBirth = itemIn.DateOfBirth;
                Item.Anniversary = itemIn.Anniversary;
                Item.Category = itemIn.Category;

                this.Title = $"ID: {(itemIn?.Id)}";
            }

            // CHANGE: Setting the view model field in class constructor
            itemsViewModel = ivmIn;

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine($"New Item - Save: last_id = {App.last_id}");

                if (newItem)
                {
                    Item.Id = ++App.last_id;
                    //MessagingCenter.Send(this, "AddItem", Item);
                    // CHANGE: Moved functionality of MessageCenter code here
                    await App.FnfDatabase.AddItemAsync(Item);
                    itemsViewModel.Items.Add(Item);
                }
                else
                {
                    await App.FnfDatabase.UpdateItemAsync(Item);

                    var oldItem = itemsViewModel.Items.Where((Item arg) => arg.Id == Item.Id).FirstOrDefault();
                    int index = itemsViewModel.Items.IndexOf(oldItem);
                    itemsViewModel.Items.Remove(oldItem);
                    itemsViewModel.Items.Insert(index, Item);

                    await DisplayAlert($"Item {(newItem ? "Added" : "Updated")}", "Successful", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert($"Error {(newItem ? "Adding" : "Editing")} Item", ex.Message, "OK");
            }
            finally
            {
                await Navigation.PopAsync();
            }
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            try
            {
                await App.FnfDatabase.DeleteItemAsync(Item);

                var oldItem = itemsViewModel.Items.Where((Item arg) => arg.Id == Item.Id).FirstOrDefault();
                itemsViewModel.Items.Remove(oldItem);

                await DisplayAlert("Item Deleted", "Successful", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error Deleting Item", ex.Message, "OK");
            }
            finally
            {
                await Navigation.PopAsync();
            }
        }
    }
}