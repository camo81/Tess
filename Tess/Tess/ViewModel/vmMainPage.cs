using Acr.UserDialogs;
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
            int x = 0;
            if (Int32.TryParse(DaysNumber.SettingValue, out x))
            {
                int DaysNum = Int32.Parse(DaysNumber.SettingValue);
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
            for (int i = 1; i <= 7; i++)
            {
            Worked.Add(new DaysWorked { WeekDay = i.ToString(), DayName=DaysName[(i -1)] });
            }

            //inserisco i workedDays se non esistono ancora
            insertWorkedDay();





        }

        #region functions
        public void StopWatchIn() {
        

        }
        public void StopWatchOut()
        {


        }

        public async void insertWorkedDay() {
            await Task.Delay(1000);
            UserDialogs.Instance.ShowLoading("wait", MaskType.Black);
            var now = DateTime.Now;
            int DayOfYear = now.DayOfYear;
            int DayOfWeek = (int)now.DayOfWeek;
            int Year = now.Year;
            string Date = now.ToString();

            int weekStart = DayOfYear - (DayOfWeek -1);
            int weekEnd = DayOfYear + (7 - DayOfWeek);

            //var t = ManageData.getAll();
            //ManageData.delDays();

            for (int i = weekStart; i <= weekEnd; i++)
            {
                int wstart = i;
                string ws = wstart.ToString();
                string y = Year.ToString(); 
                var d = ManageData.getDay(ws,y);
                if (d == null)
                {
                    DaysWorked dati = new DaysWorked();
                    dati.Datetime = Date;
                    dati.WeekDay = DayOfWeek.ToString();
                    dati.YearDay = ws;
                    dati.Year = Year.ToString();

                    var ins = ManageData.InsertDay(dati);
                }

            }
            UserDialogs.Instance.HideLoading();
        }
        #endregion
    }
}
