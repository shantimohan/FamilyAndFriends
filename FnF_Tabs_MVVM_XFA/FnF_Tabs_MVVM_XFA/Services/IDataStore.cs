using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FnF_Tabs_MVVM_XFA.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<List<T>> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(string category, bool forceRefresh = false);
    }
}
