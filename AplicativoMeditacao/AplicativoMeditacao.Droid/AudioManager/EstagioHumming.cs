
namespace AplicativoMeditacao.Droid.AudioManager
{
    public class EstagioHumming : ModeloDeEstagiosMeditacao
    {
        public EstagioHumming(GerenciadorDeAudiosMeditacao gerenciador) : base(gerenciador)
        {
        }

        public override void ajustarConfiguracoesDeAudio()
        {
            audioAtual = gerenciador.umdp.AudioHumming;
            numeroDeRepeticoes = gerenciador.umdp.NumeroRepeticoesHumming;

            if (audioAtual.Equals("semSom"))
            {
                audioGongo = "gongoHumming.mp3";
            }
            else
            {
                audioGongo = "gongoSemSom.mp3";
            }

            tempoMaximo = gerenciador.umdp.tempoMaxHumming;
            gerenciador.umdp.NomeDaMeditacao = "HUMMING";
        }

        public override void ajustarConfiguracoesDeTimer()
        {
            tempoTotalDoEstagio = gerenciador.umdp.NumeroRepeticoesHumming * gerenciador.player.Duration;
        }

        public override void comecarEstagio()
        {
            gerenciador.w1?.Acquire();
            if(numeroDeRepeticoes == 0)
            {
                terminarEstagioAtual();
            }
            else
            {
                tocarAudio();
                ajustarConfiguracoesDeTimer();
                dispararTimerDoEstagio();
            }
        }

        public override void terminarEstagioAtual()
        {
            gerenciador.estagioAtual = new EstagioUMP(gerenciador);
            gerenciador.estagioAtual.comecarEstagioAtualDaMeditacao();
        }

    }
}