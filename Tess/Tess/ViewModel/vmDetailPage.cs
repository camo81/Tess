﻿using Acr.UserDialogs;
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

namespace Tess.ViewModel
{
    public class vmDetailPage : ViewModelBase
    {

        #region Binding

        private String label1 = Traduzioni.Detail_l1;
        public String Label1
        {
            get { return label1; }
            set
            {
                label1 = value;
                Set(nameof(Label1), ref value);
            }
        }

        private String label2 = Traduzioni.Detail_l2;
        public String Label2
        {
            get { return label2; }
            set
            {
                label2 = value;
                Set(nameof(Label2), ref value);
            }
        }


        private String label3 = Traduzioni.Detail_l3;
        public String Label3
        {
            get { return label3; }
            set
            {
                label3 = value;
                Set(nameof(Label3), ref value);
            }
        }


        private String label4 = Traduzioni.Detail_l4;
        public String Label4
        {
            get { return label4; }
            set
            {
                label4 = value;
                Set(nameof(Label4), ref value);
            }
        }

        private String label5 = Traduzioni.Detail_l5;
        public String Label5
        {
            get { return label5; }
            set
            {
                label5 = value;
                Set(nameof(Label5), ref value);
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
   
        private String weektotstring;
        public String WeekTotString
        {
            get { return weektotstring; }
            set
            {
                weektotstring = value;
                Set(nameof(WeekTotString), ref value);
            }
        }

        private String dayavgstring;
        public String DayAvgString
        {
            get { return dayavgstring; }
            set
            {
                dayavgstring = value;
                Set(nameof(DayAvgString), ref value);
            }
        }

        private String hoursnumstring;
        public String HoursNumString
        {
            get { return hoursnumstring; }
            set
            {
                hoursnumstring = value;
                Set(nameof(HoursNumString), ref value);
            }
        }

        private String daysnumstring;
        public String DaysNumString
        {
            get { return daysnumstring; }
            set
            {
                daysnumstring = value;
                Set(nameof(DaysNumString), ref value);
            }
        }

        private String remainhours;
        public String RemainHours
        {
            get { return remainhours; }
            set
            {
                remainhours = value;
                Set(nameof(RemainHours), ref value);
            }
        }

        private String remainavg;
        public String RemainAvg
        {
            get { return remainavg; }
            set
            {
                remainavg = value;
                Set(nameof(RemainAvg), ref value);
            }
        }

        public String detailheading;
        public String DetailHeading
        {
            get { return detailheading; }
            set
            {
                detailheading = value;
                Set(nameof(DetailHeading), ref value);
            }
        }

        public String abouttext = Traduzioni.About_text;
        public String AboutText
        {
            get { return abouttext; }
            set
            {
                abouttext = value;
                Set(nameof(AboutText), ref value);
            }
        }

        public ICommand CommandOpenUrl
        {
            get
            {
                return new RelayCommand(() => { OpenUrl(); });
            }

        }

        #endregion

        public int DayOfYear;
        public int DayOfWeek;
        public int Year;
        public string Date;
        public double HoursNum = 0;
        public int DaysNum = 0;
        public double DayAvg = 0;
        bool check;
        double WeekTot = 0;


        public vmDetailPage()
        {
            var now = DateTime.Now;
            DayOfYear = now.DayOfYear;
            DayOfWeek = (int)now.DayOfWeek;
            Year = now.Year;
            Date = now.ToString();
            //get impostazioni richieste
            check = getReqSettings();

            WeekTot = Math.Round( HoursSum(),2);
            WeekTotString = WeekTot.ToString();

            DayAvg = Math.Round(HoursAvg(), 2);
            DayAvgString = DayAvg.ToString();

            RemainHours = (HoursNum - WeekTot).ToString();

            DetailHeading = "" + WeekTot + " / " + HoursNum + "H";

            setProgressBar(HoursNum,WeekTot);

            if (!check)
            {
                SetReq();
            }

        }

        public void OpenUrl()
        {
            var url = Traduzioni.General_GitUri;
            Uri uri = new Uri(url);

           
        }

        public double HoursSum()
        {

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

                        }

                    }


                }

            }

            return WeekTot.TotalHours;


        }

        public double HoursAvg()
        {
            DateTime weekStartDT = DateTime.Now.AddDays(-(DayOfWeek - 1));
            DateTime weekEndDT = DateTime.Now.AddDays((-1));

            int weekStart = DayOfYear - (DayOfWeek - 1);
            int weekEnd = DayOfYear - 1;

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

            double DayTot = 0;

            DaysWorked day = ManageData.getDay(DayYear, Year);

            if (day != null)
            {
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

        public async void SetReq()
        {

            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = Traduzioni.Main_confirmMessage,
                OkText = Traduzioni.Main_confirm_yes,
                CancelText = Traduzioni.Main_confirm_no,
            });
            if (result)
            {
                vmMenuPage.changePage("SettingsPage");
            }
            else
            {
                vmMenuPage.changePage("MainPage");
            }

        }

        public bool getReqSettings()
        {
            // get impostazioni
            var DaysNumber = ManageData.getValue("WdSelected");
            if (DaysNumber != null)
            {
                int x = 0;
                if (Int32.TryParse(DaysNumber.SettingValue, out x))
                {
                    DaysNum = Int32.Parse(DaysNumber.SettingValue);
                    DaysNumString = DaysNum.ToString();
                }
            }
            else
            {
                return false;
            }


            var HoursNumber = ManageData.getValue("OsSelected");
            if (HoursNumber != null)
            {
                double y = 0;
                if (Double.TryParse(HoursNumber.SettingValue, out y))
                {
                    HoursNum = Double.Parse(HoursNumber.SettingValue);
                    HoursNumString = HoursNum.ToString();
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public TimeSpan getMinBreak()
        {
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
    }
}
