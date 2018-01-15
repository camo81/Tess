using System;
using Xamarin.Forms;

namespace Tess.View
{
    public partial class MenuPage : MasterDetailPage
    {
        public MenuPage()
        {
            InitializeComponent();
            this.BindingContext = new ViewModel.vmMenuPage();
            Detail = new NavigationPage(new MainPage());

        }

    }
}
