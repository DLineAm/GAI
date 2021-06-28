using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GAIClient.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FloatingLabelInput : ContentView
    {
        string placeholder = string.Empty;

        public FloatingLabelInput()
        {
            InitializeComponent();

            if (string.IsNullOrEmpty(EntryContent.Text))
            {
                LabelTitle.IsVisible = false;
            }

            EntryContent.BindingContext = this;
            LabelTitle.BindingContext = this;
        }

        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FloatingLabelInput), null,
                BindingMode.TwoWay);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        private string _text = string.Empty;

        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(FloatingLabelInput), null, BindingMode.TwoWay);


        public string Text
        {
            get => (string)GetValue(TextProperty);
            set

            {
                SetValue(TextProperty, value);
                LabelTitle.IsVisible = !string.IsNullOrWhiteSpace(value);
                //if (!string.IsNullOrEmpty(EntryContent.Text))
                //{
                //    Placeholder = EntryContent.Placeholder;
                //    LabelTitle.IsVisible = true;
                //}
                //else
                //{
                //    EntryContent.Placeholder = Placeholder;
                //    LabelTitle.IsVisible = false;
                //}
            }
        }

        public static BindableProperty EntryMaskProperty =
            BindableProperty.Create(nameof(EntryMask), typeof(string), typeof(FloatingLabelInput), null,
                BindingMode.TwoWay);

        public string EntryMask
        {
            get => (string)GetValue(EntryMaskProperty);
            set => SetValue(EntryMaskProperty, value);
        }

        public static BindableProperty TextIsEnabledProperty =
            BindableProperty.Create(nameof(TextIsEnabled), typeof(bool), typeof(FloatingLabelInput), true, BindingMode.TwoWay);

        public bool TextIsEnabled
        {
            get => (bool)GetValue(TextIsEnabledProperty);
            set => SetValue(TextIsEnabledProperty, value);
        }

        public static BindableProperty EntryKeyboardProperty =
            BindableProperty.Create(nameof(EntryKeyboard), typeof(Keyboard), typeof(FloatingLabelInput), Keyboard.Default, BindingMode.TwoWay);

        public Keyboard EntryKeyboard { get => (Keyboard)GetValue(EntryKeyboardProperty); set => SetValue(EntryKeyboardProperty, value); }

        void Handle_Focused(object sender, FocusEventArgs e)
        {
            LabelTitle.IsVisible = true;

            if (string.IsNullOrEmpty(EntryContent.Text))
            {
                placeholder = EntryContent.Placeholder;
                EntryContent.Placeholder = string.Empty;
            }

        }

        void Handle_Unfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(EntryContent.Text)) return;

            EntryContent.Placeholder = placeholder;
            LabelTitle.IsVisible = false;

        }
    }
}