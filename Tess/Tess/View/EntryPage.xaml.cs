using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tess.Model;
using Xamarin.Forms;

namespace Tess.View
{
    public partial class EntryPage : ContentPage
    {
        public EntryPage(DaysWorked day)
        {
            InitializeComponent();
            this.BindingContext = new ViewModel.vmEntryPage(day);
        }
    }
}
