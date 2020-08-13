using AplicativoMeditacao.Data;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace AplicativoMeditacao.ViewModel
{
    public class CreditosViewModel : INotifyPropertyChanged, IRespostaWeb
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string carregamentoDoTexto;

        public string CarregamentoDoTexto
        {
            get
            {
                return carregamentoDoTexto;
            }
            set
            {
                if (value != carregamentoDoTexto)
                {
                    carregamentoDoTexto = value;
                    OnPropertyChanged("CarregamentoDoTexto");
                }
            }
        }

        public StackLayout layoutConteudo;
        private ConteudoDaPagina Conteudo { get; set; }
        private ConteudoDaPaginaDAO dao;

        public List<string> TextoCreditos { get; set; }

        public CreditosViewModel(StackLayout layout, ConteudoDaPaginaDAO cdpDao)
        {
            dao = cdpDao;
            layoutConteudo = layout;

            AbrirConteudoDaPagina();
        }

        public void AbrirConteudoDaPagina()
        {
            Conteudo = dao.Conteudos.Where(X => X.Titulo.Equals("Creditos")).FirstOrDefault();

            if (Conteudo == null)
            {
                Conteudo = new ConteudoDaPagina()
                {
                    Titulo = "Creditos",
                    Conteudo = "Por favor, conecte-se à internet e em seguida abra esse texto novamente. Esse procedimento será necessário apenas uma vez.<>{fim}<>",
                    Versao = "0"
                };
                dao.SalvarConteudo(Conteudo);
            }

            bool estaOnline = DependencyService.Get<INetworkConnectivity>().HasConnectivity();

            CarregamentoDoTexto = "Aguarde, o texto esta sendo carregado.";

            if (estaOnline)
            {
                //link antigo: http://espacopresenca.com.br/?page_id=2536
                Util.LeitorConteudoDeSite.retornaConteudoSite("https://espacopresenca.com.br/a-creditos", this);
            }
            else
            {
                respostaWeb("Erro!");
            }
        }

        public void respostaWeb(string texto)
        {
            if (texto.Equals("Erro!"))
            {
                CriarLinhasDoTexto();
            }
            else
            {
                string versaoDoSite = Util.LeitorConteudoDeSite.ExtrairVersaoDoTexto(texto);
                texto = texto.Replace("{versão " + versaoDoSite + "}", "");

                if (versaoDoSite.Equals(Conteudo.Versao))
                {
                    CriarLinhasDoTexto();
                }
                else
                {
                    Conteudo.Versao = versaoDoSite;
                    Conteudo.Conteudo = texto;
                    dao.AtualizarConteudo(Conteudo);
                    CriarLinhasDoTexto();
                }
            }
        }

        private void CriarLinhasDoTexto()
        {
            List<string> linhasDaPagina = new List<string>();
            string linha = "";
            int posicaoFinalDaLinha;
            string texto = Conteudo.Conteudo;

            while (!linha.Equals("{fim}"))
            {
                posicaoFinalDaLinha = texto.IndexOf('<');
                linha = texto.Substring(0, posicaoFinalDaLinha);
                posicaoFinalDaLinha = texto.IndexOf('>');
                texto = texto.Substring(posicaoFinalDaLinha + 1);
                if (!linha.Equals("{fim}"))
                {
                    linhasDaPagina.Add(linha);
                }
            }

            TextoCreditos = linhasDaPagina;

            CriarConteudo();
        }

        public void CriarConteudo()
        {
            CarregamentoDoTexto = "";
            Util.GerenciadorDeConteudoDePagina.criarConteudo(layoutConteudo, TextoCreditos);
        }

    }
}
