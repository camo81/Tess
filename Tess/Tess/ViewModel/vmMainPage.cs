using Acr.UserDialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tess.Common;
using Tess.Model;
using Xamarin.Forms;

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

        private String percentage; 
        public String Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;
                Set(nameof(Percentage), ref value);
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
        public int DayOfYear;
        public int DayOfWeek;
        public int Year;
        public string Date;
        public double HoursNum = 0;
        public int DaysNum = 0;

        #endregion

        public vmMainPage()
        {
            var now = DateTime.Now;
            DayOfYear = now.DayOfYear;
            DayOfWeek = (int)now.DayOfWeek;
            Year = now.Year;
            Date = now.ToString();
            
            // get impostazioni
            var DaysNumber = ManageData.getValue("WdSelected");
            int x = 0;
            if (Int32.TryParse(DaysNumber.SettingValue, out x))
            {
                DaysNum = Int32.Parse(DaysNumber.SettingValue);
            }

            var HoursNumber = ManageData.getValue("OsSelected");            
            double y = 0;
            if (Double.TryParse(HoursNumber.SettingValue, out y))
            {
                HoursNum = Double.Parse(HoursNumber.SettingValue);                
            }

            // Creazione della listview
            createListView();

            //inserisco i workedDays se non esistono ancora
            insertWorkedDay();

            //Calcolo la somma settimanale di ore e inizializzo la progress bar
            double WeekTot = HoursSum();
            setProgressBar(HoursNum, WeekTot);





        }



        #region functions

        public void createListView()
        {
            int weekStart = DayOfYear - (DayOfWeek - 1);
            int weekEnd = DayOfYear + (7 - DayOfWeek);

            this.Worked = new List<DaysWorked>();
            for (int i = 1; i <= 7; i++)
            {
                double worked = HoursPerDay(weekStart.ToString(), Year.ToString());
                var timeSpan = TimeSpan.FromHours(worked);


                Worked.Add(new DaysWorked
                {
                    WeekDay = i.ToString(),
                    DayName = DaysName[(i - 1)],
                    WorkedHours = timeSpan.ToString()
                });

                weekStart += 1;
            }
        }

        public async void insertWorkedDay() {
            await Task.Delay(2000);
            UserDialogs.Instance.ShowLoading("wait", MaskType.Black);

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

        public void StopWatchIn()
        {
            var idDW = ManageData.getDay(DayOfYear.ToString(), Year.ToString());
            string IdDayWorked = idDW.IdDaysWorked.ToString();

            bool i = checkDayEntry(IdDayWorked);
            bool a = checkDayStatus(IdDayWorked);

            if ((i) && (a))
            {
                int x = 0;
                if (Int32.TryParse(IdDayWorked, out x))
                {
                    int IdDayWorkedInt = Int32.Parse(IdDayWorked);
                    DaysWorkedHours dati = new DaysWorkedHours();
                    var now = DateTime.Now;
                    dati.IdDaysWorked = IdDayWorkedInt;
                    dati.CheckIn = now.ToString();
                    if (ManageData.InsertDayHours(dati) == 1)
                    {
                        DependencyService.Get<IAudio>().PlayAudioFile("pop.mp3");
                        UserDialogs.Instance.ShowSuccess("Bella!");
                    }
                    else
                    {
                        DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                        UserDialogs.Instance.ShowError("Errore nell'inserimento");
                    }

                }
                else {
                    DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                    UserDialogs.Instance.ShowError("Errore nel casting a int");
                }

            }
           
        }

        public void StopWatchOut()
        {
            bool i = setCheckout();
            double WeekTot = HoursSum();
            setProgressBar(HoursNum,WeekTot);        
        }

        public bool checkDayEntry(string IdDayWorked)
        {
            
            var list = ManageData.getDayHours(IdDayWorked);
            if (list.Count() > 1)
            {
                DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                UserDialogs.Instance.ShowError("Hai già inserito due entrate per la giornata odierna");
                return false;
            }

            return true;
        }

        public bool checkDayStatus(string IdDayWorked)
        {

            var list = ManageData.getOpenedDayHours(IdDayWorked);
            if (list.Count > 0)
            {
                DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                UserDialogs.Instance.ShowError("Hai già un'entrata aperta");
                return false;
            }
            return true;
        }

        public bool setCheckout()    
        {
            var idDW = ManageData.getDay(DayOfYear.ToString(), Year.ToString());
            string IdDayWorked = idDW.IdDaysWorked.ToString();

            var list = ManageData.getOpenedDayHours(IdDayWorked);

            switch (list.Count)
            {
                case 0:
                    DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                    UserDialogs.Instance.ShowError("Non ci sono Check-in da chiudere");
                    return false;

                case 1:
                    
                    DaysWorkedHours dati = new DaysWorkedHours();
                    var now = DateTime.Now;
                    var current = list.FirstOrDefault();

                    dati.CheckIn = current.CheckIn;
                    dati.IdDaysWorked = current.IdDaysWorked;
                    dati.IdDaysWorkedHours = current.IdDaysWorkedHours;
                    dati.CheckOut = now.ToString();
                    if (ManageData.UpdateDayHours(dati) == 1)
                    {
                        DependencyService.Get<IAudio>().PlayAudioFile("woosh.mp3");
                        UserDialogs.Instance.ShowSuccess("Check-in chiuso");
                        return true;
                    }
                    else {
                        DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                        UserDialogs.Instance.ShowError("C'è statao un errore nell'inserimento");
                        return false;
                    }                 
                    
                case 2:
                    DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                    UserDialogs.Instance.ShowError("C'è statao un errore (2 checkin attivi)");
                    return false;
                    
                default:
                    DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                    UserDialogs.Instance.ShowError("il count della lista openeHours è null o maggiore di 2");
                    return false; 
            }
        }

        public double HoursSum() {
            int weekStart = DayOfYear - (DayOfWeek - 1);
            int weekEnd = DayOfYear + (7 - DayOfWeek);

            double WeekTot = 0;

            for (int i = weekStart; i <= weekEnd; i++)
            {
                int wstart = i;
                string ws = wstart.ToString();
                string y = Year.ToString();
                var d = ManageData.getDay(ws, y);
                if (d != null)
                {
                          
                    string idDay = d.IdDaysWorked.ToString();
                    var f = ManageData.getClosedDayHours(idDay);
                    if (f != null)
                    {
                        //fetch della lista
                        foreach (var interval in f)
                        {
                            var start = interval.CheckIn;
                            var end = interval.CheckOut;

                            DateTime startDT = Convert.ToDateTime(start);
                            DateTime endDT = Convert.ToDateTime(end);


                            var hours = (endDT - startDT).TotalHours;
                            WeekTot = WeekTot + hours;

                        }

                            //per ogni riga calcolare la differenza tra check in e out

                        }



                }

            }

            return WeekTot;


        }

        public double HoursPerDay(string DayYear, string Year)
        {

            double DayTot = 0;

            DaysWorked day = ManageData.getDay(DayYear, Year);

            if (day != null) {
                var idDay = day.IdDaysWorked;
                var f = ManageData.getClosedDayHours(idDay.ToString());

                if (f != null)
                {
                    //fetch della lista
                    foreach (var interval in f)
                    {
                        var start = interval.CheckIn;
                        var end = interval.CheckOut;

                        DateTime startDT = Convert.ToDateTime(start);
                        DateTime endDT = Convert.ToDateTime(end);


                        var hours = (endDT - startDT).TotalHours;
                        DayTot = DayTot + hours;
                    }

                }
            }

            return DayTot;


        }

        public void setProgressBar(double HoursNum, double WeekTot)
        {

            if (HoursNum < WeekTot)
            {
                WeekTot = HoursNum;
            }

            var perc = Math.Round(WeekTot / HoursNum, 2);
            Percentage = perc.ToString();
        }

        #endregion
    }
}
