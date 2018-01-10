using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tess.Model;

namespace Tess.ViewModel
{
    public class vmMainPage : ViewModelBase
    {
        #region binding
        public String mainlabel = "Wellcome to Tess";

        public String MainLabel
        {
            get { return mainlabel; }
            set
            {
                mainlabel = value;
                Set(nameof(MainLabel), ref value);
            }
        }

        private List<DaysWorked> worked;
        public List<DaysWorked> Worked
        {
            get { return worked; }
            set
            {
                worked = value;
                Set(nameof(Worked), ref value);
            }
        }

        public ICommand gotoPage
        {
            get
            {
                return new RelayCommand(() => { vmMenuPage.changePage("About"); });

            }


        }

        public ICommand CommandStopWatchIn
        {
            get
            {
                return new RelayCommand(() => { StopWatchIn(); });

            }


        }

        public ICommand CommandStopWatchOut
        {
            get
            {
                return new RelayCommand(() => { StopWatchOut(); });

            }


        }

        public string[] DaysName = new string[] { "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato", "Domenica" };


        #endregion

        public vmMainPage()
        {

            // get impostazioni
            var DaysNumber = ManageData.getValue("WdSelected");
            int DaysNum = 5;
            int x = 0;
            if (Int32.TryParse(DaysNumber.SettingValue, out x))
            {
                int DaysNumSettings = Int32.Parse(DaysNumber.SettingValue);
                if (DaysNumSettings > 5)
                {
                    DaysNum = DaysNumSettings;
                } 
            }

            var HoursNumber = ManageData.getValue("OsSelected");
            int HoursNum = 0;
            x = 0;
            if (Int32.TryParse(HoursNumber.SettingValue, out x))
            {
                HoursNum = Int32.Parse(HoursNumber.SettingValue);
            }


            // Creazione della listview
            this.Worked = new List<DaysWorked>();
            for (int i = 1; i <= DaysNum; i++)
            {
            Worked.Add(new DaysWorked { WeekDay = i.ToString(), DayName=DaysName[(i -1)] });
            }


            var pluto = DateTime.Now;
            var pippo = pluto.DayOfYear;
            var paperino = (int)pluto.DayOfWeek;
                MainLabel = pluto + "---"+ pippo + "---" + paperino;


        }

        #region functions
        public void StopWatchIn() {
        

        }
        public void StopWatchOut()
        {


        }
        #endregion
    }
}
