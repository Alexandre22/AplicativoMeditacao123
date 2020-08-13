
using AplicativoMeditacao.Data;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.Model;
using SQLite;
using Xamarin.Forms;

namespace AplicativoMeditacao.Util
{
    public class UserMessageManager : IRespostaWeb
    {
        IUserMessage pagina;

        public UserMessageManager(IUserMessage page)
        {
            pagina = page;
        }

        public void GetUserMessage(string link)
        {
            Util.LeitorConteudoDeSite.retornaConteudoSite(link, this);
        }

        public void respostaWeb(string texto)
        {
            if (!texto.Equals("Erro!"))
            {
                var message_id = Util.LeitorConteudoDeSite.ExtrairVersaoDoTexto(texto);
                if (!message_id.Equals("0"))
                {
                    var mensagemDoSite = texto.Replace("{versão " + message_id + "}", "");
                    SQLiteConnection conexao = DependencyService.Get<ISqlite>().GetConnection();
                    MensagemAoUsuarioDAO dao = new MensagemAoUsuarioDAO(conexao);

                    var mensagem = dao.Mensagens;

                    if(mensagem.Count == 0)
                    {
                        MensagemAoUsuario msg = new MensagemAoUsuario() { Mensagem = mensagemDoSite };
                        dao.AdicionarMensagem(msg);
                        pagina.GetUserMessage(mensagemDoSite);
                    }
                    else
                    {
                        if (!mensagem[0].Mensagem.Equals(mensagemDoSite))
                        {
                            var novaMsg = mensagem[0];
                            novaMsg.Mensagem = mensagemDoSite;
                            dao.AtualizarMensagem(novaMsg);
                            pagina.GetUserMessage(mensagemDoSite);
                        }
                    }
                }
            }
        }


    }
}
