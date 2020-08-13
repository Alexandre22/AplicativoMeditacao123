using AplicativoMeditacao.Data;
using AplicativoMeditacao.Model;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace AplicativoMeditacao.ViewModel
{
    class SalvaNotificacaoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string textoNotificacao;
        public string TextoNotificacao
        {
            get
            {
                return textoNotificacao;
            }
            set
            {
                if (textoNotificacao != value)
                {
                    textoNotificacao = value;
                    OnPropertyChanged("TextoNotificacao");
                    LimitacaoCaracteres();
                }
            }
        }

        private TimeSpan horario;
        public TimeSpan Horario
        {
            get
            {
                return horario;
            }
            set
            {
                if (horario != value)
                {
                    horario = value;
                    OnPropertyChanged("Horario");
                }
            }
        }

        private bool flag;

        private bool isDomOn = false;
        private bool isSegOn = false;
        private bool isTerOn = false;
        private bool isQuaOn = false;
        private bool isQuiOn = false;
        private bool isSexOn = false;
        private bool isSabOn = false;

        public bool IsDomOn
        {
            get
            {
                return isDomOn;
            }

            set
            {
                if (isDomOn != value)
                {
                    isDomOn = value;
                    OnPropertyChanged("IsDomOn");
                    NaoRepetirNotificacao();
                }
            }
        }
        public bool IsSegOn
        {
            get
            {
                return isSegOn;
            }
            set
            {
                if (isSegOn != value)
                {
                    isSegOn = value;
                    OnPropertyChanged("IsSegOn");
                    NaoRepetirNotificacao();
                }
            }
        }
        public bool IsTerOn
        {
            get
            {
                return isTerOn;
            }
            set
            {
                if (isTerOn != value)
                {
                    isTerOn = value;
                    OnPropertyChanged("IsTerOn");
                    NaoRepetirNotificacao();
                }
            }
        }
        public bool IsQuaOn
        {
            get
            {
                return isQuaOn;
            }
            set
            {
                if (isQuaOn != value)
                {
                    isQuaOn = value;
                    OnPropertyChanged("IsQuaOn");
                    NaoRepetirNotificacao();
                }
            }
        }
        public bool IsQuiOn
        {
            get
            {
                return isQuiOn;
            }
            set
            {
                if (isQuiOn != value)
                {
                    isQuiOn = value;
                    OnPropertyChanged("IsQuiOn");
                    NaoRepetirNotificacao();
                }
            }
        }
        public bool IsSexOn
        {
            get
            {
                return isSexOn;
            }
            set
            {
                if (isSexOn != value)
                {
                    isSexOn = value;
                    OnPropertyChanged("IsSexOn");
                    NaoRepetirNotificacao();
                }
            }
        }
        public bool IsSabOn
        {
            get
            {
                return isSabOn;
            }
            set
            {
                if (isSabOn != value)
                {
                    isSabOn = value;
                    OnPropertyChanged("IsSabOn");
                    NaoRepetirNotificacao();
                }
            }
        }

        private bool repetir;
        public bool Repetir
        {
            get
            {
                return repetir;
            }
            set
            {
                if (repetir != value)
                {
                    repetir = value;
                    OnPropertyChanged("Repetir");
                    RepetirNotificacao();
                }
            }
        }
        
        private NotificacaoDAO dao;

        Notificacao notificacao;

        public INavigation Navigation { get; set; }

        public SalvaNotificacaoViewModel(NotificacaoDAO notDao)
        {
            flag = true;
            dao = notDao;
            notificacao = new Notificacao();
            Horario = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
        }
        
        public SalvaNotificacaoViewModel(NotificacaoDAO notDao, Notificacao not)
        {
            dao = notDao;
            flag = true;
            notificacao = not;
            Horario = new TimeSpan(not.Hora, not.Minuto, 0);
            TextoNotificacao = not.Legenda;
            Repetir = not.Repetir;
            if (Repetir)
            {
                IsDomOn = retornaDiaTrueFalse('0');
                IsSegOn = retornaDiaTrueFalse('1');
                IsTerOn = retornaDiaTrueFalse('2');
                IsQuaOn = retornaDiaTrueFalse('3');
                IsQuiOn = retornaDiaTrueFalse('4');
                IsSexOn = retornaDiaTrueFalse('5');
                IsSabOn = retornaDiaTrueFalse('6');
            }
        }

        public bool retornaDiaTrueFalse(char d)
        {
            foreach (char c in notificacao.DiasDaSemana)
            {
                if (c == d) return true;
            }
            return false;
        }

        public ICommand SalvarNotificacao
        {
            get
            {
                return new Command(async () =>
                {
                    if (flag)
                    {
                        flag = false;

                        string DiasSemana = "";

                        notificacao.Repetir = repetir;

                        if (repetir)
                        {
                            DiasSemana = MontaListaDiasDaSemana();
                            notificacao.Legenda = TextoNotificacao;
                            notificacao.Hora = Horario.Hours;
                            notificacao.Minuto = Horario.Minutes;
                            notificacao.DiasDaSemana = DiasSemana;
                        }
                        else
                        {
                            notificacao.Legenda = TextoNotificacao;
                            notificacao.Hora = Horario.Hours;
                            notificacao.Minuto = Horario.Minutes;
                        }

                        notificacao.geraHorarioTexto();

                        dao.SalvarNotificacao(notificacao);

                        await Navigation.PopAsync();

                        flag = true;
                    }

                });
            }
        }

        //retorna os dias da semana em que o alarme sera disparado
        public string MontaListaDiasDaSemana()
        {
            string dias = "";

            if (IsDomOn)
            {
                dias += 0;
            }
            if (IsSegOn)
            {
                dias += 1;
            }
            if (IsTerOn)
            {
                dias += 2;
            }
            if (IsQuaOn)
            {
                dias += 3;
            }
            if (IsQuiOn)
            {
                dias += 4;
            }
            if (IsSexOn)
            {
                dias += 5;
            }
            if (IsSabOn)
            {
                dias += 6;
            }

            return dias;
        }

        public void NaoRepetirNotificacao()
        {
            if (!isDomOn && !isSegOn && !isTerOn && !isQuaOn && !isQuiOn && !isSexOn && !isSabOn)
            {
                Repetir = false;
            }
        }

        public void RepetirNotificacao()
        {
            if (Repetir)
            {
                IsDomOn = true;
                IsSegOn = true;
                IsTerOn = true;
                IsQuaOn = true;
                IsQuiOn = true;
                IsSexOn = true;
                IsSabOn = true;
            }
            else
            {
                IsDomOn = false;
                IsSegOn = false;
                IsTerOn = false;
                IsQuaOn = false;
                IsQuiOn = false;
                IsSexOn = false;
                IsSabOn = false;
            }
        }
        
        public void LimitacaoCaracteres()
        {
            if (TextoNotificacao.Length > 20)
            {
                TextoNotificacao = TextoNotificacao.Remove(TextoNotificacao.Length - 1);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        
    }
}
