using AplicativoMeditacao.Data;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.Model;
using AplicativoMeditacao.Util;
using AplicativoMeditacao.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AplicativoMeditacao.ViewModel
{
    class TelaInicialViewModel : INotifyPropertyChanged, IRespostaWeb, ICurrentAppVersion, IUserMessage
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        GerenciadorNotificacoes gn;
        TelaEspacoPresenca tep;
        Orientacoes orientacoes;
        AntesMeditacao am;
        SalvaNotificacao sn;
        Creditos cred;
        SQLiteConnection conexao;
        private bool flag;

        //variaveis da pagina orientaçoes
        public FormattedString TituloTexto { get; set; }
        private ConteudoDaPagina Conteudo { get; set; }
        ConteudoDaPaginaDAO conteudoDaPaginaDAO;
        public ObservableCollection<PaginaOrientacao> Topico { get; set; }
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
        //fim das variaveis da pagina de orientaçoes

        public string mensagemAtualizacaoUm;
        public string MensagemAtualizacaoUm
        {
            get
            {
                return mensagemAtualizacaoUm;
            }
            set
            {
                if(value != mensagemAtualizacaoUm)
                {
                    mensagemAtualizacaoUm = value;
                    OnPropertyChanged("MensagemAtualizacaoUm");
                }
            }
        }

        public string mensagemAtualizacaoDois;
        public string MensagemAtualizacaoDois
        {
            get
            {
                return mensagemAtualizacaoDois;
            }
            set
            {
                if (value != mensagemAtualizacaoDois)
                {
                    mensagemAtualizacaoDois = value;
                    OnPropertyChanged("MensagemAtualizacaoDois");
                }
            }
        }

        public INavigation Navigation { get; set; }

        private TelaInicial telaInicial;

        public TelaInicialViewModel(TelaInicial ti)
        {
            telaInicial = ti;
            flag = true;
            MensagemAtualizacaoUm = "";
            MensagemAtualizacaoDois = "";
            //site antigo: http://espacopresenca.com.br/?page_id=2986
            new VerificadorDeVersao(this).GetCurrentVersion("https://espacopresenca.com.br/a-atualizacoes");
            //site antigo: http://espacopresenca.com.br/?page_id=2988
            new UserMessageManager(this).GetUserMessage("http://espacopresenca.com.br/a-comunicacoes");
            conexao = DependencyService.Get<ISqlite>().GetConnection();
        }
        
        public void GetUserMessage(string message)
        {
            int posicaoFinalMensagem = message.IndexOf('{');
            while (!(message.Substring(posicaoFinalMensagem, 5).Equals("{fim}")))
            {
                posicaoFinalMensagem++;
            }
            var mensagem_usuario = message.Substring(0, posicaoFinalMensagem);
            
            mensagem_usuario = mensagem_usuario.Replace("</p>", "\n\n");
            mensagem_usuario = mensagem_usuario.Replace("<br />","\n\n");

            var char1 = mensagem_usuario[mensagem_usuario.Length - 1];

            if (char1.Equals('\n'))
            {
                mensagem_usuario = mensagem_usuario.Substring(0, mensagem_usuario.Length - 1);
            }

            telaInicial.DisplayAlert("",mensagem_usuario, "Ok");
        }

        public void GetCurrentAppVersion(string versao)
        {
            if (!versao.Equals("Erro!"))
            {
                var appVersion = DependencyService.Get<IInstalledAppVersion>().GetAppVersion();
                if (!versao.Equals(appVersion))
                {
                    MensagemAtualizacaoUm = "Atualização disponível.";
                    MensagemAtualizacaoDois = "Clique aqui para atualizar.";
                }
            }
        }

        public ICommand AbrirAppStore
        {
            get
            {
                return new Command(() =>
                {
                    DependencyService.Get<IOpenAppStore>().openAppStore();
                });
            }
        }

        public ICommand AbreOrientacoes
        {
            get
            {
                return new Command(() =>
                {
                    if (flag)
                    {
                        flag = false;
                        conteudoDaPaginaDAO = new ConteudoDaPaginaDAO(conexao);
                        Topico = new ObservableCollection<PaginaOrientacao>();

                        TituloTexto = new FormattedString();
                        TituloTexto.Spans.Add(new Span { Text = "ORIENTAÇÕES", FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) });

                        CarregamentoDoTexto = "Aguarde o texto ser carregado.";

                        //site antigo: http://espacopresenca.com.br/?page_id=2523
                        AbrirConteudoDaPagina("Orientacoes", "https://espacopresenca.com.br/a-orientacoes");
                    }
                });
            }
        }

        public void AbrirConteudoDaPagina(string titulo, string link)
        {
            Conteudo = conteudoDaPaginaDAO.Conteudos.Where(X => X.Titulo.Equals(titulo)).FirstOrDefault();

            if (Conteudo == null)
            {
                Conteudo = new ConteudoDaPagina()
                {
                    Titulo = titulo,
                    Conteudo = "Por favor, conecte-se à internet e em seguida abra esse texto novamente. Esse procedimento será necessário apenas uma vez.<>{fim}<>",
                    Versao = "0"
                };
                conteudoDaPaginaDAO.SalvarConteudo(Conteudo);
            }

            bool estaOnline = DependencyService.Get<INetworkConnectivity>().HasConnectivity();

            if (estaOnline)
            {
                Util.LeitorConteudoDeSite.retornaConteudoSite(link, this);
            }
            else
            {
                respostaWeb("Erro!");
            }
        }

        public void respostaWeb(string texto)
        {
            if (texto.Equals("Erro!")) //deu erro, abrir a pagina do jeito como esta
            {
                CriarPaginasDoTopico();
            }
            else //nao deu erro, verificar versao, atualizar se necessario e abrir a pagina
            {
                string versaoDoSite = Util.LeitorConteudoDeSite.ExtrairVersaoDoTexto(texto);
                texto = texto.Replace("{versão " + versaoDoSite + "}", "");

                if (versaoDoSite.Equals(Conteudo.Versao))
                {
                    CriarPaginasDoTopico();
                }
                else
                {
                    Conteudo.Versao = versaoDoSite;
                    Conteudo.Conteudo = texto;
                    conteudoDaPaginaDAO.AtualizarConteudo(Conteudo);
                    CriarPaginasDoTopico();
                }

            }
        }

        public void CriarPaginasDoTopico()
        {
            Topico.Clear();

            int pagina = 0;
            string texto = Conteudo.Conteudo;

            int posicaoFinalDaLinha;
            string linha = "";

            while (linha != "{fim}")
            {
                List<string> linhasDaPagina = new List<string>();

                while (linha != "{fim}" && linha != "[pula]")
                {
                    if (!linha.Equals(""))
                    {
                        linhasDaPagina.Add(linha);
                        linhasDaPagina.Add("");
                    }

                    posicaoFinalDaLinha = texto.IndexOf('<');
                    linha = texto.Substring(0, posicaoFinalDaLinha);

                    posicaoFinalDaLinha = texto.IndexOf('>');
                    texto = texto.Substring(posicaoFinalDaLinha + 1);
                }

                PaginaOrientacao paginaOrientacao = new PaginaOrientacao(TituloTexto, linhasDaPagina, pagina);

                Topico.Add(paginaOrientacao);

                if (linha.Equals("[pula]"))
                {
                    linha = "";
                }

                pagina++;
            }

            AbrirTopicoOrientacao();
        }

        public async void AbrirTopicoOrientacao()
        {
            orientacoes = new Orientacoes(Topico[0], Topico);
            await Navigation.PushAsync(orientacoes);
            flag = true;
            CarregamentoDoTexto = "";
        }


        public ICommand AbreMeditacao
        {
            get
            {
                return new Command(async () =>
                {
                    if (flag)
                    {
                        flag = false;

                        MeditacaoDAO meditacaoDAO = new MeditacaoDAO(conexao);
                        am = new AntesMeditacao(meditacaoDAO);
                        await Navigation.PushAsync(am);

                        flag = true;
                    }
                });
            }
        }

        public ICommand AbreGerenciadorNotificacoes
        {
            get
            {
                return new Command(async () =>
                {
                    if (flag)
                    {
                        flag = false;
                        INotifier notifier = DependencyService.Get<INotifier>();

                        NotificacaoDAO notificacaoDAO = new NotificacaoDAO(conexao, notifier);

                        if (notificacaoDAO.Notificacoes.Count != 0)
                        {
                            gn = new GerenciadorNotificacoes(notificacaoDAO);
                            await Navigation.PushAsync(gn);
                        }
                        else
                        {
                            sn = new SalvaNotificacao(notificacaoDAO);
                            await Navigation.PushAsync(sn);
                        }
                        flag = true;
                    }
                });
            }
        }

        public ICommand AbreEspacoPresenca
        {
            get
            {
                return new Command(async () =>
               {
                   if (flag)
                   {
                       flag = false;
                       ConteudoDaPaginaDAO conteudoDAO = new ConteudoDaPaginaDAO(conexao);

                       tep = new TelaEspacoPresenca(conteudoDAO);
                       await Navigation.PushAsync(tep);
                       flag = true;
                   }
               });
            }
        }

        public ICommand AbreCreditos
        {
            get
            {
                return new Command(async () =>
                {
                    if (flag)
                    {
                        flag = false;
                        ConteudoDaPaginaDAO conteudoDAO = new ConteudoDaPaginaDAO(conexao);

                        cred = new Creditos(conteudoDAO);
                        await Navigation.PushAsync(cred);
                        flag = true;
                    }
                });
            }
        }

    }
}
