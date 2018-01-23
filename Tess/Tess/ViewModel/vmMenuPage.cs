using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tess.Common;
using Tess.View;
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


        private MainPage mainpage = new MainPage();
        public MainPage mainPage
        {
            get { return mainpage; }
            set
            {
                mainpage = value;
                Set(nameof(mainPage), ref value);
            }
        }

        private SettingsPage settingspage = new SettingsPage();
        public SettingsPage settingsPage
        {
            get { return settingspage; }
            set
            {
                settingspage = value;
                Set(nameof(settingsPage), ref value);
            }
        }

        private LanguagePage languagepage = new LanguagePage();
        public LanguagePage languagePage
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
                
                return new RelayCommand<Page>((page) => {

                    //get type of object passed
                    Type t = page.GetType();
                    //create a new instance of obj                    
                    page=Activator.CreateInstance(t) as Page;

                    functions.changePage(page); });
            }


        }

        #endregion

        public vmMenuPage(MasterDetailPage current)
        {

            bool check = functions.checkReqSet();
            Page page = new Page();
            
            if (check)
            {
                page = new MainPage();
            }
            else {
                page = new SettingsPage();
            }
            
            current.Detail = new NavigationPage(page);

        }

    }
}



