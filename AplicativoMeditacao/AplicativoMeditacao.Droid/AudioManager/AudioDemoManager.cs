using AplicativoMeditacao.Interfaces;
using Android.Media;
using Xamarin.Forms;
using AplicativoMeditacao.Droid.AudioManager;

[assembly: Dependency(typeof(AudioDemoManager))]
namespace AplicativoMeditacao.Droid.AudioManager
{
    public class AudioDemoManager : IAudioDemo
    {
        private MediaPlayer player = null;

        public void tocarDemonstracao()
        {
            if(player != null)
            {
                pararMeditacao();
                tocarDemonstracao();
            }
            else
            {
                player = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.hummingDemo);

                player.Completion += (sender, args) =>
                {
                    pararMeditacao();
                };

                player.Start();
            }
        }

        public void pararMeditacao()
        {
            if(player != null)
            {
                player.Stop();
                player.Release();
                player = null;
            }
        }

    }
}