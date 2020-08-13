using AplicativoMeditacao.Model;
using SQLite;
using System.Collections.ObjectModel;

namespace AplicativoMeditacao.Data
{
    public class MeditacaoDAO
    {
        private SQLiteConnection conexao;
        private ObservableCollection<Meditacao> tempos;
        public ObservableCollection<Meditacao> TemposMeditacao
        {
            get
            {
                return GetAll();
            }
            protected set { tempos = value; }
        }

        public MeditacaoDAO(SQLiteConnection con)
        {
            conexao = con;
            conexao.CreateTable<Meditacao>();
            tempos = GetAll();
        }

        public void AdicionarMeditacao(Meditacao med)
        {
            conexao.Insert(med);
            tempos.Add(med);
        }

        public void AtualizarMeditacao(Meditacao med)
        {
            conexao.Update(med);
        }

        private ObservableCollection<Meditacao> GetAll()
        {
            return new ObservableCollection<Meditacao>(conexao.Table<Meditacao>());
        }

    }
}
