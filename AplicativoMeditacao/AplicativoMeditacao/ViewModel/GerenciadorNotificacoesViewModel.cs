using AplicativoMeditacao.Data;
using AplicativoMeditacao.Model;
using AplicativoMeditacao.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;

namespace AplicativoMeditacao.ViewModel
{
    class GerenciadorNotificacoesViewModel : INotifyPropertyChanged
    {
        SalvaNotificacao sn;
        public ObservableCollection<Notificacao> Notificacoes { get; set; }
        private NotificacaoDAO dao;
        public ICommand AdicionarNotificacao { get; protected set; }
        public INavigation Navigation { get; set; }

        private bool flag;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nome)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(nome));
        }

        private Notificacao notSelecionada;
        public Notificacao NotSelecionada
        {
            get
            {
                return notSelecionada;
            }
            set
            {
                if(notSelecionada != value)
                {
                    notSelecionada = value;
                    AcaoNotificacao(notSelecionada);
                    notSelecionada = null;
                    OnPropertyChanged("NotSelecionada");
                }
            }
        }

        private ContentPage page;
        
        public GerenciadorNotificacoesViewModel(NotificacaoDAO notDao, ContentPage pag)
        {
            flag = true;
            dao = notDao;
            this.page = pag;
            Notificacoes = dao.Notificacoes;
            
            AdicionarNotificacao = new Command(async () =>
            {
                if (flag)
                {
                    flag = false;
                    sn = new SalvaNotificacao(dao);
                    await Navigation.PushAsync(sn);
                    flag = true;
                }
            });
        }

        private RelayCommand<object> _MostrarEsconderNotificacao;

        public RelayCommand<object> MostrarEsconderNotificacao
        {
            get
            {
                return _MostrarEsconderNotificacao ?? (_MostrarEsconderNotificacao = new RelayCommand<object>((currentObject) => dao.MostrarEsconder(currentObject as Notificacao)));
            }
        }

        private RelayCommand<object> apagarNoificacao;

        public RelayCommand<object> ApagarNotificacao
        {
            get
            {
                return apagarNoificacao ?? (apagarNoificacao = new RelayCommand<object>((currentObject) => ApagarNotif(currentObject as Notificacao)));
            }
        }
        
        public async void ApagarNotif(Notificacao notificacao)
        {
            var resposta = await page.DisplayAlert("Remover Notificação", "Deseja realmente remover essa notificação?", "Sim", "Não");
            if (resposta)
            {
                dao.RemoverNotificacao(notificacao);

                if (Notificacoes.Count == 0)
                {
                    Navigation.PopAsync();
                }
            }
        }
        
        private RelayCommand<object> ativarDesativarNotificacao;

        public RelayCommand<object> AtivarDesativarNotificacao
        {
            get
            {
                return ativarDesativarNotificacao ?? (ativarDesativarNotificacao = new RelayCommand<object>((currentObject) => AtivarDesativarNot(currentObject as Notificacao)));
            }
        }

        public void AtivarDesativarNot(Notificacao not)
        {
            if (not.Ativado)
            {
                dao.DesativarNotificacao(not);
            } else
            {
                dao.AtivarNotificacao(not);
            }
        }

        private RelayCommand<object> ativarDesativarVibracao;

        public RelayCommand<object> AtivarDesativarVibracao
        {
            get
            {
                return ativarDesativarVibracao ?? (ativarDesativarVibracao = new RelayCommand<object>((currentObject) => AtivarDesativarVibracaoNot(currentObject as Notificacao)));
            }
        }

        public void AtivarDesativarVibracaoNot(Notificacao not)
        {
            if (not.Vibrar)
            {
                dao.DesativarVibracao(not);
            }
            else
            {
                dao.AtivarVibracao(not);
            }
        }

        private RelayCommand<object> ativarDesativarSom;

        public RelayCommand<object> AtivarDesativarSom
        {
            get
            {
                return ativarDesativarSom ?? (ativarDesativarSom = new RelayCommand<object>((currentObject) => AtivarDesativarSomNot(currentObject as Notificacao)));
            }
        }

        public void AtivarDesativarSomNot(Notificacao not)
        {
            if (not.Som)
            {
                dao.DesativarSom(not);
            }
            else
            {
                dao.AtivarSom(not);
            }
        }

        public async void AcaoNotificacao(Notificacao notificacao)
        {
            if (flag)
            {
                flag = false;
                sn = new SalvaNotificacao(dao, notificacao);
                await Navigation.PushAsync(sn);
                flag = true;
            }
        }

    }
}
