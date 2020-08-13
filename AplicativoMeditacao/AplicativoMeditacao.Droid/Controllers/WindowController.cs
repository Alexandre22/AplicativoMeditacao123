using AplicativoMeditacao.Interfaces;
using Android.OS;
using Android.App;
using Xamarin.Forms;
using Android.Views;
using Android.Content;
using AplicativoMeditacao.Droid.Controllers;

[assembly: Dependency(typeof(WindowController))]
namespace AplicativoMeditacao.Droid.Controllers
{
    public class WindowController : IWindowController
    {
        static PowerManager powerManager;
        static PowerManager.WakeLock wakeLock;
        static Activity activity;

        public void acenderTela()
        {
            var context = Android.App.Application.Context;

            powerManager = (PowerManager)context.GetSystemService(Context.PowerService);
            wakeLock = powerManager.NewWakeLock(WakeLockFlags.Full | WakeLockFlags.AcquireCausesWakeup, "WakeUpScreen");

            activity = (Activity)Forms.Context;
            activity.Window.AddFlags(WindowManagerFlags.DismissKeyguard |
                WindowManagerFlags.ShowWhenLocked |
                WindowManagerFlags.TurnScreenOn);

            wakeLock.Acquire();
        }

        public void apagarTela()
        {
            wakeLock.Release();

            activity.Window.ClearFlags(WindowManagerFlags.DismissKeyguard |
                WindowManagerFlags.ShowWhenLocked |
                WindowManagerFlags.TurnScreenOn);
        }
    }
}