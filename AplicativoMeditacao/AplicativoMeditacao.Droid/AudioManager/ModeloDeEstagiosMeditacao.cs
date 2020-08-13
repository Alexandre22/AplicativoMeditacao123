using Android.Content;
using Android.Media;
using Android.OS;
using System;

namespace AplicativoMeditacao.Droid.AudioManager
{
    public abstract class ModeloDeEstagiosMeditacao
    {
        protected GerenciadorDeAudiosMeditacao gerenciador;
        protected int tempoTotalDoEstagio;
        protected string audioAtual;
        protected string audioGongo;
        public int numeroDeRepeticoes;
        public int tempoMaximo;

        public DateTime ultimoTempo;
        
        public ModeloDeEstagiosMeditacao(GerenciadorDeAudiosMeditacao gerenciador)
        {
            this.gerenciador = gerenciador;
            
            if(gerenciador._timer == null)
            {
                criarTimer();
            }
            
            var context = Android.App.Application.Context;
            
            if(gerenciador.w1 == null)
            {
                gerenciador.pm = (PowerManager)context.GetSystemService(Context.PowerService);
                gerenciador.w1 = gerenciador.pm.NewWakeLock(WakeLockFlags.Partial, "TimerRunning");
            }
        }
        
        public abstract void ajustarConfiguracoesDeAudio();
        public abstract void ajustarConfiguracoesDeTimer();
        public abstract void comecarEstagio();
        public abstract void terminarEstagioAtual();

        public void acrescentarMinuto()
        {
            if (numeroDeRepeticoes == 0) return;
            var novoTempo = gerenciador.umdp.TempoTotal + TimeSpan.FromMilliseconds(gerenciador.player.Duration);

            var limiteTempo = TimeSpan.FromMilliseconds(tempoMaximo * gerenciador.player.Duration);
            if (novoTempo <= limiteTempo)
            {
                gerenciador.umdp.TempoTotal = novoTempo;
                numeroDeRepeticoes++;
            }
        }

        public void tocarAudio()
        {
            gerenciador.player = new MediaPlayer();
            gerenciador.player.Completion += (sender, args) =>
            {
                gerenciador.player.Release();
                gerenciador.player = null;
                terminoDoAudio();
            };
            if (gerenciador._timer?.Enabled == false)
            {
                gerenciador._timer.Enabled = true;
                gerenciador.umdp.NomeBotao = "II";
            }

            var fd = global::Android.App.Application.Context.Assets.OpenFd(audioAtual + ".mp3");
            gerenciador.player.Prepared += (s, e) =>
            {
                gerenciador.player.Start();
            };
            gerenciador.player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            gerenciador.player.Prepare();
        }

        public void tocarGongo()
        {
            gerenciador.player = new MediaPlayer();
            gerenciador.player.Completion += (sender, args) =>
            {
                gerenciador.player.Release();
                gerenciador.player = null;
                terminarEstagioAtual();
            };
            if (gerenciador._timer.Enabled == false)
            {
                gerenciador._timer.Enabled = true;
                gerenciador.umdp.NomeBotao = "II";
            }

            var fd = global::Android.App.Application.Context.Assets.OpenFd(audioGongo);
            gerenciador.player.Prepared += (s, e) =>
            {
                gerenciador.player.Start();
            };
            gerenciador.player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            gerenciador.player.Prepare();
        }

        public void terminoDoAudio()
        {
            numeroDeRepeticoes--;
            var diferencaDeTempo = DateTime.Now - ultimoTempo;
            gerenciador.umdp.TempoFinal += diferencaDeTempo.ToString() + "; ";
            ultimoTempo = DateTime.Now;
            if (numeroDeRepeticoes > 0)
            {
                tocarAudio();
            }
            else
            {
                tocarGongo();
            }
        }

        public void criarTimer()
        {
            gerenciador._timer = new System.Timers.Timer();

            gerenciador._timer.Interval = 1000;

            gerenciador._timer.Elapsed += (s, a) =>
            {
                TimeSpan ts = gerenciador.umdp.TempoTotal.Subtract(new System.TimeSpan(0, 0, 1));

                gerenciador.umdp.TempoTotal = ts;

                if (gerenciador.umdp.TempoTotal.TotalSeconds <= 0)
                {
                    gerenciador._timer.Stop();
                }
            };
        }

        public void dispararTimerDoEstagio()
        {
            gerenciador.umdp.TempoTotal = TimeSpan.FromMilliseconds(tempoTotalDoEstagio);
            
            gerenciador._timer.Start();
        }

        public void comecarEstagioAtualDaMeditacao()
        {
            ultimoTempo = DateTime.Now;
            ajustarConfiguracoesDeAudio();
            comecarEstagio();
        }

    }
}