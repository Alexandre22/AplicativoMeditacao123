using System;

using Android.App;
using Android.Content;
using Xamarin.Forms;
using Android.OS;
using AplicativoMeditacao.Droid.NotificationManager;
using AplicativoMeditacao.Interfaces;

[assembly: Dependency(typeof(PlatformNotifier))]
namespace AplicativoMeditacao.Droid.NotificationManager
{
    class PlatformNotifier : INotifier
    {
        public void cancelNotify(int requestCode)
        {
            var context = Android.App.Application.Context;

            Intent alarmIntent = new Intent(context, typeof(AlarmReceiver));

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, requestCode, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

            alarmManager.Cancel(pendingIntent);
        }

        public void LocalNotify(int idNotificacao, string title, string message, Int32 diaDaSemanaAlarme, Int32 horaAlarme, Int32 minutoAlarme, bool repetir)
        {
            var context = Android.App.Application.Context;

            Intent alarmIntent = new Intent(context, typeof(AlarmReceiver));

            alarmIntent.PutExtra("message", message);
            alarmIntent.PutExtra("title", title);
            alarmIntent.PutExtra("id", Convert.ToString(idNotificacao));

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, idNotificacao, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            var diaDaSemana = Convert.ToInt32(hoje.DayOfWeek);
            var horaAtual = hoje.Hour;
            var minutoAtual = hoje.Minute;

            int diasAdicionar = 0;

            if (diaDaSemana == diaDaSemanaAlarme)
            {
                if (horaAtual > horaAlarme)
                {
                    diasAdicionar = 7;
                    diaDaSemana = diaDaSemanaAlarme;
                }
                else if (horaAtual == horaAlarme && minutoAtual >= minutoAlarme)
                {
                    diasAdicionar = 7;
                    diaDaSemana = diaDaSemanaAlarme;
                }
            }
            while (diaDaSemana != diaDaSemanaAlarme)
            {
                diasAdicionar++;
                diaDaSemana++;
                if (diaDaSemana == 7) diaDaSemana = 0;
            }

            var tempoAlarme = new DateTime(DateTime.Now.AddDays(diasAdicionar).Year, DateTime.Now.AddDays(diasAdicionar).Month, DateTime.Now.AddDays(diasAdicionar).Day, horaAlarme, minutoAlarme, 0);

            var diferencaTempo = tempoAlarme - hoje;
            var tempoDisparo = (long)diferencaTempo.TotalMilliseconds;

            if (repetir)
            {
                alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + tempoDisparo, AlarmManager.IntervalDay * 7, pendingIntent);
            }
            else
            {
                alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + tempoDisparo, pendingIntent);
            }

        }

    }
}