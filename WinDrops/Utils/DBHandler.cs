using SQLite;
using SQLiteNetExtensions.Extensions;
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

        public static List<DropConfig> GetDropConfigs()
        {
            return _DB.Query<DropConfig>("Select * From DropConfig");
        }

        public static int AddDropConfig(DropConfig dropConfig = null)
        {
            if (dropConfig == null)
            { dropConfig = new DropConfig(); }
            _DB.Insert(dropConfig);
            return 0;
        }

        public static int DeleteDropConfig(DropConfig dropConfig)
        {
            if (dropConfig == null)
            { return 1; }

            _DB.Delete(dropConfig, recursive:true);
            return 0;
        }

    }
}
