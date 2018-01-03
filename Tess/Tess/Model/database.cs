using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace Tess.Model
{
    class database
    {
    }

    public interface ISqlLite { SQLiteConnection getConnection(); }

    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int IdSetting { get; set; }

        [Unique]
        public string SettingName { get; set; }

        public string SettingValue { get; set; }

        public override string ToString()
        {
            return string.Format("[Numbers: IdSetting={0}, SettingName={1}, SettingValue={2}", IdSetting, SettingName, SettingValue);
        }
    }

    public class Language
    {
        [PrimaryKey, AutoIncrement]
        public int IdLanguage { get; set; }

        [Unique]
        public string LanguageName { get; set; }

        [Unique]
        public string LangCode { get; set; }


        public override string ToString()
        {
            return string.Format("[Language: IdLanguage={0}, LanguageName={1}, LangCode{2}", IdLanguage, LanguageName, LangCode);
        }
    }

    public static class ManageData
    {
        public const string TabellaSettings = "Settings";
        public const string TabellaLanguage = "Language";
        static SQLiteConnection database = DependencyService.Get<ISqlLite>().getConnection();


        public static void CreaDataBase()
        {
            database.CreateTable<Settings>();
            database.CreateTable<Language>();
        }

        #region language functions

        public static int delLanguage()
        {
            return database.DeleteAll<Language>();
        }

        public static int InsertLanguage(Language lang)
        {
            return database.Insert(lang);
        }

        public static Language getLang()
        {

            try
            {
                string query = $"SELECT * FROM [{TabellaLanguage}]";
                var lista = database.Query<Language>(query);
                return lista.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion
    }



}
