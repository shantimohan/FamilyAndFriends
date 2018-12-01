using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FnF_Tabs_MVVM_XFA.Models;
using SQLite;

namespace FnF_Tabs_MVVM_XFA.Services
{
    public class FnFDataStore : IDataStore<Item>
    {
        static List<Item> items = new List<Item>();
        static SQLiteAsyncConnection dbConn;

        public FnFDataStore() { }

        //DBOP: Initialize db connection and open the people table (the only table)
        public FnFDataStore(string dbPath)
        {
            dbConn = new SQLiteAsyncConnection(dbPath);
            dbConn.CreateTableAsync<Item>().Wait();
            //TODO: The following two statements are to be run when the table structure changes
            //dbConn.DropTableAsync<Item>().Wait();
            //dbConn.CreateTableAsync<Item>().Wait();
        }

        public async Task RecreateDB()
        {
            await dbConn.DropTableAsync<Item>();
            await dbConn.CreateTableAsync<Item>();
        }

        //DBOP: Get total item count from people table
        public async Task<int> GetTotalItemCount()
        {
            return await dbConn.Table<Item>().CountAsync();
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            Debug.WriteLine($"AddItemAsync: item.Id = {item.Id}");
            Debug.WriteLine($"AddItemAsync: item.Category = {item.Category}");

            //DBOP: Insert item into table People
            await dbConn.InsertAsync(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            //DBOP: Update item in table People
            await dbConn.UpdateAsync(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Item item)
        {
            //DBOP: Delete item from table People
            await dbConn.DeleteAsync(item);

            return await Task.FromResult(true);
        }

        public async Task<List<Item>> GetItemAsync(int id)
        {
            //return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));

            return await dbConn.QueryAsync<Item>($"SELECT * FROM [people] WHERE [Id] = {id}");
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string category, bool forceRefresh = false)
        {
            //return await Task.FromResult(items.Where(x => x.Category == category));

            return await dbConn.QueryAsync<Item>($"SELECT * FROM [people] WHERE [Category] = \"{category}\"");
        }

        //DBOP: Get all existing data from people table
        public Task<List<Item>> GetAllPeopleAsync()
        {
            // return a list of people saved to the Item table in the database
            return dbConn.Table<Item>().ToListAsync();
        }
    }
}