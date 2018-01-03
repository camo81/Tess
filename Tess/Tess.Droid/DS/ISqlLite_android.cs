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
using Tess.Droid.DS;
using SQLite;
using System.IO;

[assembly: Dependency(typeof(ISqlLite_android))]

namespace Tess.Droid.DS
{
    class ISqlLite_android : Tess.Model.ISqlLite
    {
        public SQLiteConnection getConnection()
        {
            var sqliteFilename = "Tess.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            // Create the connection
            var conn = new SQLite.SQLiteConnection(path);
            // Return the database connection
            return conn;
        }


    }
}