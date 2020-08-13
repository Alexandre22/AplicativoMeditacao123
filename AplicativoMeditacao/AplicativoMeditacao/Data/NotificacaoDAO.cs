using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.Model;
using SQLite;
using System;
using System.Collections.ObjectModel;

namespace AplicativoMeditacao.Data
{
    public class NotificacaoDAO
    {
        INotifier notifier;
        private SQLiteConnection conexao;
        private ObservableCollection<Notificacao> lista;
        public ObservableCollection<Notificacao> Notificacoes
        {
            get
            {
                return lista;
            }
            protected set { lista = value; }
        }

        public NotificacaoDAO(SQLiteConnection con, INotifier not)
        {
            notifier = not;
            conexao = con;
            conexao.CreateTable<Notificacao>();
            lista = GetAll();
        }

        public void SalvarNotificacao(Notificacao not)
        {
            if (not.ID != 0)
            {
                conexao.Update(not);
                AtivarNotificacao(not);
            }
            else
            {
                int id = conexao.Insert(not);
                lista.Add(not);
                AtivarNotificacao(not);
                AtivarVibracao(not);
            }

        }

        public void RemoverNotificacao(Notificacao not)
        {
            conexao.Delete(not);
            lista.Remove(not);
            DesativarNotificacao(not);
        }

        public void AtivarNotificacao(Notificacao not)
        {
            not.StatusAlarme = "Ativado";
            not.Ativado = true;

            if (not.Repetir)
            {
                var ids = not.gerarIdsAlarmes().Split(',');
                for (int i = 0; i < not.DiasDaSemana.Length; i++)
                {
                    notifier.LocalNotify(Convert.ToInt32(ids[i]), "Um Minuto de Presença", not.Legenda, (int)Char.GetNumericValue(not.DiasDaSemana[i]), not.Hora, not.Minuto, true);
                }
            }
            else
            {
                not.DiasDaSemana = retornaDiaDaSemana(not);
                var ids = not.gerarIdsAlarmes().Split(',');
                notifier.LocalNotify(Convert.ToInt32(ids[0]), "Um Minuto de Presença", not.Legenda, (int)Char.GetNumericValue(not.DiasDaSemana[0]), not.Hora, not.Minuto, false);
            }
            
            conexao.Update(not);

        }

        public void DesativarNotificacao(Notificacao not)
        {
            not.StatusAlarme = "Desativado";
            not.Ativado = false;

            conexao.Update(not);

            var ids = not.gerarIdsAlarmes().Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                notifier.cancelNotify(Convert.ToInt32(ids[i]));
            }
        }

        public void AtivarVibracao(Notificacao not)
        {
            not.StatusVibracao = "Vibrar";
            not.Vibrar = true;

            conexao.Update(not);
        }

        public void DesativarVibracao(Notificacao not)
        {
            not.StatusVibracao = "Não Vibrar";
            not.Vibrar = false;

            conexao.Update(not);
        }

        public void AtivarSom(Notificacao not)
        {
            not.StatusSom = "Com Som";
            not.Som = true;

            conexao.Update(not);
        }

        public void DesativarSom(Notificacao not)
        {
            not.StatusSom = "Sem Som";
            not.Som = false;

            conexao.Update(not);
        }

        public void MostrarEsconder(Notificacao not)
        {
            if (not.MostrarDados)
            {
                not.MostrarDados = false;
                not.MostrarEsconder = "+";
            }
            else
            {
                not.MostrarDados = true;
                not.MostrarEsconder = "-";
            }

            conexao.Update(not);
        }

        public string retornaDiaDaSemana(Notificacao not)
        {
            string diaDaSemana = "";
            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            var diaSemana = Convert.ToInt32(hoje.DayOfWeek);
            var horaAtual = hoje.Hour;
            var minutoAtual = hoje.Minute;

            if (horaAtual > not.Hora)
            {
                diaSemana++;
            }
            else if (horaAtual == not.Hora)
            {
                if (minutoAtual >= not.Minuto)
                {
                    diaSemana++;
                }
            }

            if (diaSemana == 7) diaSemana = 0;

            diaDaSemana = Convert.ToString(diaSemana);

            return diaDaSemana;
        }

        private ObservableCollection<Notificacao> GetAll()
        {
            return new ObservableCollection<Notificacao>(conexao.Table<Notificacao>());
        }
    }
}
