using FnF_Tabs_MVVM_XFA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FnF_Tabs_MVVM_XFA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Debug.WriteLine("In MainPage - OnAppearing");

            //DBOP:  Get item count from database
            int itemCount = await App.FnfDatabase.GetTotalItemCount();

            App.last_id = itemCount;

            Debug.WriteLine($"Main Page - last_id = {App.last_id}");
        }
    }
}