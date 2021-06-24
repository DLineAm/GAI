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

        private string _text = string.Empty;

        public static BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(FloatingLabelInput), null, BindingMode.TwoWay);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set

            {
                SetValue(TextProperty, value);
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