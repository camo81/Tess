using System;
using System.Threading;
using Tess.Model;
using Xamarin.Forms;

namespace Tess.Common
{
    static class functions
    {
        public static string idiotMessage()
        {
            var messages = Traduzioni.Loading_message;
            string[] messageList = messages.Split('|');
            var rnd = new Random();
            var message = messageList[rnd.Next(0, messageList.Length)];

            return message;
        }

        public static void pageAsync(Page page, bool animation = true)
        {

            MasterDetailPage tmp = Application.Current.MainPage as MasterDetailPage;
            tmp.Detail.Navigation.PushAsync(page, animation);

        }

        public static void changePage(Page page)
        {
            MasterDetailPage newPage = App.Current.MainPage as MasterDetailPage;
            newPage.Detail = new NavigationPage(page);
            newPage.IsPresented = false;

        }

        public static void changePage(string page)
        {
            MasterDetailPage newPage = App.Current.MainPage as MasterDetailPage;

            switch (page)
            {
                case "MainPage":
                    Page gotoHome = new View.MainPage();
                    newPage.Detail = new NavigationPage(gotoHome);
                    break;

                case "SettingsPage":
                    Page gotoSettings = new View.SettingsPage();
                    Page pippo = newPage.Detail.Navigation.NavigationStack[0];
                    pippo = gotoSettings;
                    break;

                case "LanguagePage":
                    Page gotoLang = new View.LanguagePage();
                    newPage.Detail = new NavigationPage(gotoLang);
                    break;

                case "AboutPage":
                    Page gotoAbout = new View.AboutPage();
                    newPage.Detail = new NavigationPage(gotoAbout);
                    break;

                case "DetailPage":
                    Page gotoDetail = new View.DetailPage();
                    newPage.Detail = new NavigationPage(gotoDetail);
                    break;

                default:
                    Page gotoPage = new View.MainPage();
                    newPage.Detail = new NavigationPage(gotoPage);
                    break;
            }

            newPage.IsPresented = false;

        }

        public static double hoursFromLastCheckIn(string YearDay, string Year) {

            double tot = 0;
            TimeSpan interval = new TimeSpan();
            var i = ManageData.getDay(YearDay,Year);
            if (i != null)

            {
                var p = ManageData.getOpenedDayHours(i.IdDaysWorked.ToString());
                foreach (var item in p)
                {
                    string c = item.CheckIn;
                    DateTime cdt = DateTime.Parse(c);
                    DateTime now = DateTime.Now;

                    interval = now - cdt;

                    tot = tot + interval.TotalHours;
                }
            }


            return tot;
        }

        public static bool checkReqSet() {

            var DaysNumber = ManageData.getValue("WdSelected");
            if (DaysNumber == null)
            {
                return false;
            }

            var HoursNumber = ManageData.getValue("OsSelected");
            if (HoursNumber == null)
            {
                return false;
            }


            return true;

        }

        public static string setProgressBar(double HoursNum, double WeekTot)
        {

            if (HoursNum < WeekTot)
            {
                WeekTot = HoursNum;
            }

            var perc = Math.Round(WeekTot / HoursNum, 2);
            IFormatProvider cultureUS = new System.Globalization.CultureInfo("en-US");
            return perc.ToString(cultureUS);
        }
    }

    public class Timer
    {
        private readonly TimeSpan _timeSpan;
        private readonly Action _callback;

        private static CancellationTokenSource _cancellationTokenSource;

        public Timer(TimeSpan timeSpan, Action callback)
        {
            _timeSpan = timeSpan;
            _callback = callback;
            _cancellationTokenSource = new CancellationTokenSource();
        }
        public void Start()
        {
            CancellationTokenSource cts = _cancellationTokenSource; // safe copy
            Device.StartTimer(_timeSpan, () =>
            {
                if (cts.IsCancellationRequested)
                {
                    return false;
                }
                _callback.Invoke();
                return true; //true to continuous, false to single use
            });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref _cancellationTokenSource, new CancellationTokenSource()).Cancel();
        }
    }
}
