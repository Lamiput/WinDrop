using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinDrops.Model;


namespace WinDrops.Utils
{
    static class DBHandler
    {
        private static SQLiteConnection _DB;

        public static void OpenConnection()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WinDropsDB.db");
            _DB = new SQLiteConnection(databasePath);

            _DB.CreateTable<DropConfig>();
            _DB.CreateTable<Device>();
            _DB.CreateTable<DropTask>();
        }

        public static void CloseConnection()
        {
            if (_DB != null)
                _DB.Close();
        }

    }
}
