using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Tess.Common;
using Tess.Droid.DS;
using Android.Media;
using Tess.Model;

[assembly: Dependency(typeof(IAudio_android))]
namespace Tess.Droid.DS

{

    public class IAudio_android : IAudio
    {
        public IAudio_android()
        {
        }

        public void PlayAudioFile(string fileName)
        {
            var fx = ManageData.getValue("PlaySound");
            if (fx.SettingValue != "False")
            {
                var player = new MediaPlayer();
                var fd = global::Android.App.Application.Context.Assets.OpenFd(fileName);

                player.Prepared += (s, e) =>
                {
                    player.Start();
                };

                player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
                player.Prepare();
            }

        }
    }



}