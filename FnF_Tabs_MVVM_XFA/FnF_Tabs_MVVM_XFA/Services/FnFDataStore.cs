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
            //RecreateDB().Wait();
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
            return await dbConn.QueryAsync<Item>($"SELECT * FROM [people] WHERE [Id] = {id}");
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string category, bool forceRefresh = false)
        {
            return await dbConn.QueryAsync<Item>($"SELECT * FROM [people] WHERE [Category] = \"{category}\"");
        }
    }
}