using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tess.Model;
using Xamarin.Forms;
using Acr;
using Tess.Common;

namespace Tess.ViewModel
{
    public class vmLanguagePage : ViewModelBase
    {
        #region binding
        private String langset = "";
        public String LangSet
        {
            get { return langset; }
            set
            {
                langset = value;
                Set(nameof(LangSet), ref value);

            }
        }

        private List<Lingua> lingue;
        public List<Lingua> Lingue
        {
            get { return lingue; }
            set
            {
                lingue = value;
                Set(nameof(Lingue), ref value);
            }
        }

        private Lingua langselected;
        public Lingua LangSelected
        {
            get { return langselected; }
            set
            {
                langselected = value;
                Set(nameof(LangSelected), ref value);
            }

        }

        public ICommand SaveLanguage
        {
            get
            {
                return new RelayCommand(() => { SaveLang(); });
            }

        }

        public ICommand gotoPage
        {
            get
            {
                return new RelayCommand(() => { vmMenuPage.changePage("AboutPage"); });

            }


        }

        private String opstatus;
        public String OpStatus
        {
            get { return opstatus; }
            set
            {
                opstatus = value;
                Set(nameof(OpStatus), ref value);
            }
        }

        private string pickertitle = Traduzioni.Language_pickerTitle;
        public string pickerTitle
        {
            get { return pickertitle; }
            set
            {
                pickertitle = value;
                Set(nameof(pickerTitle), ref value);
            }

        }

        private string savebutton = "ddddd";
        public string saveButton
        {
            get { return savebutton; }
            set
            {
                savebutton = value;
                Set(nameof(saveButton), ref value);
            }

        }
        #endregion
        public vmLanguagePage()
        {
            this.Lingue = new List<Lingua>();
            Lingue.Add(new Lingua { langCode = "en-GB", languageName = "English" });
            Lingue.Add(new Lingua { langCode = "it-IT", languageName = "Italian" });

            try
            {
                var i = ManageData.getLang();
                LangSelected = Lingue.Where(x => x.langCode == i.LangCode).FirstOrDefault();
            }
            catch (Exception e)
            {
                //OpStatus = "Not set" + e;

            }
        }

        public bool SaveLang()
        {

            var lingua = new Language();
            lingua.LangCode = LangSelected.langCode;
            lingua.LanguageName = LangSelected.languageName;
            var op = 0;
            ManageData.delLanguage();
            try
            {
                op = ManageData.InsertLanguage(lingua);
                Acr.UserDialogs.UserDialogs.Instance.ShowSuccess("");
                var linguaCorrente = new System.Globalization.CultureInfo(lingua.LangCode);
                Traduzioni.Culture = linguaCorrente;
            }
            catch (Exception e)
            {


            }

            return true;
        }

    }
}
