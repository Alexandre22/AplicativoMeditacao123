using Android.Content;
using Android.OS;

namespace AplicativoMeditacao.Droid.TimerManager
{
    public class TimerServiceConnection : Java.Lang.Object, IServiceConnection
    {
        public bool IsConnected { get; private set; }
        public TimerBinder Binder { get; set; }

        public TimerServiceConnection()
        {
            IsConnected = false;
            Binder = null;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            Binder = service as TimerBinder;
            IsConnected = Binder != null;
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            IsConnected = false;
            Binder = null;
        }
    }
}