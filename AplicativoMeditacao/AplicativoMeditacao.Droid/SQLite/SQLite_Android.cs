using SQLite;
using System.IO;
using Xamarin.Forms;
using AplicativoMeditacao.Droid.SQLite;
using AplicativoMeditacao.Interfaces;

[assembly: Dependency(typeof(SQLite_Android))]
namespace AplicativoMeditacao.Droid.SQLite
{
    class SQLite_Android : ISqlite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "AppMeditacao.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            var conn = new SQLiteConnection(path);
            return conn;
        }
    }
}