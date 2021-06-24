using GAIClient.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace GAIClient.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}