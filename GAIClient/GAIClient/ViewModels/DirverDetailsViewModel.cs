using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GAIClient.Models;
using GAIClient.Services;
using GAIClient.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GAIClient.ViewModels
{
    [QueryProperty(nameof(DriverId), nameof(DriverId))]
    public class DirverDetailsViewModel : BaseViewModel
    {
        private Drivers _driver;
        public Drivers Driver { get => _driver; set => SetProperty(ref _driver, value); }

        private byte[] _image;
        public byte[] Image { get => _image; set => SetProperty(ref _image, value); }

        public Command ApplyCommand { get; }

        public Command PickImageCommand { get; }

        public DirverDetailsViewModel()
        {
            ApplyCommand = new Command(Apply);
            PickImageCommand = new Command(PickImage);
        }

        private async void PickImage()
        {
            await Application.Current.MainPage.DisplayAlert("Информация", @"При выборе фотографии водителя убедитесь, что файл соответствует следующим параметрам:
1.	Соотношение сторон изображения равно 3x4;
2.	Изображение вертикальное;
3.	Размер изображение не более 2 мегабайт;
4.	Изображение формата JPG или PNG.
", "k");

            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Выберите фото" });

            var stream = await photo.OpenReadAsync();

            var result = ImageSource.FromStream(() => stream);

            var streamImageSource = (StreamImageSource)result;
            var cancellationToken = CancellationToken.None;
            var task = streamImageSource.Stream(cancellationToken);
            var sream = task.Result;

            var ms = new MemoryStream();
            await sream.CopyToAsync(ms);

            Image = ms.ToArray();

        }

        private async void Apply()
        {
            if (_passportNumber.Length != 10)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Поле с серией и нормером паспорта должно содержать 10 символов", "k");
                return;
            }

            if (string.IsNullOrWhiteSpace(Driver.firstname) ||
                string.IsNullOrWhiteSpace(Driver.lastname) ||
                string.IsNullOrWhiteSpace(Driver.middlename) ||
                _passportNumber.Length == 0 ||
                string.IsNullOrWhiteSpace(Driver.address) ||
                string.IsNullOrWhiteSpace(Driver.address_life) ||
                Driver.phone.Length == 0 ||
                string.IsNullOrWhiteSpace(Driver.email) ||
                Image == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", @"Обязательные поля (с *) должны быть заполнены!", "k");
                return;
            }

            Driver.passport_serial = Convert.ToInt32(_passportNumber.Substring(0, 4));
            Driver.passport_number = Convert.ToInt32(_passportNumber.Substring(4));

            Driver.image = Image;

            //Добавление

            if (DriverId == -1)
            {
                if (!await DriversDataStore.AddItemAsync(Driver))
                    await Application.Current.MainPage.DisplayAlert("Ошибка", @"Ошибка при изменении записи, попробуйте позже", "k");
                DriversViewModel.Instance.Refresh();
                await Shell.Current.GoToAsync($"..");
                return;
            }

            //Изменение

            if (!await DriversDataStore.UpdateItemAsync(Driver))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", @"Ошибка при изменении записи, попробуйте позже", "k");
                return;
            }

            DriversViewModel.Instance.Refresh();
            await Shell.Current.GoToAsync($"..");

        }

        public bool Answer = false;

        private int _driverId;
        public int DriverId
        {
            get => _driverId;
            set
            {
                SetProperty(ref _driverId, value);
                UpdateDriverInfo();
            }
        }


        private string _passportNumber;
        public string PassportNumber
        {
            get => _passportNumber;
            set => SetProperty(ref _passportNumber, value);
        }

        private string ParsePassportString()
        {
            return Driver == null
                ? string.Empty
                : Driver.passport_serial.ToString() + Driver.passport_number.ToString();
        }


        private async void UpdateDriverInfo()
        {
            if (DriverId == -1)
            {
                Driver = new Drivers();
                return;
            }
            Driver = await DriversDataStore.GetItemAsync(DriverId.ToString());

            if (Driver == null) return;

            Image = Driver.image;

            PassportNumber = ParsePassportString();
        }
    }
}