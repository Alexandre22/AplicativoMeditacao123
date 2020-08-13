using System;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.ViewModel;
using System.Collections.Generic;
using Android.Media;
using Xamarin.Forms;
using AplicativoMeditacao.Droid.TimerManager;
using Android.Content;
using System.Threading.Tasks;
using Android.App;
using System.Diagnostics;

[assembly: Dependency(typeof(TimerService))]
namespace AplicativoMeditacao.Droid.TimerManager
{
    public class TimerService : ITimer
    {
        TimerServiceConnection timerConnection;

        public void startServiceTimer(MeditacaoUMDPViewModel umdp)
        {
            var context = Android.App.Application.Context;

            var timerIntent = new Intent(context, typeof(TimerBoundService));
            timerConnection = new TimerServiceConnection();
            var retorno = context.BindService(timerIntent, timerConnection, Bind.AutoCreate);

            var t = new Task(() => 
            {
                try
                {
                    while (timerConnection.Binder == null) { }
                    timerConnection.Binder?.iniciarTimer(umdp);
                }
                catch (Exception e)
                { }
            });
            t.Start();

        }
        
        public void increaseTimer()
        {
            try
            {
                timerConnection.Binder?.increaseTimer();
            }catch(Exception e) { }
        }

        public void pauseTimer()
        {
            try
            {
                timerConnection.Binder?.pauseTimer();
            }
            catch (Exception e) { }
        }

        public void resetTimer()
        {
            try
            {
                timerConnection.Binder?.resetTimer();
            }
            catch (Exception e) { }
        }

        public void resumeTimer()
        {
            try
            {
                timerConnection.Binder?.resumeTimer();
            }
            catch (Exception e) { }
        }

        public void stopTimer()
        {
            try
            {
                timerConnection.Binder?.pararTimer();
            }
            catch (Exception e) { }
        }
        
    }
}