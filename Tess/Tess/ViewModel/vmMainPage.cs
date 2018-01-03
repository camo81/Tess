using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #endregion
        public vmMainPage()
        {

            var pluto = DateTime.Now;
            var pippo = pluto.DayOfYear;
            var paperino = (int)pluto.DayOfWeek;
                MainLabel = pluto + "---"+ pippo + "---" + paperino;
        }




    }
}
