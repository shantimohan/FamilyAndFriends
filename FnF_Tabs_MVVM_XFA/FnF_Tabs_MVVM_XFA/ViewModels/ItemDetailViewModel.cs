using System;

using FnF_Tabs_MVVM_XFA.Models;

namespace FnF_Tabs_MVVM_XFA.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = $"ID: {(item?.Id)}";
            Item = item;
        }
    }
}
