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

        public static double hoursFromLastCheckIn(string YearDay, string Year) {

            double tot = 0;
            TimeSpan interval = new TimeSpan();
            var i = ManageData.getDay(YearDay,Year);
            var p = ManageData.getOpenedDayHours(i.IdDaysWorked.ToString());
            foreach (var item in p)
            {
                string c = item.CheckIn;
                DateTime cdt = DateTime.Parse(c);
                DateTime now = DateTime.Now;

                interval = now - cdt;

                tot = tot + interval.TotalHours;
            }         

            return tot;
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
