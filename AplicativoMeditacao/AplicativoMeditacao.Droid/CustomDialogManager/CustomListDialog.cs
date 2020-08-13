using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.Droid.CustomDialogManager;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android.Media;

[assembly: Dependency(typeof(CustomListDialog))]
namespace AplicativoMeditacao.Droid.CustomDialogManager
{
    public class CustomListDialog : ICustomListDialog
    {
        private MediaPlayer player;

        public Task<string> showCustomListDialog(Dictionary<string, string> listaDeItens, string audioAtual)
        {
            var tcs = new TaskCompletionSource<string>();
            var audioSelecionado = audioAtual;
            
            var context = Android.App.Application.Context;

            var inflater = context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;

            var view = inflater.Inflate(Resource.Layout.layoutCustomListDialog, null);
            
            var radioGroup = view.FindViewById<RadioGroup>(Resource.Id.radioGroupList);
            var btOk = view.FindViewById<Android.Widget.Button>(Resource.Id.btOk);

            //linha inserida
            int espaco = 1;

            foreach (var item in listaDeItens)
            {
                var radioButton = new RadioButton(context)
                {
                    Text = item.Key,
                    TextSize = 20.0F
                };

                radioGroup.AddView(radioButton);

                if(espaco != 0 && (espaco%2) == 0)
                {
                    var space = new Space(context) { };

                    space.SetMinimumHeight(20);

                    radioGroup.AddView(space);
                }

                espaco++;

                if (item.Key.Equals(audioAtual))
                {
                    radioGroup.Check(radioButton.Id);
                }
            }

            radioGroup.CheckedChange += (sender, e) =>
            {
                RadioButton radioButton = view.FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);

                audioSelecionado = radioButton.Text;

                pararPlayer();

                player = new MediaPlayer();

                player.Completion += (s, args) =>
                {
                    player.Stop();
                    player.Release();
                    player = null;
                };

                var fd = global::Android.App.Application.Context.Assets.OpenFd(listaDeItens[audioSelecionado] + "Demo.mp3");
                player.Prepared += (send, events) =>
                {
                    player.Start();
                };
                player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
                player.Prepare();
            };

            AlertDialog.Builder builder = new AlertDialog.Builder(Forms.Context)
                .SetView(view);

            var alert = builder.Create();

            alert.SetCanceledOnTouchOutside(false);

            alert.Show();

            btOk.Click += (senders, args) =>
            {
                pararPlayer();
                tcs.SetResult(audioSelecionado);
                alert.Dismiss();
            };

            return tcs.Task;
        }

        private void pararPlayer()
        {
            if (player != null)
            {
                player?.Stop();
                player?.Release();
                player = null;
            }
        }
    }
}