using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GAIClient.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace GAIClient.Services
{
    public class DriverDataStore : IDataStore<Drivers>
    {
        private List<Drivers> _drivers;

        public DriverDataStore()
        {

        }

        private async  Task<List<Drivers>> GetDrivers()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(App.Address + "api/Driver/");
            var message = await response.Content.ReadAsStringAsync();

            return JArray.Parse(message).ToObject<List<Drivers>>();
        }


        public async Task<bool> AddItemAsync(Drivers item)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.PostAsJsonAsync(App.Address + "api/Driver/", item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync(Drivers item)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.PutAsJsonAsync(App.Address + "api/Driver/" + item.id, item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            //var json = new StringContent( , Encoding.UTF8, "application/json");
            //var response = client.PutAsync(new Uri(App.Address + "api/Driver/" + item.id), new StringContent() )
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Drivers> GetItemAsync(string id)
        {
            return Task.FromResult(_drivers.FirstOrDefault(p => p.id.ToString() == id));
        }

        public async Task<IEnumerable<Drivers>> GetItemsAsync(bool forceRefresh = false)
        {
            //return await Task.FromResult(_drivers ?? (_drivers = await GetDrivers()).AsEnumerable());
            return await Task.FromResult((_drivers = await GetDrivers()).AsEnumerable());
        }
    }
}