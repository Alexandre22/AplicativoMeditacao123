using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.ViewModel;
using Android.Media;
using Android.OS;
using AplicativoMeditacao.Droid.AudioManager;
using Xamarin.Forms;
using Java.Lang;

[assembly: Dependency(typeof(GerenciadorDeAudiosMeditacao))]
namespace AplicativoMeditacao.Droid.AudioManager
{
    public class GerenciadorDeAudiosMeditacao : IAudio
    {
        public ModeloDeEstagiosMeditacao estagioAtual;
        public MeditacaoUMDPViewModel umdp;
        public System.Timers.Timer _timer;
        public MediaPlayer player;
        public PowerManager pm;
        public PowerManager.WakeLock w1;

        public void acrescentarMinuto()
        {
            estagioAtual.acrescentarMinuto();
        }

        public void pararMeditacao()
        {
            if (player != null)
            {
                player?.Stop();
                player?.Release();
                player = null;
                _timer?.Stop();
                w1?.Release();
            }
        }

        public void pausarMeditacao()
        {
            player?.Pause();
            _timer.Enabled = false;
        }

        public void reiniciaMeditacao()
        {
            pararMeditacao();
            w1?.Acquire();
            estagioAtual.comecarEstagioAtualDaMeditacao();
        }

        public void resumirMeditacao()
        {
            player?.Start();
            _timer.Enabled = true;
        }

        public void tocarSomMeditacao(MeditacaoUMDPViewModel umdp)
        {
            this.umdp = umdp;
            umdp.TempoFinal = "";
            estagioAtual = new EstagioHumming(this);
            estagioAtual.comecarEstagioAtualDaMeditacao();
        }
    }
}