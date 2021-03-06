﻿using FnF_Tabs_MVVM_XFA.Models;
using FnF_Tabs_MVVM_XFA.Views;
using Syncfusion.ListView.XForms.Control.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace FnF_Tabs_MVVM_XFA
{
    class SfListViewAccordionBehavior : Behavior<FamilyPage>
    {
        #region Fields

        //private Item tappedItem;      // Moved to App.xaml.cs
        private Syncfusion.ListView.XForms.SfListView listview;
        #endregion


        #region Override Methods

        protected override void OnAttachedTo(FamilyPage bindable)
        {
            listview = bindable.FindByName<Syncfusion.ListView.XForms.SfListView>("ItemsListView");
            //listview.ItemTapped += ListView_ItemTapped;
            listview.ItemDoubleTapped += Listview_ItemDoubleTapped;
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            //listview.ItemTapped -= ListView_ItemTapped;
            listview.ItemDoubleTapped -= Listview_ItemDoubleTapped;
        }
        #endregion

        #region Private Methods

        private void Listview_ItemDoubleTapped(object sender, Syncfusion.ListView.XForms.ItemDoubleTappedEventArgs e)
        //private void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            Debug.WriteLine("Entering - Behavior: Item Tapped / Double Tapped");

            if (App.tappedItem != null && App.tappedItem.ShowActionMenu)
            {
                var previousIndex = listview.DataSource.DisplayItems.IndexOf(App.tappedItem);

                App.tappedItem.ShowActionMenu = false;

                if (Device.RuntimePlatform != Device.macOS)
                    Device.BeginInvokeOnMainThread(() => { listview.RefreshListViewItem(previousIndex, previousIndex, true); });
            }

            if (App.tappedItem == (e.ItemData as Item))
            {
                if (Device.RuntimePlatform == Device.macOS)
                {
                    var previousIndex = listview.DataSource.DisplayItems.IndexOf(App.tappedItem);
                    Device.BeginInvokeOnMainThread(() => { listview.RefreshListViewItem(previousIndex, previousIndex, true); });
                }

                App.tappedItem = null;
            }
            else
            {
                App.tappedItem = e.ItemData as Item;
                if(!App.tappedItem.ShowActionMenu)
                    App.tappedItem.ShowActionMenu = true;

                if (Device.RuntimePlatform == Device.macOS)
                {
                    var visibleLines = this.listview.GetVisualContainer().ScrollRows.GetVisibleLines();
                    var firstIndex = visibleLines[visibleLines.FirstBodyVisibleIndex].LineIndex;
                    var lastIndex = visibleLines[visibleLines.LastBodyVisibleIndex].LineIndex;
                    Device.BeginInvokeOnMainThread(() => { listview.RefreshListViewItem(firstIndex, lastIndex, true); });
                }
                else
                {
                    var currentIndex = listview.DataSource.DisplayItems.IndexOf(e.ItemData);
                    Device.BeginInvokeOnMainThread(() => { listview.RefreshListViewItem(currentIndex, currentIndex, true); });
                }
            }

            Debug.WriteLine("Exiting - Behavior: Item Tapped / Double Tapped");
        }
        #endregion
    }
}
