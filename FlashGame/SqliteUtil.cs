using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace FlashGame
{
    public class SqliteUtil
    {
        static string connString = string.Format(@"data source={0}\data\sl3", Environment.CurrentDirectory);

        static SQLiteConnection conn;


        static SqliteUtil()
        {
            conn = new SQLiteConnection(connString);
        }

        public static IQueryable<GameInfo> GetAllGames()
        {
            return null;
        }
    }
}
