using SQLite;

namespace AplicativoMeditacao.Interfaces
{
    public interface ISqlite
    {
        SQLiteConnection GetConnection();
    }
}
