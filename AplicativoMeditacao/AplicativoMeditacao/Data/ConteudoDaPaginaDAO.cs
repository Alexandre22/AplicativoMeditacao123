using AplicativoMeditacao.Model;
using SQLite;
using System.Collections.ObjectModel;

namespace AplicativoMeditacao.Data
{
    public class ConteudoDaPaginaDAO
    {
        private SQLiteConnection conexao;
        private ObservableCollection<ConteudoDaPagina> lista;
        public ObservableCollection<ConteudoDaPagina> Conteudos
        {
            get
            {
                return lista;
            }
            protected set { lista = value; }
        }

        public ConteudoDaPaginaDAO(SQLiteConnection con)
        {
            conexao = con;
            conexao.CreateTable<ConteudoDaPagina>();
            lista = GetAll();
        }

        public void SalvarConteudo(ConteudoDaPagina cdp)
        {
            conexao.Insert(cdp);
            lista.Add(cdp);
        }

        public void AtualizarConteudo(ConteudoDaPagina cdp)
        {
            conexao.Update(cdp);
        }

        private ObservableCollection<ConteudoDaPagina> GetAll()
        {
            return new ObservableCollection<ConteudoDaPagina>(conexao.Table<ConteudoDaPagina>());
        }

    }
}
