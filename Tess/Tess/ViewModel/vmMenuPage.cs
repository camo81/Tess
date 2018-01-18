using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tess.Common;
using Xamarin.Forms;

namespace Tess.ViewModel
{
    class vmMenuPage : ViewModelBase
    {
        #region binding
        private String main = Traduzioni.Menu_main;
        public String Main
        {
            get { return main; }
            set
            {
                main = value;
                Set(nameof(Main), ref value);
            }
        }

        private String settings = Traduzioni.Menu_settings;
        public String Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                Set(nameof(Settings), ref value);
            }
        }

        private String language = Traduzioni.Menu_language;
        public String Language
        {
            get { return language; }
            set
            {
                language = value;
                Set(nameof(Language), ref value);
            }
        }


        public ICommand gotoPage
        {
            get
            {
                return new RelayCommand<string>((page) => { changePage(page); });
            }


        }

        #endregion

        public vmMenuPage()
        {

        }


        public static void changePage(string page)
        {
            MasterDetailPage newPage = App.Current.MainPage as MasterDetailPage;

            switch (page)
            {
                case "MainPage":
                    Page gotoHome = new View.MainPage();
                    newPage.Detail = new NavigationPage(gotoHome);
                    break;

                case "SettingsPage":
                    Page gotoSettings = new View.SettingsPage();
                    newPage.Detail = new NavigationPage(gotoSettings);
                    break;

                case "LanguagePage":
                    Page gotoLang = new View.LanguagePage();
                    newPage.Detail = new NavigationPage(gotoLang);
                    break;

                case "AboutPage":
                    Page gotoAbout = new View.AboutPage();
                    newPage.Detail = new NavigationPage(gotoAbout);
                    break;

                case "DetailPage":
                    Page gotoDetail = new View.DetailPage();
                    newPage.Detail = new NavigationPage(gotoDetail);
                    break;

                default:
                    Page gotoPage = new View.MainPage();
                    newPage.Detail = new NavigationPage(gotoPage);
                    break;
            }
     
            newPage.IsPresented = false;

        }
    }


}
