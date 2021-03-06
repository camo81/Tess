﻿using System;
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

    public class DaysWorked
    {
        [PrimaryKey, AutoIncrement]
        public int IdDaysWorked { get; set; }

        public string Datetime { get; set; }

        public string WeekDay { get; set; }

        public string YearDay { get; set; }

        public string Year { get; set; }

        public string DayName { get; set;}

        public string WorkedHours { get; set; } = "-";

        public string WorkedUpDown { get; set; } = "";

        public string MonthDay { get; set; }

        public override string ToString()
        {
            return string.Format("[Language: IdDaysWorked={0}, Datetime={1}, WeekDay{2} ,YearDay{3}, Year{4}", IdDaysWorked, Datetime, WeekDay, YearDay, Year);
        }
    }

    public class DaysWorkedHours
    {
        [PrimaryKey, AutoIncrement]
        public int IdDaysWorkedHours { get; set; }

        public int IdDaysWorked { get; set; }

        public string CheckIn { get; set; } = "";

        public string CheckOut { get; set; } = "";

        public override string ToString()
        {
            return string.Format("[Language: IdDaysWorkedHours={0}, IdDaysWorked={1}, CheckIn={2}, CheckOut{3}", IdDaysWorkedHours, IdDaysWorked, CheckIn, CheckOut);
        }
    }

    public static class ManageData
    {
        public const string TabellaSettings = "Settings";
        public const string TabellaLanguage = "Language";
        public const string TabellaDays = "DaysWorked";
        public const string TabellaDaysHours = "DaysWorkedHours";
        static SQLiteConnection database = DependencyService.Get<ISqlLite>().getConnection();


        public static void CreaDataBase()
        {
            database.CreateTable<Settings>();
            database.CreateTable<Language>();
            database.CreateTable<DaysWorked>();
            database.CreateTable<DaysWorkedHours>();
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

        #region settings functions

        public static int InsertSettings(Settings dati)
        {
            return database.Insert(dati);
        }

        public static Settings getValue(string SettingName)
        {
            string query = $"SELECT * FROM [{TabellaSettings}] WHERE [SettingName] = \"{SettingName}\"";
            var lista = database.Query<Settings>(query);
            return lista.FirstOrDefault();
        }

        public static int UpdateSettings(Settings dati)
        {
            int i = database.Update(dati);
            return i;
        }

        public static int delSettings()
        {
            return database.DeleteAll<Settings>();
        }
        #endregion

        #region DayWorked functions
        public static DaysWorked getDay(string YearDay, string Year)
        {
            string query = $"SELECT * FROM [{TabellaDays}] WHERE [Year] = \"{Year}\" AND [YearDay] = \"{YearDay}\"";
            var lista = database.Query<DaysWorked>(query);
            return lista.FirstOrDefault();
        }

        public static DaysWorked getDayFromId(string IdDaysWorked)
        {
            string query = $"SELECT * FROM [{TabellaDays}] WHERE [IdDaysWorked] = \"{IdDaysWorked}\"";
            var lista = database.Query<DaysWorked>(query);
            return lista.FirstOrDefault();
        }

        public static List<DaysWorked> getAllDays()
        {

            string query = $"SELECT * FROM [{TabellaDays}]";
            var lista = database.Query<DaysWorked>(query);
            return lista;
        }

        public static int InsertDay(DaysWorked dati)
        {
            return database.Insert(dati);
        }

        public static int delDays()
        {
            return database.DeleteAll<DaysWorked>();
        }
        #endregion

        #region  DaysWorkedHours functions

        public static int delDaysHours()
        {
            return database.DeleteAll<DaysWorkedHours>();
        }

        public static void delDayHours(int IdDaysWorked)
        {
            string query = $"DELETE FROM [{TabellaDaysHours}] WHERE [IdDaysWorked] = \"{IdDaysWorked}\" ";
            var lista = database.Query<DaysWorkedHours>(query);
        }

        public static List<DaysWorkedHours> getAllHours()
        {
            string query = $"SELECT * FROM [{TabellaDaysHours}]";
            var lista = database.Query<DaysWorkedHours>(query);
            return lista;
        }

        public static List<DaysWorkedHours> getDayHours(string IdDaysWorked)
        {
            string query = $"SELECT * FROM [{TabellaDaysHours}] WHERE [IdDaysWorked] = \"{IdDaysWorked}\" ";
            var lista = database.Query<DaysWorkedHours>(query);
            return lista;
        }

        public static List<DaysWorkedHours> getClosedDayHours(string IdDaysWorked)
        {
            string query = $"SELECT * FROM [{TabellaDaysHours}] WHERE [IdDaysWorked] = \"{IdDaysWorked}\" AND [CheckIn] <> \"\" AND  [CheckOut] <> \"\"";
            var lista = database.Query<DaysWorkedHours>(query);
            return lista;
        }

        public static List<DaysWorkedHours> getOpenedDayHours(string IdDaysWorked)
        {
            string query = $"SELECT * FROM [{TabellaDaysHours}] WHERE [IdDaysWorked] = \"{IdDaysWorked}\"  AND [CheckIn] <> \"\" AND [CheckOut] = \"\" ";
            var lista = database.Query<DaysWorkedHours>(query);
            return lista;
        }

        public static int InsertDayHours(DaysWorkedHours dati)
        {
            return database.Insert(dati);
        }

        public static int UpdateDayHours(DaysWorkedHours dati)
        {
            int i = database.Update(dati);
            return i;
        }

        public static DaysWorkedHours lastDaysWorkedHours()
        {
            string query = $"SELECT * FROM [{TabellaDaysHours}] ORDER BY IdDaysWorkedHours DESC";
            var lista = database.Query<DaysWorkedHours>(query);
            return lista.FirstOrDefault();
        }
        #endregion
    }




}
