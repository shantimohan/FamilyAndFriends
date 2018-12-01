using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FnF_Tabs_MVVM_XFA.Models;

namespace FnF_Tabs_MVVM_XFA.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        static List<Item> items = new List<Item>();

        public MockDataStore(string category)
        {
            if (items.Count == 0)
            {
                var mockItems = new List<Item>
                {
                    new Item { Id = 1, Name="First Family Member", DateOfBirth="Jan 1, 2018", Anniversary="Jun 1, 2018", Category="Family" },
                    new Item { Id = 2, Name="Second Family Member", DateOfBirth="Jan 1, 2018", Anniversary="Jun 1, 2018", Category="Family" },
                    new Item { Id = 3, Name="First Friend", DateOfBirth="Jan 1, 2018", Anniversary="Jun 1, 2018", Category="Friends" },
                    new Item { Id = 4, Name="Second Friend", DateOfBirth="Jan 1, 2018", Anniversary="Jun 1, 2018", Category="Friends" },
                    new Item { Id = 5, Name="Third Family Member", DateOfBirth="Jan 1, 2018", Anniversary="Jun 1, 2018", Category="Family" },
                    new Item { Id = 6, Name="Third Friend", DateOfBirth="Jan 1, 2018", Anniversary="Jun 1, 2018", Category="Friends" },
                    new Item { Id = 7, Name="Fourth Family Member", DateOfBirth="Jan 1, 2018", Anniversary="Jun 1, 2018", Category="Family" },
                    new Item { Id = 8, Name="Fourth Friend", DateOfBirth="Jan 1, 2018", Anniversary="Jun 1, 2018", Category="Friends" },
                    new Item { Id = 9, Name="Fifth Family Member", DateOfBirth="Jan 1, 2018", Anniversary="Jun 1, 2018", Category="Family" },
                };

                foreach (var item in mockItems)
                {
                    items.Add(item);
                }

                //TODO: Create the Table People in SQLite

                //TODO: Insert all mockItems into the table People

            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            //TODO: Insert item into table People

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            //TODO: Update item in table People

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            //TODO: Delete item from table People

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string category, bool forceRefresh = false)
        {
            return await Task.FromResult(items.Where(i => i.Category == category));
        }
    }
}