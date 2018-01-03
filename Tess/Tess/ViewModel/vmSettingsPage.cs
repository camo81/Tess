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

        }



    }
}
