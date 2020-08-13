using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AplicativoMeditacao.Droid.AudioManager
{
    class EstagioUMP : ModeloDeEstagiosMeditacao
    {
        public EstagioUMP(GerenciadorDeAudiosMeditacao gerenciador) : base(gerenciador)
        {
        }

        public override void ajustarConfiguracoesDeAudio()
        {
            audioAtual = gerenciador.umdp.AudioUMP;
            numeroDeRepeticoes = gerenciador.umdp.NumeroRepeticoes;
            audioGongo = "gongoHumming.mp3";
            tempoMaximo = gerenciador.umdp.tempoMaxUMDP;
            gerenciador.umdp.NomeDaMeditacao = "UM MINUTO DE PRESENÇA";
        }

        public override void ajustarConfiguracoesDeTimer()
        {
            tempoTotalDoEstagio = gerenciador.umdp.NumeroRepeticoes * gerenciador.player.Duration;
        }

        public override void comecarEstagio()
        {
            tocarAudio();
            ajustarConfiguracoesDeTimer();
            dispararTimerDoEstagio();
        }

        public override void terminarEstagioAtual()
        {
            gerenciador.w1?.Release();
            gerenciador.umdp.FinalizarMeditacao = true;
        }
        
    }
}