using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicativoMeditacao.Interfaces
{
    public interface INotifier
    {
        void LocalNotify(int idNotificacao, string title, string message, Int32 diaDaSemanaAlarme, Int32 horaAlarme, Int32 minutoAlarme, bool repetir);
        void cancelNotify(int requestCode);
    }
}
