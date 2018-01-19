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
    public class vmEntryPage : ViewModelBase
    {
        #region binding
        private TimeSpan in1;

        public TimeSpan In1
        {
            get { return in1; }
            set
            {
                in1 = value;
                Set(nameof(In1), ref value);
            }
        }

        private TimeSpan in2;

        public TimeSpan In2
        {
            get { return in2; }
            set
            {
                in2 = value;
                Set(nameof(In2), ref value);
            }
        }

        private TimeSpan out1;

        public TimeSpan Out1
        {
            get { return out1; }
            set
            {
                out1 = value;
                Set(nameof(Out1), ref value);
            }
        }

        private TimeSpan out2;

        public TimeSpan Out2
        {
            get { return out2; }
            set
            {
                out2 = value;
                Set(nameof(Out2), ref value);
            }
        }

        public ICommand SaveSettings
        {
            get
            {
                return new RelayCommand(() => { SaveSet(); });
            }

        }

        public ICommand DelSettings
        {
            get
            {
                return new RelayCommand(() => { DelSet(); });
            }

        }
        #endregion

        public DaysWorked SelectedDay;

        public vmEntryPage(DaysWorked day)
        {
            
            var i = ManageData.getDay(day.YearDay, day.Year);
            var p = ManageData.getDayHours(i.IdDaysWorked.ToString());
            SelectedDay = day;

            if (p != null)
            {
                switch (p.Count)
                {
                    case 1:
                        DateTime in1D = DateTime.Parse(p[0].CheckIn);
                        In1 = in1D.TimeOfDay;

                        DateTime out1D;
                        if (!DateTime.TryParse(p[0].CheckOut, out out1D))
                        {
                            Out1 = TimeSpan.Zero;
                        }
                        else
                        {
                            Out1 = out1D.TimeOfDay;
                        }

                        break;

                    case 2:

                        in1D = DateTime.Parse(p[0].CheckIn);
                        In1 = in1D.TimeOfDay;

                        if (!DateTime.TryParse(p[0].CheckOut, out out1D))
                        {
                            Out1 = TimeSpan.Zero;
                        }
                        else
                        {
                            Out1 = out1D.TimeOfDay;
                        }


                        DateTime in2D = DateTime.Parse(p[1].CheckIn);
                        In2 = in2D.TimeOfDay;

                        DateTime out2D;
                        if (!DateTime.TryParse(p[1].CheckOut, out out2D))
                        {
                            out2D = DateTime.Now;
                            
                        }
                        else {
                            Out2 = out2D.TimeOfDay;
                        }
                        

                        break;

                    default:
                        break;
                }

            }
            


        }

        #region functions
        public void SaveSet()
        {
            int SelectedYear;
            int SelectedYearDay;

            try
            {
                SelectedYear = Int32.Parse(SelectedDay.Year);
            }
            catch (FormatException e)
            {
                SelectedYear = -1;
            }

            try
            {
                SelectedYearDay = Int32.Parse(SelectedDay.YearDay);
            }
            catch (FormatException e)
            {
                SelectedYearDay = -1;
            }

            if ((SelectedYear > 0) && (SelectedYearDay > 0))
            {
                DateTime theDate = new DateTime(SelectedYear, 1, 1).AddDays(SelectedYearDay - 1);

                DateTime In1ToSave = theDate + In1;
                DateTime Out1ToSave = theDate + Out1;

                DateTime In2ToSave = theDate + In2;
                DateTime Out2ToSave = theDate + Out2;



                if ((Out1ToSave < In1ToSave) || (Out2ToSave < In2ToSave))
                {
                    UserDialogs.Instance.ShowError(Traduzioni.Entry_gandalf);
                }
                else {

                    ManageData.delDayHours(SelectedDay.IdDaysWorked);


                    if ((In1ToSave.TimeOfDay.Ticks != 0) && (Out1ToSave.TimeOfDay.Ticks != 0))
                    {
                        DaysWorkedHours dati = new DaysWorkedHours();

                        dati.IdDaysWorked = SelectedDay.IdDaysWorked;
                        dati.CheckIn = In1ToSave.ToString();
                        dati.CheckOut = Out1ToSave.ToString();

                        ManageData.InsertDayHours(dati);
                    }

                    if ((In2ToSave.TimeOfDay.Ticks != 0) && (Out2ToSave.TimeOfDay.Ticks != 0))
                    {
                        DaysWorkedHours dati = new DaysWorkedHours();

                        dati.CheckIn = In2ToSave.ToString();
                        dati.CheckOut = Out2ToSave.ToString();

                        ManageData.InsertDayHours(dati);
                    }


                    UserDialogs.Instance.ShowSuccess(Traduzioni.Settings_SaveSetOk);

                    vmMenuPage.changePage(new View.MainPage());

                }
                




            }





        }

        public async void DelSet()
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = Traduzioni.Setting_confirmMessage,
                OkText = Traduzioni.Setting_confirm_yes,
                CancelText = Traduzioni.Setting_confirm_no
            });
            if (result)
            {
                ManageData.delDayHours(SelectedDay.IdDaysWorked);
                In1 = In2 = Out1 = Out2 = TimeSpan.FromSeconds(0.1);
                vmMenuPage.changePage(new View.MainPage());

            }

        }
        #endregion

    }
}
