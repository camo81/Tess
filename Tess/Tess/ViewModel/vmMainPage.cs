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
using Tess.View;
using Xamarin.Forms;

namespace Tess.ViewModel
{
    public class vmMainPage : ViewModelBase
    {
        #region binding

        private Timer timer;

        private TimeSpan totalSeconds = new TimeSpan(0, 0, 0, 0);
        public TimeSpan TotalSeconds
        {
            get { return totalSeconds; }
            set { Set(ref totalSeconds, value); }
        }


        private String updown = "ic_thumb_down_outline_grey600_48dp.png";

        public String upDown
        {
            get { return updown; }
            set
            {
                updown = value;
                Set(nameof(upDown), ref value);
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
                return new RelayCommand(() => { functions.pageAsync(new AboutPage()); });

            }


        }

        public ICommand gotoDetail
        {
            get
            {
                return new RelayCommand(() => { functions.pageAsync(new DetailPage()); });

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

        public ICommand CommandEditRow
        {
            get
            {
                return new RelayCommand<DaysWorked>((idR) => { EditRow(idR); });
            }

        }

        public ICommand CommandDelRow
        {
            get
            {
                return new RelayCommand<DaysWorked>((idR) => { DelRow(idR); });
            }

        }

        public string[] DaysName = new string[] { Traduzioni.Monday, Traduzioni.Tuesday, Traduzioni.Wednesday, Traduzioni.Thursday, Traduzioni.Friday, Traduzioni.Saturday, Traduzioni.Sunday };
        public int DayOfYear;
        public int DayOfWeek;
        public int Year;
        public string Date;
        public double HoursNum = 0;
        public int DaysNum = 0;
        bool check;

        #endregion

        public vmMainPage()
        {
            var now = DateTime.Now;
            DayOfYear = now.DayOfYear;
            DayOfWeek = (int)now.DayOfWeek;
            Year = now.Year;
            Date = now.ToString();

            //get impostazioni richieste
            check = getReqSettings();
            //Set di updown
            setUpDown();
            // Creazione della listview
            createListView();
            //inserisco i workedDays se non esistono ancora
            insertWorkedDay();
            //Calcolo la somma settimanale di ore e inizializzo la progress bar
            double WeekTot = HoursSum();
            setProgressBar(HoursNum, WeekTot);

            //set del timer
            setTimer();

            if (!check)
            {
                SetReq();
            }

        }



        #region functions

        public void setTimer() {
            //get delle ore giornaliere
            double hours = HoursPerDay(DayOfYear.ToString(), Year.ToString());
            double lastCheckIn = functions.hoursFromLastCheckIn(DayOfYear.ToString(), Year.ToString());

            TimeSpan hoursTS = new TimeSpan();
            hoursTS = TimeSpan.FromHours(hours);

            TimeSpan lastCheckInTS = new TimeSpan();
            lastCheckInTS = TimeSpan.FromHours(lastCheckIn);

            //sommo le ore a totaleseconds
            totalSeconds = totalSeconds + hoursTS + lastCheckInTS;
            timer = new Timer(TimeSpan.FromSeconds(1), Count);
            TotalSeconds = totalSeconds;

            if (lastCheckIn > 0)
            {
                timer.Start();
            }
        }

        private void Count()
        {
            
            if (totalSeconds.TotalSeconds == 0)
            {
                //do something after hitting 0, in this example it just stops/resets the timer
                //StopTimerCommand();
                TotalSeconds = totalSeconds.Add(new TimeSpan(0, 0, 0, 1));
            }
            else
            {
                TotalSeconds = totalSeconds.Add(new TimeSpan(0, 0, 0, 1));
            }
        }

        private void StopTimerCommand()
        {
            TotalSeconds = new TimeSpan(0, 0, 0, 10);
            timer.Stop();
        }

        public bool getReqSettings() {
            // get impostazioni
            var DaysNumber = ManageData.getValue("WdSelected");
            if (DaysNumber != null)
            {
                int x = 0;
                if (Int32.TryParse(DaysNumber.SettingValue, out x))
                {
                    DaysNum = Int32.Parse(DaysNumber.SettingValue);
                }
            }
            else {
                return false;
            }


            var HoursNumber = ManageData.getValue("OsSelected");
            if (HoursNumber != null)
            {
                double y = 0;
                if (Double.TryParse(HoursNumber.SettingValue, out y))
                {
                    HoursNum = Double.Parse(HoursNumber.SettingValue);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public void setUpDown() {
            //ore/giorno attuali 
            double HAvg = HoursAvg();

            // ore/giorno richieste
            double HperDayReq = HoursNum / DaysNum;

            if (HAvg >= HperDayReq)
            {
                upDown = "ic_thumb_up_outline_grey600_48dp.png";
            }


        }

        public void createListView()
        {

            DateTime weekStartDT = DateTime.Now.AddDays(-(DayOfWeek - 1));
            DateTime weekEndDT = DateTime.Now.AddDays((7 - DayOfWeek));
            

            this.Worked = new List<DaysWorked>();
            int cont = 0;
            for (DateTime i = weekStartDT; i <= weekEndDT; i = i.AddDays(1))
            {
                string upDownD = "ic_thumb_down_outline_grey600_18dp.png";
                TimeSpan timeSpan;
                cont +=1;
                double worked = HoursPerDay(i.DayOfYear.ToString() , i.Year.ToString());
                timeSpan = TimeSpan.FromHours(worked);
                int p = (int)i.DayOfWeek;

                if ((DaysNum > 0) && (HoursNum > 0))
                {
                    TimeSpan ReqTimeSpan = TimeSpan.FromHours(HoursNum / DaysNum);

                    if (timeSpan >= ReqTimeSpan)
                    {
                        upDownD = "ic_thumb_up_outline_grey600_18dp.png";
                    }

                }

                Worked.Add(new DaysWorked
                {
                    WeekDay = p.ToString(),
                    DayName = DaysName[(cont - 1)],
                    WorkedHours = timeSpan.ToString(),
                    Year = i.Year.ToString(),
                    YearDay = i.DayOfYear.ToString(),
                    WorkedUpDown = upDownD
                });

            }

        }

        public async void insertWorkedDay() {
            await Task.Delay(2000);
            var message = Common.functions.idiotMessage();
            UserDialogs.Instance.ShowLoading(message, MaskType.Black);

            DateTime weekStartDT = DateTime.Now.AddDays( -(DayOfWeek - 1) );
            DateTime weekEndDT = DateTime.Now.AddDays((7 - DayOfWeek));


            //var t = ManageData.getAllDays();
            //ManageData.delDays();

            //TODO: vedere come gestire gli anni bisestili. O con il bool leap o meglio facendo il ciclo su un elemento datetime invece che int!

            for (DateTime i = weekStartDT; i <= weekEndDT; i=i.AddDays(1) )
            {                
                string h = i.ToString();
                int wstart = i.DayOfYear;
                string ws = wstart.ToString();
                string y = i.Year.ToString();
                var d = ManageData.getDay(ws,y);
                if (d == null)
                {
                    DaysWorked dati = new DaysWorked();
                    int wdint = (int)i.DayOfWeek;
                    dati.Datetime = i.ToString();
                    dati.WeekDay = wdint.ToString();
                    dati.YearDay = i.DayOfYear.ToString();
                    dati.Year = i.Year.ToString();
                     
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
                        UserDialogs.Instance.ShowSuccess(Traduzioni.Main_insert);
                    }
                    else
                    {
                        DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                        UserDialogs.Instance.ShowError(Traduzioni.Main_insertError2);
                    }

                }
                else {
                    DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                    UserDialogs.Instance.ShowError(Traduzioni.Main_castError);
                }

                timer.Start();

            }
           
        }

        public void StopWatchOut()
        {
            bool i = setCheckout();
            double WeekTot = HoursSum();
            setProgressBar(HoursNum,WeekTot);
            functions.changePage(new View.MainPage());
            timer.Stop();
        }

        public bool checkDayEntry(string IdDayWorked)
        {
            
            var list = ManageData.getDayHours(IdDayWorked);
            if (list.Count() > 1)
            {
                DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                UserDialogs.Instance.ShowError(Traduzioni.Main_moreThan2);
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
                UserDialogs.Instance.ShowError(Traduzioni.Main_1opened);
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
                    UserDialogs.Instance.ShowError(Traduzioni.Main_noToClose);
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
                        UserDialogs.Instance.ShowSuccess(Traduzioni.Main_Closed);
                        return true;
                    }
                    else {
                        DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                        UserDialogs.Instance.ShowError(Traduzioni.Main_insertError);
                        return false;
                    }                 
                    
                case 2:
                    DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                    UserDialogs.Instance.ShowError(Traduzioni.Main_moreThan1);
                    return false;
                    
                default:
                    DependencyService.Get<IAudio>().PlayAudioFile("error.mp3");
                    UserDialogs.Instance.ShowError(Traduzioni.Main_countError);
                    return false; 
            }
        }

        public double HoursSum() {

            DateTime weekStartDT = DateTime.Now.AddDays(-(DayOfWeek - 1));
            DateTime weekEndDT = DateTime.Now.AddDays((7 - DayOfWeek));

            TimeSpan WeekTot = new TimeSpan();

            for (DateTime i = weekStartDT; i <= weekEndDT; i = i.AddDays(1))
                {
                int wstart = i.DayOfYear;
                string ws = wstart.ToString();
                string y = Year.ToString();
                var d = ManageData.getDay(ws, y);
                if (d != null)
                {
                          
                    string idDay = d.IdDaysWorked.ToString();
                    var f = ManageData.getClosedDayHours(idDay);
                    if (f != null)
                    {

                        foreach (var interval in f)
                        {
                            var start = interval.CheckIn;
                            var end = interval.CheckOut;

                            DateTime startDT = Convert.ToDateTime(start);
                            DateTime endDT = Convert.ToDateTime(end);

                            TimeSpan hours = endDT - startDT;
                            WeekTot = WeekTot + hours;

                        }

                        // se c'è una pausa, verifico la durata minima
                        if (f.Count == 2)
                        {
                            //Timespan della pausa
                            DateTime CO1 = DateTime.Parse(f[0].CheckOut);
                            DateTime CI2 = DateTime.Parse(f[1].CheckIn);
                            TimeSpan BR = CI2 - CO1;

                            //Timespan minimo richiesto dai settings
                            TimeSpan MinReq = getMinBreak();

                            if (BR < MinReq)
                            {
                                WeekTot = WeekTot - (MinReq - BR);
                            }
                            double Tot = WeekTot.TotalHours;

                            if (Tot < 0)
                            {
                                WeekTot = new TimeSpan(0,0,0);
                            }


                        }

                    }


                }

            }

            return WeekTot.TotalHours;


        }

        public TimeSpan getMinBreak() {
            // get ore min break
            int MBH = 0;
            var Hr = ManageData.getValue("BHSelected");
            if (Hr != null)
            {
                bool isConvertible = false;
                isConvertible = int.TryParse(Hr.SettingValue, out MBH);

            }


            // get minutes min break
            int MBM = 0;
            var Mn = ManageData.getValue("BMSelected");
            if (Mn != null)
            {
                bool isConvertible = false;
                isConvertible = int.TryParse(Mn.SettingValue, out MBM);

            }

            TimeSpan MinBreak = new TimeSpan(MBH, MBM, 0);

            return MinBreak;
        }

        public double HoursAvg()
        {
            DateTime weekStartDT = DateTime.Now.AddDays(-(DayOfWeek - 1));
            DateTime weekEndDT = DateTime.Now.AddDays((-1));

            int weekStart = DayOfYear - (DayOfWeek - 1);
            int weekEnd = DayOfYear -1;

            TimeSpan WeekTot = new TimeSpan();
            int Days = 0;


            for (DateTime i = weekStartDT; i <= weekEndDT; i = i.AddDays(1))
                {
                int wstart = i.DayOfYear;
                string ws = wstart.ToString();
                string y = Year.ToString();
                var d = ManageData.getDay(ws, y);
                if (d != null)
                {

                    string idDay = d.IdDaysWorked.ToString();
                    var f = ManageData.getClosedDayHours(idDay);
                    if (f != null)
                    {
                        Days++;
                        foreach (var interval in f)
                        {
                            var start = interval.CheckIn;
                            var end = interval.CheckOut;

                            DateTime startDT = Convert.ToDateTime(start);
                            DateTime endDT = Convert.ToDateTime(end);


                            TimeSpan hours = endDT - startDT;
                            WeekTot = WeekTot + hours;

                        }

                        // se c'è una pausa, verifico la durata minima
                        if (f.Count == 2)
                        {
                            //Timespan della pausa
                            DateTime CO1 = DateTime.Parse(f[0].CheckOut);
                            DateTime CI2 = DateTime.Parse(f[1].CheckIn);
                            TimeSpan BR = CI2 - CO1;

                            //Timespan minimo richiesto dai settings
                            TimeSpan MinReq = getMinBreak();

                            if (BR < MinReq)
                            {
                                WeekTot = WeekTot - (MinReq - BR);
                            }

                        }

                    }



                }

            }

            return WeekTot.TotalHours / Days;


        }

        public double HoursPerDay(string DayYear, string Year)
        {

            TimeSpan DayTot = new TimeSpan();

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


                        TimeSpan hours = endDT - startDT;
                        DayTot = DayTot + hours;
                    }

                    // se c'è una pausa, verifico la durata minima
                    if (f.Count == 2)
                    {
                        //Timespan della pausa
                        DateTime CO1 = DateTime.Parse(f[0].CheckOut);
                        DateTime CI2 = DateTime.Parse(f[1].CheckIn);
                        TimeSpan BR = CI2 - CO1;

                        //Timespan minimo richiesto dai settings
                        TimeSpan MinReq = getMinBreak();

                        if (BR < MinReq)
                        {
                            DayTot = DayTot - (MinReq - BR);                           
                        }

                    }

                }
            }

            double Tot = DayTot.TotalHours;
            if (Tot < 0)
            {
                return 0;
            }
            else {
                return DayTot.TotalHours;
            }
            


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

        public void EditRow(DaysWorked p1) {
            DaysWorked day = ManageData.getDay(p1.YearDay, p1.Year);
            functions.pageAsync(new EntryPage(day));     
        }

        public async void DelRow(DaysWorked p1)
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = Traduzioni.Setting_confirmMessage,
                OkText = Traduzioni.Setting_confirm_yes,
                CancelText = Traduzioni.Setting_confirm_no
            });
            if (result)
            {
                DaysWorked day = ManageData.getDay(p1.YearDay, p1.Year);
                ManageData.delDayHours(day.IdDaysWorked);
                UserDialogs.Instance.ShowSuccess(Traduzioni.Settings_SaveSetOk);
                functions.changePage(new MainPage());
            }

            
        }

        public async void SetReq()
        {

            {

                var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Message = Traduzioni.Main_confirmMessage,
                    OkText = Traduzioni.Main_confirm_yes,
                    CancelText = Traduzioni.Main_confirm_no,
                });
                if (result)
                {
                    functions.changePage(new View.SettingsPage());
                }
                else
                {
                    functions.changePage(new View.MainPage());
                }

            }
        }

        #endregion
    }
}

