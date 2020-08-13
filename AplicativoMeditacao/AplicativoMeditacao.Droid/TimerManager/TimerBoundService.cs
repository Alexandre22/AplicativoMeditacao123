
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AplicativoMeditacao.ViewModel;
using System;
using System.Collections.Generic;

namespace AplicativoMeditacao.Droid.TimerManager
{

    [Service(Name = "com.xamarin.TimerBoundService")]
    public class TimerBoundService : Service
    {
        private System.Timers.Timer _timer;
        MeditacaoUMDPViewModel umdp;
        public TimerBinder mBinder;
        private List<string> listaHumming;
        private List<string> listaUMDP;
        int tempoHumming;
        int tempoUMDP;
        string comandoAtual;
        private MediaPlayer player;
        private PowerManager pm;
        PowerManager.WakeLock w1;

        public override void OnCreate()
        {
            base.OnCreate();
            
        }

        public override IBinder OnBind(Intent intent)
        {
            mBinder = new TimerBinder(this);
            return mBinder;
        }

        public override bool OnUnbind(Intent intent)
        {
            return base.OnUnbind(intent);
        }

        public override void OnDestroy()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }

            base.OnDestroy();
        }

        //acima estao os metodos necessarios para criaçao e destruiçao do service

        //a partir daqui começarao os metodos implementados para fazer as tasks necessarias do aplicativo

        public void iniciarTimer(MeditacaoUMDPViewModel umdpvm)
        {
            umdp = umdpvm;
            int repeticoesHumming = umdp.NumeroRepeticoesHumming;
            int repeticoes = umdp.NumeroRepeticoes;

            var context = Android.App.Application.Context;

            pm = (PowerManager)context.GetSystemService(PowerService);
            w1 = pm.NewWakeLock(WakeLockFlags.Partial, "TimerRunning");

            tempoHumming = repeticoesHumming * 60000;
            tempoUMDP = repeticoes * 60000;

            montarListaHumming();

            montarListaUMDP();

            executarComandos();
        }

        private void montarListaHumming()
        {
            listaHumming = new List<string>();
            if (tempoHumming != 0)
            {
                listaHumming.Add("humming");
                listaHumming.Add("gongoHumming");
            }
        }

        private void montarListaUMDP()
        {
            listaUMDP = new List<string>();
            listaUMDP.Add("umdp");
            listaUMDP.Add("gongoFinal");
        }

        private void executarComandos()
        {
            if (listaHumming.Count > 0)
            {
                comandoAtual = listaHumming[0];
                listaHumming.RemoveAt(0);
            }
            else if (listaUMDP.Count > 0)
            {
                comandoAtual = listaUMDP[0];
                listaUMDP.RemoveAt(0);
            }
            else
            {
                comandoAtual = "finalizado";
                w1?.Release();
                umdp.FinalizarMeditacao = true;
                return;
            }

            if (comandoAtual.Equals("humming"))
            {
                criarTimer(tempoHumming);
            }
            else if (comandoAtual.Equals("gongoHumming"))
            {
                prepararPlayer("gongoHumming");
            }
            else if (comandoAtual.Equals("umdp"))
            {
                criarTimer(tempoUMDP);
            }
            else if (comandoAtual.Equals("gongoFinal"))
            {
                prepararPlayer("gongo");
            }

        }

        private void prepararPlayer(string nomeArquivo)
        {
            player = new MediaPlayer();

            player.Completion += (sender, args) =>
            {
                player.Release();
                player = null;
                executarComandos();
            };

            var fd = global::Android.App.Application.Context.Assets.OpenFd(nomeArquivo + ".mp3");
            player.Prepared += (s, e) =>
            {
                player.Start();
            };
            player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            player.Prepare();
        }

        private void criarTimer(int tempo)
        {
            umdp.TempoTotal = TimeSpan.FromMilliseconds(tempo);
            _timer = new System.Timers.Timer();
            //Trigger event every second
            _timer.Interval = 1000;
            
            _timer.Elapsed += (s, a) =>
            {
                TimeSpan ts = umdp.TempoTotal.Subtract(new System.TimeSpan(0, 0, 1));
                umdp.TempoTotal = ts;
                
                if (umdp.TempoTotal.TotalSeconds <= 0)
                {
                    _timer.Stop();
                    executarComandos();
                }

                /*if (umdp.TempoTotal.Seconds == 0 && umdp.TempoTotal.Minutes == 0 && umdp.TempoTotal.Hours == 0)
                {
                    _timer.Stop();
                    executarComandos();
                }*/
            };
            
            _timer?.Start();

            w1?.Acquire();
        }

        public void resetTimer()
        {
            _timer.Stop();
            _timer.Enabled = false;
            if (player != null)
            {
                player.Stop();
                player.Release();
                player = null;
            }

            if (comandoAtual.Equals("humming") || comandoAtual.Equals("gongoHumming"))
            {
                montarListaHumming();
                executarComandos();
            }
            else if (comandoAtual.Equals("umdp") || comandoAtual.Equals("gongoFinal"))
            {
                montarListaUMDP();
                executarComandos();
            }
        }

        public void increaseTimer()
        {
            var novoTempo = umdp.TempoTotal + TimeSpan.FromMilliseconds(60 * 1000);
            TimeSpan limiteTempo = new TimeSpan(0, 0, 0);

            if (comandoAtual.Equals("humming"))
            {
                limiteTempo = TimeSpan.FromMilliseconds(umdp.tempoMaxHumming * 60000);

            }
            else if (comandoAtual.Equals("umdp"))
            {
                limiteTempo = TimeSpan.FromMilliseconds(umdp.tempoMaxUMDP * 60000);
            }

            if (novoTempo <= limiteTempo)
            {
                umdp.TempoTotal = umdp.TempoTotal + TimeSpan.FromMilliseconds(60 * 1000);
            }

        }

        public void pauseTimer()
        {
            if (comandoAtual.Equals("humming") || comandoAtual.Equals("umdp"))
            {
                _timer.Enabled = false;
            }
            else
            {
                if (player != null)
                {
                    player.Pause();
                }
            }
        }

        public void resumeTimer()
        {
            if (comandoAtual.Equals("humming") || comandoAtual.Equals("umdp"))
            {
                _timer.Enabled = true;
            }
            else
            {
                if (player != null)
                {
                    player.Start();
                }
            }
        }

        public void pararTimer()
        {
            //liberar wake lock
            w1?.Release();
            if (comandoAtual.Equals("humming") || comandoAtual.Equals("umdp"))
            {
                _timer.Enabled = false;
            }
            else
            {
                if (player != null)
                {
                    player.Stop();
                    player.Release();
                    player = null;
                }
            }
        }

    }
}