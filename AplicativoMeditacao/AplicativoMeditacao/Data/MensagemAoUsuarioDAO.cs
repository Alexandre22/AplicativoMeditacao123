using AplicativoMeditacao.Model;
using SQLite;
using System.Collections.ObjectModel;

namespace AplicativoMeditacao.Data
{
    public class MensagemAoUsuarioDAO
    {
        private SQLiteConnection conexao;

        private ObservableCollection<MensagemAoUsuario> mensagens;
        public ObservableCollection<MensagemAoUsuario> Mensagens
        {
            get
            {
                return GetAll();
            }
            protected set { mensagens = value; }
        }

        public MensagemAoUsuarioDAO(SQLiteConnection con)
        {
            conexao = con;
            conexao.CreateTable<MensagemAoUsuario>();
            mensagens = GetAll();
        }

        public void AdicionarMensagem(MensagemAoUsuario msg)
        {
            conexao.Insert(msg);
            mensagens.Add(msg);
        }

        public void AtualizarMensagem(MensagemAoUsuario msg)
        {
            conexao.Update(msg);
        }

        private ObservableCollection<MensagemAoUsuario> GetAll()
        {
            return new ObservableCollection<MensagemAoUsuario>(conexao.Table<MensagemAoUsuario>());
        }

    }
}
