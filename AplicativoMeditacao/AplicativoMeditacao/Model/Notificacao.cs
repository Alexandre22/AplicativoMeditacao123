using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AplicativoMeditacao.Model
{
    public class Notificacao : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        private string legenda;
        public string Legenda
        {
            get
            {
                return legenda;
            }
            set
            {
                if (legenda != value)
                {
                    SetField(ref legenda, value, "Legenda");
                }
            }
        }

        public int Hora { get; set; }
        public int Minuto { get; set; }

        private string horarioTexto;
        public string HorarioTexto
        {
            get
            {
                return horarioTexto;
            }
            set
            {
                if (horarioTexto != value)
                {
                    SetField(ref horarioTexto, value, "HorarioTexto");
                }
            }
        }
        

        private string statusAlarme;
        public string StatusAlarme
        {
            get
            {
                return statusAlarme;
            }
            set
            {
                if (statusAlarme != value)
                {
                    SetField(ref statusAlarme, value, "StatusAlarme");
                }
            }
        }

        private bool ativado;

        public bool Ativado
        {
            get
            {
                return ativado;
            }
            set
            {
                if (ativado != value)
                {
                    SetField(ref ativado, value, "Ativado");
                }
            }
        }

        public bool Repetir { get; set; }

        public string DiasDaSemana { get; set; }

        private string statusVibracao;

        public string StatusVibracao
        {
            get
            {
                return statusVibracao;
            }
            set
            {
                if (statusVibracao != value)
                {
                    SetField(ref statusVibracao, value, "StatusVibracao");
                }
            }
        }

        

        public bool Vibrar { get; set; }

        private string statusSom;

        public string StatusSom
        {
            get
            {
                return statusSom;
            }
            set
            {
                if (statusSom != value)
                {
                    SetField(ref statusSom, value, "StatusSom");
                }
            }
        }
        

        public bool Som { get; set; }

        private bool mostrarDados;

        public bool MostrarDados
        {
            get
            {
                return mostrarDados;
            }
            set
            {
                if (mostrarDados != value)
                {
                    SetField(ref mostrarDados, value, "MostrarDados");
                }
            }
        }

        private string mostrarEsconder;

        public string MostrarEsconder
        {
            get
            {
                return mostrarEsconder;
            }
            set
            {
                if (mostrarEsconder != value)
                {
                    SetField(ref mostrarEsconder, value, "MostrarEsconder");
                }
            }
        }

        public Notificacao()
        {
            StatusAlarme = "Ativado";
            Ativado = true;
            StatusVibracao = "Vibrar";
            Vibrar = true;
            StatusSom = "Com Som";
            Som = true;
            MostrarDados = true;
            MostrarEsconder = "-";
        }

        public string geraHorarioTexto()
        {
            string horaTexto = Convert.ToString(Hora);
            string minutoTexto = Convert.ToString(Minuto);
            if (Hora < 10)
            {
                horaTexto = "0" + Convert.ToString(Hora);
            }
            if (Minuto < 10)
            {
                minutoTexto = "0" + Convert.ToString(Minuto);
            }

            HorarioTexto = horaTexto + ":" + minutoTexto;

            if (Repetir)
            {
                foreach (var n in DiasDaSemana)
                {
                    HorarioTexto += ", " + (DiaDaSemana)(int)Char.GetNumericValue(n);
                }
            }

            return HorarioTexto;
        }

        public string gerarIdsAlarmes()
        {
            string idsAlarmes = "";
            foreach (var n in DiasDaSemana)
            {
                idsAlarmes += Convert.ToString(ID) + n + ",";
            }
            idsAlarmes = idsAlarmes.Remove(idsAlarmes.Length - 1);
            return idsAlarmes;
        }

    }
}
