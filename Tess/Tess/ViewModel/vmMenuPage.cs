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


        private Page mainpage = new View.MainPage();
        public Page mainPage
        {
            get { return mainpage; }
            set
            {
                mainpage = value;
                Set(nameof(mainPage), ref value);
            }
        }

        private Page settingspage = new View.SettingsPage();
        public Page settingsPage
        {
            get { return settingspage; }
            set
            {
                settingspage = value;
                Set(nameof(settingsPage), ref value);
            }
        }

        private Page languagepage = new View.LanguagePage();
        public Page languagePage
        {
            get { return languagepage; }
            set
            {
                languagepage = value;
                Set(nameof(languagePage), ref value);
            }
        }

        public ICommand gotoPage
        {
            get
            {
                return new RelayCommand<Page>((page) => { changePage(page); });
            }


        }

        #endregion

        public vmMenuPage()
        {

        }


        public static void changePage(Page page)
        {
            MasterDetailPage newPage = App.Current.MainPage as MasterDetailPage;
            newPage.Detail = new NavigationPage(page);
            newPage.IsPresented = false;

        }
    }


}
