using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tess.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Tess.Common;
using Acr.UserDialogs;
using Tess.View;

namespace Tess.ViewModel
{
    public class vmSettingsPage : ViewModelBase
    {
        #region binding
        private String opstatus;
        public String opStatus
        {
            get { return opstatus; }
            set
            {
                opstatus = value;
                Set(nameof(opStatus), ref value);

            }
        }

        private bool isselected = true;
        public bool isSelected
        {
            get { return isselected; }
            set
            {
                isselected = value;
                Set(nameof(isSelected), ref value);

            }
        }

        private String pickerdescription = Traduzioni.Settings_pickerDesc;
        public String pickerDescription
        {
            get { return pickerdescription; }
            set
            {
                pickerdescription = value;
                Set(nameof(pickerDescription), ref value);

            }
        }

        private String pickerwdtitle = Traduzioni.Settings_picker1;
        public String pickerWdTitle
        {
            get { return pickerwdtitle; }
            set
            {
                pickerwdtitle = value;
                Set(nameof(pickerWdTitle), ref value);

            }
        }

        private String pickerhdtitle = Traduzioni.Settings_picker2;
        public String pickerHdTitle
        {
            get { return pickerhdtitle; }
            set
            {
                pickerhdtitle = value;
                Set(nameof(pickerHdTitle), ref value);

            }
        }

        private String breakhour = Traduzioni.Settings_picker3;
        public String breakHour
        {
            get { return breakhour; }
            set
            {
                breakhour = value;
                Set(nameof(breakHour), ref value);

            }
        }

        private String breakminute = Traduzioni.Settings_picker4;
        public String breakMinute
        {
            get { return breakminute; }
            set
            {
                breakminute = value;
                Set(nameof(breakMinute), ref value);

            }
        }

        private String switchtext = Traduzioni.Settings_sound;
        public String switchText
        {
            get { return switchtext; }
            set
            {
                switchtext = value;
                Set(nameof(switchText), ref value);

            }
        }

        private String wdset = "";
        public String WdSet
        {
            get { return wdset; }
            set
            {
                wdset = value;
                Set(nameof(WdSet), ref value);

            }
        }

        private List<WorkingDays> giorni;
        public List<WorkingDays> Giorni
        {
            get { return giorni; }
            set
            {
                giorni = value;
                Set(nameof(Giorni), ref value);
            }
        }

        private HoursWeek osselected;
        public HoursWeek OsSelected

        {
            get { return osselected; }
            set
            {
                osselected = value;
                Set(nameof(OsSelected), ref value);
            }

        }

        private WorkingDays wdselected;
        public WorkingDays WdSelected
        {
            get { return wdselected; }
            set
            {
                wdselected = value;
                Set(nameof(WdSelected), ref value);
            }

        }

        private int osindex;
        public int OsIndex
        {
            get { return osindex; }
            set
            {
                osindex = value;
                Set(nameof(OsIndex), ref value);
            }

        }

        private int wdindex;
        public int WdIndex
        {
            get { return wdindex; }
            set
            {
                wdindex = value;
                Set(nameof(WdIndex), ref value);
            }

        }

        private int bhindex;
        public int BHIndex
        {
            get { return bhindex; }
            set
            {
                bhindex = value;
                Set(nameof(BHIndex), ref value);
            }

        }

        private int bmindex;
        public int BMIndex
        {
            get { return bmindex; }
            set
            {
                bmindex = value;
                Set(nameof(BMIndex), ref value);
            }

        }

        private MinBreakHour bhselected;
        public MinBreakHour BHSelected
        {
            get { return bhselected; }
            set
            {
                bhselected = value;
                Set(nameof(BHSelected), ref value);
            }

        }

        private MinBreakMinute bmselected;
        public MinBreakMinute BMSelected
        {
            get { return bmselected; }
            set
            {
                bmselected = value;
                Set(nameof(BMSelected), ref value);
            }

        }

        private List<HoursWeek> oresettimana;
        public List<HoursWeek> oreSettimana
        {
            get { return oresettimana; }
            set
            {
                oresettimana = value;
                Set(nameof(oreSettimana), ref value);
            }
        }

        private List<MinBreakHour> minhbreak;
        public List<MinBreakHour> minHBreak
        {
            get { return minhbreak; }
            set
            {
                minhbreak = value;
                Set(nameof(minHBreak), ref value);
            }
        }

        private List<MinBreakMinute> minmbreak;
        public List<MinBreakMinute> minMBreak
        {
            get { return minmbreak; }
            set
            {
                minmbreak = value;
                Set(nameof(minMBreak), ref value);
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

        public ICommand gotoPage
        {
            get
            {
                return new RelayCommand(() => { functions.pageAsync(new AboutPage(),false); });
            }


        }
        #endregion

        public vmSettingsPage()
        {
            this.Giorni = new List<WorkingDays>();
            Giorni.Add(new WorkingDays { number = "1" });
            Giorni.Add(new WorkingDays { number = "2" });
            Giorni.Add(new WorkingDays { number = "3" });
            Giorni.Add(new WorkingDays { number = "4" });
            Giorni.Add(new WorkingDays { number = "5" });
            Giorni.Add(new WorkingDays { number = "6" });
            Giorni.Add(new WorkingDays { number = "7" });

            this.oreSettimana = new List<HoursWeek>();
            for (int i = 1; i < 101; i++)
            {
                oreSettimana.Add(new HoursWeek { number = ""+i });
            }

            this.minHBreak = new List<MinBreakHour>();
            for (int i = 0; i < 5; i++)
            {
                minHBreak.Add(new MinBreakHour { number = "" + i });
            }

            this.minMBreak = new List<MinBreakMinute>();
            for (int i = 0; i <61 ; i++)
            {
                minMBreak.Add(new MinBreakMinute { number = "" + i });
            }


            #region GetValue
            try
            {
                /*Breve spiegazione triste: i raccatta la mia impostazione corrente. con linq faccio una query sulla lista giorni dicendogli che 
                 l'attributo number dell'oggetto deve essere uguale al valore raccattato con getvalue*/

                var i = ManageData.getValue("WdSelected");
                WdSelected = Giorni.Where(x => x.number == i.SettingValue).FirstOrDefault();
            }
            catch (Exception e)
            {
                //opStatus = "Not set" + e;

            }

            try
            {
                var i = ManageData.getValue("OsSelected");
                OsSelected = oreSettimana.Where(x => x.number == i.SettingValue).FirstOrDefault();
            }
            catch (Exception e)
            {
                //opStatus = "Not set" + e;

            }

            try
            {
                var i = ManageData.getValue("BHSelected");
                BHSelected = minHBreak.Where(x => x.number == i.SettingValue).FirstOrDefault();
            }
            catch (Exception e)
            {
                //opStatus = "Not set" + e;

            }

            try
            {
                var i = ManageData.getValue("BMSelected");
                BMSelected = minMBreak.Where(x => x.number == i.SettingValue).FirstOrDefault();
            }
            catch (Exception e)
            {
                //opStatus = "Not set" + e;

            }
            try
            {
                var i = ManageData.getValue("PlaySound");
                if (i.SettingValue == "False")
                {
                    isSelected = false;
                }
                
            }
            catch (Exception e)
            {
                //opStatus = "Not set" + e;

            }
            #endregion

        }

        #region functions
        public void SaveSet()
        {
            var dati = new Settings();

            //set della variabile WdSelected
            dati.SettingName = "WdSelected";
            int Wd = 0;
            if (WdSelected != null)
            {
                dati.SettingValue = WdSelected.number;

                //verifico se esiste già un setting per WdSelected
                var tmp = ManageData.getValue("WdSelected");

                if ((tmp == null))
                {
                    // se non esiste faccio l'insert
                    try
                    {
                        Wd = ManageData.InsertSettings(dati);
                    }
                    catch (Exception e)
                    {
                        opStatus = "" + e;
                    }
                }
                //altrimenti l'update
                else
                {
                    dati.IdSetting = tmp.IdSetting;
                    Wd = ManageData.UpdateSettings(dati);
                }

            }

            //set della variabile OsSelected
            dati.SettingName = "OsSelected";
            int Os = 0;
            if (OsSelected != null)
            {
                dati.SettingValue = OsSelected.number;

                //verifico se esiste già un setting per OsSelected
                var tmp = ManageData.getValue("OsSelected");

                if ((tmp == null))
                {
                    // se non esiste faccio l'insert
                    try
                    {
                        Os = ManageData.InsertSettings(dati);
                    }
                    catch (Exception e)
                    {
                        opStatus = "" + e;
                    }
                }
                //altrimenti l'update
                else
                {
                    dati.IdSetting = tmp.IdSetting;
                    Os = ManageData.UpdateSettings(dati);
                }

            }


            //set della variabile BHSelected
            dati.SettingName = "BHSelected";
            int Bh = 0;
            if (BHSelected != null)
            {
                dati.SettingValue = BHSelected.number;


                //verifico se esiste già un setting per BHSelected
                var tmp = ManageData.getValue("BHSelected");

                if ((tmp == null))
                {
                    // se non esiste faccio l'insert
                    try
                    {
                        Bh = ManageData.InsertSettings(dati);
                    }
                    catch (Exception e)
                    {
                        opStatus = "" + e;
                    }
                }
                //altrimenti l'update
                else
                {
                    dati.IdSetting = tmp.IdSetting;
                    Bh = ManageData.UpdateSettings(dati);
                }

            }



            //set della variabile BMSelected
            dati.SettingName = "BMSelected";
            int Bm = 0;
            if (BMSelected != null)
            {
                dati.SettingValue = BMSelected.number;


                //verifico se esiste già un setting per BMSelected
                var tmp = ManageData.getValue("BMSelected");

                if ((tmp == null))
                {
                    // se non esiste faccio l'insert
                    try
                    {
                        Bm = ManageData.InsertSettings(dati);
                    }
                    catch (Exception e)
                    {
                        opStatus = "" + e;
                    }
                }
                //altrimenti l'update
                else
                {
                    dati.IdSetting = tmp.IdSetting;
                    Bm = ManageData.UpdateSettings(dati);
                }


            }

            //set della variabile isSelected
            dati.SettingName = "PlaySound";


            dati.SettingValue = isSelected.ToString(); 
            var tmp2 = ManageData.getValue("PlaySound");
            if ((tmp2 == null))
            {
                // se non esiste faccio l'insert
                try
                {
                    Wd = ManageData.InsertSettings(dati);
                }
                catch (Exception e)
                {
                    opStatus = "" + e;
                }
            }
            else
            {
                dati.IdSetting = tmp2.IdSetting;
                Wd = ManageData.UpdateSettings(dati);
            }
            


            if ((Wd != 0) && (Os != 0))
            {
                bool isConvertible1 = false;
                bool isConvertible2 = false;
                int OsInt = 0;
                int WdInt = 0;

                isConvertible1 = int.TryParse(OsSelected.number, out OsInt);
                isConvertible2 = int.TryParse(WdSelected.number, out WdInt);

                if ( (isConvertible1) && (isConvertible2) && ( (OsInt / WdInt) > 15) )
                {
                    UserDialogs.Instance.ShowError(Traduzioni.Settings_LavoriTroppo, 4000);
                }
                else {
                    UserDialogs.Instance.ShowSuccess(Traduzioni.Settings_SaveSetOk);
                }
                
            }
            else
            {
                UserDialogs.Instance.ShowError(Traduzioni.Settings_SaveSetKo);
            }

            //opStatus = Wd + " / " + Os + " / " + Bh + " / " + Bm;

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
                ManageData.delSettings();
                BHIndex = -1;
                WdIndex = -1;
                OsIndex = -1;
                BMIndex = -1;
                isSelected = true;
            }

        }
        #endregion



    }
}
