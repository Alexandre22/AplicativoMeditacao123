using Android.OS;
using AplicativoMeditacao.ViewModel;

namespace AplicativoMeditacao.Droid.TimerManager
{
    public class TimerBinder : Binder
    {
        public TimerBoundService Service { get; protected set; }

        public TimerBinder(TimerBoundService service)
        {
            this.Service = service;
        }

        public void iniciarTimer(MeditacaoUMDPViewModel page)
        {
            Service?.iniciarTimer(page);
        }

        public void pararTimer()
        {
            Service?.pararTimer();
        }

        public void resetTimer()
        {
            Service?.resetTimer();
        }

        public void increaseTimer()
        {
            Service?.increaseTimer();
        }

        public void pauseTimer()
        {
            Service?.pauseTimer();
        }

        public void resumeTimer()
        {
            Service?.resumeTimer();
        }

    }
}