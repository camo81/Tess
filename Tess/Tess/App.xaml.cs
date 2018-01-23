using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tess.Common;
using Tess.Model;
using Tess.ViewModel;
using Xamarin.Forms;

namespace Tess
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            ManageData.CreaDataBase();
            //setto la lingua di default dell'applicazione
            var currentLang = ManageData.getLang();
            System.Globalization.CultureInfo lingua;
            if (currentLang == null)
            {
                lingua = new System.Globalization.CultureInfo("en-GB");
            }
            else
            {
                lingua = new System.Globalization.CultureInfo(currentLang.LangCode);
            }

            Traduzioni.Culture = lingua;
            MainPage = new View.MenuPage();

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
