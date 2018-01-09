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
        public HoursWeek osSelected

        {
            get { return osselected; }
            set
            {
                osselected = value;
                Set(nameof(osSelected), ref value);
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
        #endregion

        public void SaveSet()
        {
            var dati = new Settings();

            //set della variabile WdSelected
            dati.SettingName = "WdSelected";
            dati.SettingValue = WdSelected.number;
            int u = 0;

            //verifico se esiste già un setting per WdSelected
            var tmp = ManageData.getValue("WdSelected");

            if ((tmp == null) )
            {
                // se non esiste faccio l'insert
                try
                {
                    u = ManageData.InsertSettings(dati);
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
                u = ManageData.UpdateSettings(dati);
            }


/*

            if ((u != 0) && (p != 0) && (MultiSave.Count > 0))
            {
                UserDialogs.Instance.ShowSuccess(Traduzioni.Settings_SaveSetOk);
            }
            else
            {
                UserDialogs.Instance.ShowError(Traduzioni.Settings_SaveSetKo);
            }*/

        }

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
                opStatus = "Not set" + e;

            }

            #endregion

        }



    }
}
