using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GAIClient.Models;
using GAIClient.Views;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace GAIClient.ViewModels
{
    public class DriversViewModel : BaseViewModel
    {
        public static DriversViewModel Instance { get; private set; }
        public Command LoadItemsCommand { get; }

        public Command AddDriverCommand { get; }

        public Command<Drivers> ItemTapped { get; }

        private List<Drivers> _drivers = new List<Drivers>();
        public List<Drivers> Drivers { get => _drivers; set => SetProperty(ref _drivers, value); }

        private Drivers _driver;
        public Drivers Driver { get => _driver; set => SetProperty(ref _driver, value); }

        private string _driversCount;
        public string DriversCount { get => _driversCount; set => SetProperty(ref _driversCount, value); }

        private bool _loadingIsRunning;
        public bool LoadingIsRunning { get => _loadingIsRunning; set => SetProperty(ref _loadingIsRunning, value); }

        public DriversViewModel()
        {
            this.Title = "Водители";

            LoadItemsCommand = new Command(async () => await GetDrivers());
            AddDriverCommand = new Command(AddDriver);
            ItemTapped = new Command<Drivers>(OnItemSelected);
            //var thread = new Thread(GetDrivers);

            Instance = this;
            
            LoadItemsCommand.Execute(null);
            //thread.Start();
        }

        private async void AddDriver()
        {
            await GoToDriversPage(new Drivers() {id = -1});
        }

        private async Task GetDrivers()
        {
            DriversCount = "Загрузка";
            LoadingIsRunning = true;
            try
            {
                Drivers = (await DriversDataStore.GetItemsAsync(true)).ToList();
                DriversCount = $"Найдено {Drivers.Count} записей";
            }
            catch (Exception e)
            {
                DriversCount = "Не удалось подключиться к серверу";
                await Application.Current.MainPage.DisplayAlert("", e.Message, "k");
            }
            finally
            {
                LoadingIsRunning = false;
            }
           
            //try
            //{
            //    var client = new HttpClient();
            //    var response = await client.GetAsync(App.Address + "api/Driver");
            //    var message = await response.Content.ReadAsStringAsync();

            //    Drivers = JArray.Parse(message).ToObject<List<Drivers>>();

            //    DriversCount = $"Найдено {Drivers.Count} записей";
            //    LoadingIsRunning = false;
            //}
            //catch (Exception e)
            //{
            //    LoadingIsRunning = false;
            //    DriversCount = "Не удалось подключиться к серверу";
            //    await Application.Current.MainPage.DisplayAlert("", e.Message, "k");
            //}
        }

        async void OnItemSelected(Drivers item)
        {
            if (item == null)
                return;
            //await Application.Current.MainPage.DisplayAlert(item.lastname + " " + item.firstname + " " + item.middlename, item.address, "k");

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");

            await GoToDriversPage(item);

            //await Shell.Current.GoToAsync($"{nameof(DriverDeatilsPage)}?{nameof(DirverDetailsViewModel.DriverId)}={item.id}").ContinueWith(Refresh);


        }

        private async Task GoToDriversPage(Drivers item)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(DriverDeatilsPage)}?{nameof(DirverDetailsViewModel.DriverId)}={item.id}");

            //Refresh();
        }

        public void Refresh(Task obj = null)
        {
            LoadItemsCommand.Execute(null);
        }
    }
}