using AplicativoMeditacao.Data;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.Model;
using AplicativoMeditacao.Util;
using AplicativoMeditacao.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace AplicativoMeditacao.ViewModel
{
    public class AntesMeditacaoViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string tempoTotalMeditado;
        public string TempoTotalMeditado
        {
            get
            {
                return tempoTotalMeditado;
            }
            set
            {
                if(tempoTotalMeditado != value)
                {
                    tempoTotalMeditado = value;
                    OnPropertyChanged("TempoTotalMeditado");
                }
            }
        }

        private double valor;
        public double Valor
        {
            get
            {
                return valor;
            }
            set
            {
                if(valor != value)
                {
                    valor = Math.Round(value);
                    OnPropertyChanged("Valor");
                }
            }
        }

        private bool flag;

        public int tempoMaximoHumming;
        public int TempoMaximoHumming
        {
            get
            {
                return tempoMaximoHumming;
            }
            set
            {
                if (tempoMaximoHumming != value)
                {
                    tempoMaximoHumming = value;
                    OnPropertyChanged("TempoMaximoHumming");
                }
            }
        }

        public int tempoMaximoUMDP;
        public int TempoMaximoUMDP
        {
            get
            {
                return tempoMaximoUMDP;
            }
            set
            {
                if(tempoMaximoUMDP != value)
                {
                    tempoMaximoUMDP = value;
                    OnPropertyChanged("TempoMaximoUMDP");
                }
            }
        }
        
        public int TempoMinimoUMDP { get; set; }

        private double valorHumming;
        public double ValorHumming
        {
            get
            {
                return valorHumming;
            }
            set
            {
                if (valorHumming != value)
                {
                    valorHumming = Math.Round(value);
                    OnPropertyChanged("ValorHumming");
                }
            }
        }
        
        public string audioHumming;
        public string AudioHumming {
            get
            {
                return audioHumming;
            }
            set
            {
                if(value != audioHumming)
                {
                    audioHumming = value;
                    OnPropertyChanged("AudioHumming");
                }
            }
        }

        public string audioUMP;
        public string AudioUMP
        {
            get
            {
                return audioUMP;
            }
            set
            {
                if(value != audioUMP)
                {
                    audioUMP = value;
                    OnPropertyChanged("AudioUMP");
                }
            }
        }

        public ICommand ComecaMeditacaoUMDP { get; protected set; }
        public INavigation Navigation { get; set; }

        public ICommand AbrirListaDeSonsHumming { get; protected set; }
        public ICommand AbrirListaDeSonsUMP { get; protected set; }

        public ICommand DiminuirTempoDeHumming { get; protected set; }
        public ICommand AumentarTempoDeHumming { get; protected set; }

        public ICommand DiminuirTempoDeUMP { get; protected set; }
        public ICommand AumentarTempoDeUMP { get; protected set; }

        MeditacaoUMDP umdp;

        private MeditacaoDAO dao;
        
        public AntesMeditacaoViewModel(MeditacaoDAO medDao)
        {
            Valor = 1;
            ValorHumming = 0;
            flag = true;

            dao = medDao;
            
            ajustaLimitesMaximosDeTempos();
            TempoMinimoUMDP = 1;

            //AudioHumming = dao.TemposMeditacao[0].AudioHumming;

            AudioHumming = verificarAudioExiste(dao.TemposMeditacao[0].AudioHumming, ListasDeSonsMeditacao.listaDeSonsHumming);

            //AudioUMP = dao.TemposMeditacao[0].AudioUMP;

            AudioUMP = verificarAudioExiste(dao.TemposMeditacao[0].AudioUMP, ListasDeSonsMeditacao.listaDeSonsUMP);

            ajustarTempoTotalMeditado();

            ComecaMeditacaoUMDP = new Command(async () =>
            {
                if (flag)
                {
                    flag = false;
                    var repeticoes = (int)valor;
                    var repeticoesHumming = (int)valorHumming;
                    umdp = new MeditacaoUMDP(ListasDeSonsMeditacao.listaDeSonsHumming[AudioHumming], ListasDeSonsMeditacao.listaDeSonsUMP[AudioUMP], repeticoes, repeticoesHumming, dao);
                    await Navigation.PushAsync(umdp);
                    flag = true;
                }
                
            });

            AbrirListaDeSonsHumming = new Command(async () =>
            {
                var resposta = await DependencyService.Get<ICustomListDialog>().showCustomListDialog(ListasDeSonsMeditacao.listaDeSonsHumming, AudioHumming);
                AudioHumming = resposta;
                var meditacao = dao.TemposMeditacao[0];
                meditacao.AudioHumming = AudioHumming;
                dao.AtualizarMeditacao(meditacao);
            });

            AbrirListaDeSonsUMP = new Command(async () =>
            {
                var resposta = await DependencyService.Get<ICustomListDialog>().showCustomListDialog(ListasDeSonsMeditacao.listaDeSonsUMP, AudioUMP);
                AudioUMP = resposta;
                var meditacao = dao.TemposMeditacao[0];
                meditacao.AudioUMP = AudioUMP;
                dao.AtualizarMeditacao(meditacao);
            });

            DiminuirTempoDeHumming = new Command(() =>
            {
                if(ValorHumming > 0)
                {
                    ValorHumming--;
                }
                
            });

            AumentarTempoDeHumming = new Command(() =>
            {
                if(ValorHumming < TempoMaximoHumming)
                {
                    ValorHumming++;
                }
            });

            DiminuirTempoDeUMP = new Command(() =>
            {
                if (Valor > 1)
                {
                    Valor--;
                }

            });

            AumentarTempoDeUMP = new Command(() =>
            {
                if (Valor < TempoMaximoUMDP)
                {
                    Valor++;
                }
            });

        }
        
        public void ajustarTempoTotalMeditado()
        {
            TimeSpan ts = TimeSpan.FromMinutes(dao.TemposMeditacao[0].TempoTotalMeditado);

            string horas = "0";
            
            horas = Math.Floor(ts.TotalHours).ToString();
            if (ts.TotalHours < 10)
            {
                horas = "0" + horas;
            }

            double m1 = Math.Floor(ts.TotalMinutes);

            while(m1 >= 60)
            {
                m1 -= 60;
            }

            string minutos = m1.ToString();

            if (m1 < 10)
            {
                minutos = "0" + minutos;
            }

            TempoTotalMeditado = horas + ":" + minutos;
        }

        public void ajustaLimitesMaximosDeTempos()
        {
            Meditacao med = new Meditacao();
            

            if (dao.TemposMeditacao.Count == 0)
            {
                var audioInicialHumming = ListasDeSonsMeditacao.listaDeSonsHumming.GetEnumerator();
                audioInicialHumming.MoveNext();

                var audioInicialUMP = ListasDeSonsMeditacao.listaDeSonsUMP.GetEnumerator();
                audioInicialUMP.MoveNext();

                med = new Meditacao()
                {
                    TempoMaximoHumming = 5,
                    TempoMaximoUMDP = 5,
                    AudioHumming = audioInicialHumming.Current.Key,
                    //AudioHumming = "Didgeridoo",
                    AudioUMP = audioInicialUMP.Current.Key,
                    //AudioUMP = "Qual é a realidade?",
                    TempoTotalMeditado = 0
                };
                dao.AdicionarMeditacao(med);
            } else
            {
                med = dao.TemposMeditacao[0];
            }
            
            Valor = 1;
            ValorHumming = 0;

            TempoMaximoHumming = med.TempoMaximoHumming;
            TempoMaximoUMDP = med.TempoMaximoUMDP;
        }

        public string verificarAudioExiste(string AudioAtual, Dictionary<string, string> lista)
        {
            var listaSons = lista.GetEnumerator();
            listaSons.MoveNext();
            string primeiroSom = listaSons.Current.Key;

            string audioCorreto = "";
            
            while(listaSons.Current.Key != null)
            {
                if (listaSons.Current.Key.Equals(AudioAtual)) break;

                listaSons.MoveNext();
            }

            if(listaSons.Current.Key == null)
            {
                audioCorreto = primeiroSom;
            }
            else
            {
                audioCorreto = AudioAtual;
            }

            return audioCorreto;
                
        }

    }
}
