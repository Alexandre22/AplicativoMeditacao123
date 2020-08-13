using AplicativoMeditacao.Data;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.Model;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AplicativoMeditacao.ViewModel
{
    public class MeditacaoUMDPViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private int numeroRepeticoes;

        public int NumeroRepeticoes
        {
            get
            {
                return numeroRepeticoes;
            }
            set
            {
                if(numeroRepeticoes != value)
                {
                    numeroRepeticoes = value;
                }
            }
        }

        private int numeroRepeticoesHumming;

        public int NumeroRepeticoesHumming
        {
            get
            {
                return numeroRepeticoesHumming;
            }
            set
            {
                if (numeroRepeticoesHumming != value)
                {
                    numeroRepeticoesHumming = value;
                }
            }
        }

        private string nomeBotao;
        public string NomeBotao
        {
            get
            {
                return nomeBotao;
            }
            set
            {
                if(nomeBotao != value)
                {
                    nomeBotao = value;
                    OnPropertyChanged("NomeBotao");
                }
            }
        }

        private TimeSpan tempoTotal;
        public TimeSpan TempoTotal
        {
            get
            {
                return tempoTotal;
            }
            set
            {
                if(tempoTotal != value)
                {
                    tempoTotal = value;
                    OnPropertyChanged("TempoTotal");
                }
            }
        }

        private bool fimMeditacao;
        public bool FinalizarMeditacao
        {
            get
            {
                return fimMeditacao;
            }
            set
            {
                if(fimMeditacao != value)
                {
                    fimMeditacao = value;
                    finalizarMeditacao();
                }
            }
        }

        private string tempoFinal;
        public string TempoFinal
        {
            get
            {
                return tempoFinal;
            }
            set
            {
                if(tempoFinal != value)
                {
                    tempoFinal = value;
                    OnPropertyChanged("TempoFinal");
                }
            }
        }

        private string nomeDaMeditacao;
        public string NomeDaMeditacao
        {
            get
            {
                return nomeDaMeditacao;
            }
            set
            {
                if (nomeDaMeditacao != value)
                {
                    nomeDaMeditacao = value;
                    OnPropertyChanged("NomeDaMeditacao");
                }
            }
        }

        public ICommand ReiniciaMeditacao { get; protected set; }

        public ICommand PausaIniciaMeditacao { get; protected set; }

        public ICommand AcrescentarUmMinuto { get; protected set; }

        public INavigation Navigation { get; set; }

        ContentPage page;

        private MeditacaoDAO dao;

        public int tempoMaxHumming;
        public int tempoMaxUMDP;

        public string AudioHumming { get; protected set; }
        public string AudioUMP { get; protected set; }

        public MeditacaoUMDPViewModel(string audioHumming, string audioUMP, int repeticoes, int repeticoesHumming, ContentPage pag, MeditacaoDAO medDao)
        {
            AudioHumming = audioHumming;
            AudioUMP = audioUMP;

            NumeroRepeticoes = repeticoes;

            NumeroRepeticoesHumming = repeticoesHumming;
            
            this.page = pag;

            dao = medDao;

            atribuirTemposMaximos();

            FinalizarMeditacao = false;
            
            TempoTotal = new TimeSpan(0, 0, 0);

            NomeDaMeditacao = "";

            comecarMeditacao();

            TempoFinal = "";
            
            ReiniciaMeditacao = new Command(() =>
            {
                reiniciarMeditacao();
            });

            PausaIniciaMeditacao = new Command(() =>
            {
                if (NomeBotao.Equals("II"))
                {
                    //DependencyService.Get<ITimer>().pauseTimer();
                    DependencyService.Get<IAudio>().pausarMeditacao();
                    NomeBotao = ">";
                } else
                {
                    //DependencyService.Get<ITimer>().resumeTimer();
                    DependencyService.Get<IAudio>().resumirMeditacao();
                    NomeBotao = "II";
                }
            });

            AcrescentarUmMinuto = new Command(() =>
            {
                acrescentarMinuto();
            });
        }
        
        public void atribuirTemposMaximos()
        {
            tempoMaxHumming = dao.TemposMeditacao[0].TempoMaximoHumming;
            tempoMaxUMDP = dao.TemposMeditacao[0].TempoMaximoUMDP;
        }

        public void comecarMeditacao()
        {
            //DependencyService.Get<ITimer>().startServiceTimer(this);
            DependencyService.Get<IAudio>().tocarSomMeditacao(this);
            NomeBotao = "II";
        }

        public void reiniciarMeditacao()
        {
            //DependencyService.Get<ITimer>().resetTimer();
            DependencyService.Get<IAudio>().reiniciaMeditacao();
            NomeBotao = "II";
        }

        public void pararMeditacao()
        {
            //DependencyService.Get<ITimer>().stopTimer();
            DependencyService.Get<IAudio>().pararMeditacao();
        }

        public async void finalizarMeditacao()
        {
            if (FinalizarMeditacao)
            {
                //acender tela
                DependencyService.Get<IWindowController>().acenderTela();

                var qualidadeDePresenca = await page.DisplayAlert("", "Houve Qualidade de Presença em sua prática?", "Sim", "Não");

                if (qualidadeDePresenca)
                {
                    Meditacao meditacao = dao.TemposMeditacao[0];
                    meditacao.TempoTotalMeditado += NumeroRepeticoes + numeroRepeticoesHumming;
                    dao.AtualizarMeditacao(meditacao);
                }

                bool aumentar_tempo_humming = NumeroRepeticoesHumming == tempoMaxHumming && NumeroRepeticoesHumming < 30;

                bool aumentar_tempo_ump = NumeroRepeticoes == tempoMaxUMDP;
                
                if (aumentar_tempo_humming || aumentar_tempo_ump)
                {
                    DependencyService.Get<ICustomDialog>().callCustomDialog(aumentar_tempo_humming, aumentar_tempo_ump, this);
                }
                else
                {
                    var resposta = await page.DisplayAlert("","Deseja praticar novamente?", "Sim", "Não");

                    if (resposta)
                    {
                        FinalizarMeditacao = false;
                        comecarMeditacao();
                    }
                    else
                    {
                        await Navigation.PopAsync();
                    }
                    //apagar tela
                    DependencyService.Get<IWindowController>().apagarTela();
                }
            }
        }

        public void voltarPagina()
        {
            Navigation.PopAsync();
        }
        
        public void acrescentarMinuto()
        {
            //DependencyService.Get<ITimer>().increaseTimer();
            DependencyService.Get<IAudio>().acrescentarMinuto();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
