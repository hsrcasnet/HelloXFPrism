using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelloXFPrism.Model;

namespace HelloXFPrism.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        private readonly List<Item> items;

        public MockDataStore()
        {
            this.items = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Price = 1.50m, Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item",Price = 4.0444m, Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Price = 1.30m, Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item",Price = 1234.04m, Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Price = 0.00m, Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Price = 0.00m, Description = "This is an item description." },
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            this.items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = this.items.FirstOrDefault(arg => arg.Id == item.Id);
            this.items.Remove(oldItem);
            this.items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = this.items.FirstOrDefault(arg => arg.Id == id);
            this.items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(this.items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(this.items);
        }
    }
}