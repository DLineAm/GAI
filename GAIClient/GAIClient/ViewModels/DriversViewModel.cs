using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GAI_API.Models;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace GAIClient.ViewModels
{
    public class DriversViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; }

        public Command<Drivers> ItemTapped { get; }

        private List<Drivers> _drivers = new List<Drivers>();
        public List<Drivers> Drivers { get => _drivers; set => SetProperty(ref _drivers, value); }

        private Drivers _driver;
        public Drivers Driver { get => _driver; set => SetProperty(ref _driver, value); }

        public string Title { get; set; } = "Водители";

        private string _driversCount;
        public string DriversCount { get => _driversCount; set => SetProperty(ref _driversCount, value); }

        private bool _loadingIsRunning;
        public bool LoadingIsRunning { get => _loadingIsRunning; set => SetProperty(ref _loadingIsRunning, value); }

        public DriversViewModel()
        {
            LoadItemsCommand = new Command(async () => await GetDrivers());
            ItemTapped = new Command<Drivers>(OnItemSelected);
            //var thread = new Thread(GetDrivers);
            DriversCount = "Загрузка";
            LoadingIsRunning = true;
            LoadItemsCommand.Execute(null);
            //thread.Start();
        }

        private async Task<List<Drivers>> GetDrivers()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(App.Address + "api/Driver");
                var message = await response.Content.ReadAsStringAsync();

                Drivers = JArray.Parse(message).ToObject<List<Drivers>>();
                DriversCount = $"Найдено {Drivers.Count} записей";
                LoadingIsRunning = false;
                return JArray.Parse(message).ToObject<List<Drivers>>();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("", e.Message, "k");
                return null;
            }
        }

        async void OnItemSelected(Drivers item)
        {
            if (item == null)
                return;
            await Application.Current.MainPage.DisplayAlert(item.lastname + " " + item.firstname + " " + item.middlename, item.address, "k");

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

    }
}