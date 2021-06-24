using GAIClient.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GAIClient.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _login;
        public string Login { get => _login; set => SetProperty(ref _login, value); }

        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }

        private int _incorrentCounter;

        private bool _inputIsEnabeld;
        public bool InputIsEnabled
        {
            get => _inputIsEnabeld; set
            {
                _inputIsEnabeld = value;
                OnPropertyChanged();

            }
        }

        //private bool _timerIsAlive;
        private int _elapsedTime = 60;

        private bool _errorIsVisible;

        public bool ErrorIsVisible
        {
            get => _errorIsVisible;
            set
            {
                _errorIsVisible = value;
                OnPropertyChanged();
            }
        }

        private string _errorText;

        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                OnPropertyChanged();
            }
        }


        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            InputIsEnabled = true;

            try
            {
                //var elapsedTime = GetPreferences("Elapsed Time", 0.0.ToString());

                var elapsedTime = Convert.ToInt32(GetPreferences("Elapsed Time", "0"));

                if (elapsedTime == 0.0) return;

                _elapsedTime = elapsedTime;
                SetEnabled(false);
                Device.StartTimer(TimeSpan.FromSeconds(1), EnforceTime);

            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("Exception", e.Message, "k");


            }
        }

        private string GetPreferences(string name, string defaultvalue)
        {
            var result = Preferences.Get(name, defaultvalue);
            if (result == defaultvalue)
                Preferences.Set(name, defaultvalue);
            return result;
        }

        private async void OnLoginClicked(object obj)
        {
            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", !SetIncorrentCounter()
                    ? $"Оба поля должны быть заполнены! \nКоличество попыток: {3 - _incorrentCounter}."
                    : "Оба поля должны быть заполнены! \nВвод заблокирован на 1 минуту.", "k");
                return;
            }

            if (Login != "inspector" || Password != "inspector")
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", !SetIncorrentCounter()
                    ? $"Введены неверные логин и пароль. \nКоличество попыток: {3 - _incorrentCounter}."
                    : "Введены неверные логин и пароль. \nВвод заблокирован на 1 минуту.", "k");
                return;
            }

            //Password = string.Empty;
            //Login = string.Empty;

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private bool SetIncorrentCounter()
        {
            var isCleared = false;

            ++_incorrentCounter;
            if (_incorrentCounter == 3)
            {
                _incorrentCounter = 0;
                isCleared = !isCleared;
                SetEnabled(false);
                Device.StartTimer(TimeSpan.FromSeconds(1), EnforceTime);
            }

            return isCleared;
        }

        private bool EnforceTime()
        {
            --_elapsedTime;
            Preferences.Set("Elapsed Time", ((double)_elapsedTime).ToString());
            ErrorText = $"Слишком много попыток. \nПовторите попытку через {_elapsedTime} сек.";
            if (_elapsedTime == 0)
            {
                _elapsedTime = 60;
                SetEnabled(true);
                return false;
            }

            return true;
        }

        private void SetEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                InputIsEnabled = true;
                ErrorIsVisible = false;
                return;
            }
            InputIsEnabled = false;
            ErrorIsVisible = true;
            ErrorText = "Слишком много попыток. \nПовторите попытку через 60 сек.";
        }
    }
}
