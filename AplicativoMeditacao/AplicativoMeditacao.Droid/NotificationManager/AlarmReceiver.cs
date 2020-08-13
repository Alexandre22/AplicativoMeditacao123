using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using AplicativoMeditacao.Data;
using AplicativoMeditacao.Droid.SQLite;
using AplicativoMeditacao.Model;
using Java.Lang;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace AplicativoMeditacao.Droid.NotificationManager
{
    [BroadcastReceiver]
    [IntentFilter(new[] { Android.Content.Intent.ActionBootCompleted })]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var message = intent.GetStringExtra("message");
            var title = intent.GetStringExtra("title");
            var id = intent.GetStringExtra("id");

            if (title == "Um Minuto de Presença")
            {
                id = id.Remove(id.Length - 1);

                var notIntent = new Intent(context, typeof(MainActivity));
                var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
                var manager = NotificationManagerCompat.From(context);

                SQLite_Android sqlite = new SQLite_Android();
                PlatformNotifier pf = new PlatformNotifier();
                NotificacaoDAO dao = new NotificacaoDAO(sqlite.GetConnection(), pf);
                ObservableCollection<Notificacao> notificacoes = dao.Notificacoes;
                var numero = Convert.ToInt32(id);

                var notificacao = (from not in notificacoes
                                   where not.ID == numero
                                   select not).FirstOrDefault<Notificacao>();

                if (!notificacao.Repetir)
                {
                    dao.DesativarNotificacao(notificacao);
                }

                //gerando notificacao para disparo

                var builder = new Notification.Builder(context)
                    .SetContentIntent(contentIntent)
                    .SetSmallIcon(Resource.Drawable.ic_notification)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
                    .SetAutoCancel(true);

                if (notificacao.Vibrar)
                {
                    builder.SetDefaults(NotificationDefaults.Vibrate);
                }

                if (notificacao.Som)
                {
                    builder.SetSound(Android.Net.Uri.Parse("android.resource://" + context.ApplicationContext.PackageName + "/Raw/" + Resource.Raw.som_notificacao));
                }

                var notification = builder.Build();
                manager.Notify(0, notification);

            }
            else
            {
                SQLite_Android sqlite = new SQLite_Android();
                PlatformNotifier pf = new PlatformNotifier();
                NotificacaoDAO dao = new NotificacaoDAO(sqlite.GetConnection(), pf);
                ObservableCollection<Notificacao> notificacoes = dao.Notificacoes;

                for (int i = 0; i < notificacoes.Count; i++)
                {
                    if (notificacoes[i].Ativado)
                    {
                        dao.AtivarNotificacao(notificacoes[i]);
                    }
                }

            }

        }

    }
}