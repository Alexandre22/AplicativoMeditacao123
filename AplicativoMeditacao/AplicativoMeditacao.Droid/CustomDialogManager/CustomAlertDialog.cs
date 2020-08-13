
using Android.App;
using Android.Content;
using Android.Views;
using AplicativoMeditacao.Interfaces;
using Xamarin.Forms;
using AplicativoMeditacao.Droid.CustomDialogManager;
using AplicativoMeditacao.Droid.SQLite;
using AplicativoMeditacao.Data;
using AplicativoMeditacao.Model;
using AplicativoMeditacao.ViewModel;
using AplicativoMeditacao.Droid.Controllers;

[assembly: Dependency(typeof(CustomAlertDialog))]
namespace AplicativoMeditacao.Droid.CustomDialogManager
{
    public class CustomAlertDialog : ICustomDialog
    {

        private bool hummingConcluido;
        private bool umdpConcluido;
        private Android.Widget.Button btHummingSim;
        private Android.Widget.Button btHummingNao;
        private Android.Widget.Button btUMDPSim;
        private Android.Widget.Button btUMDPNao;
        private AlertDialog alert;
        MeditacaoUMDPViewModel umdp;

        public void callCustomDialog(bool hummingMaximo, bool umdpMaximo, MeditacaoUMDPViewModel umdpvm)
        {
            SQLite_Android sqlite = new SQLite_Android();
            MeditacaoDAO dao = new MeditacaoDAO(sqlite.GetConnection());
            Meditacao tempos = dao.TemposMeditacao[0];
            umdp = umdpvm;

            hummingConcluido = !hummingMaximo;
            umdpConcluido = !umdpMaximo;
            
            var context = Android.App.Application.Context;

            var inflater = context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;

            var view = inflater.Inflate(Resource.Layout.layout1, null);

            var layoutHumming = view.FindViewById<Android.Widget.LinearLayout>(Resource.Id.layoutHumming);
            var layoutUMDP = view.FindViewById<Android.Widget.LinearLayout>(Resource.Id.layoutUMDP);
            var layoutPrincipal = view.FindViewById<Android.Widget.LinearLayout>(Resource.Id.layoutPrincipal);
            
            btHummingSim = view.FindViewById<Android.Widget.Button>(Resource.Id.btHummingSim);
            btHummingNao = view.FindViewById<Android.Widget.Button>(Resource.Id.btHummingNao);

            btUMDPSim = view.FindViewById<Android.Widget.Button>(Resource.Id.btUMDPSim);
            btUMDPNao = view.FindViewById<Android.Widget.Button>(Resource.Id.btUMDPNao);
            
            btHummingSim.Click += (sender, args) =>
            {
                tempos.TempoMaximoHumming += 5;
                dao.AtualizarMeditacao(tempos);
                acoesBotoesHumming();
            };

            btHummingNao.Click += (sender, args) =>
            {
                acoesBotoesHumming();
            };

            btUMDPSim.Click += (sender, args) =>
            {
                tempos.TempoMaximoUMDP += 5;
                dao.AtualizarMeditacao(tempos);
                acoesBotoesUMDP();
            };

            btUMDPNao.Click += (sender, args) =>
            {
                acoesBotoesUMDP();
            };

            if (!hummingMaximo)
            {
                layoutPrincipal.RemoveView(layoutHumming);
                //layoutHumming.Visibility = ViewStates.Invisible;
            }
            if (!umdpMaximo)
            {
                layoutPrincipal.RemoveView(layoutUMDP);
                //layoutUMDP.Visibility = ViewStates.Invisible;
            }

            AlertDialog.Builder builder = new AlertDialog.Builder(Forms.Context)
                .SetView(view);

            alert = builder.Create();

            alert.SetCanceledOnTouchOutside(false);

            alert.Show();
        }

        private void acoesBotoesHumming()
        {
            btHummingSim.Enabled = false;
            btHummingNao.Enabled = false;

            hummingConcluido = true;
            
            if(hummingConcluido && umdpConcluido)
            {
                alert.Dismiss();
                umdp.voltarPagina();
                var wc = new WindowController();
                wc.apagarTela();
            }
        }

        private void acoesBotoesUMDP()
        {
            btUMDPSim.Enabled = false;
            btUMDPNao.Enabled = false;
            
            umdpConcluido = true;

            if (hummingConcluido && umdpConcluido)
            {
                alert.Dismiss();
                umdp.voltarPagina();
                var wc = new WindowController();
                wc.apagarTela();
            }
        }
    }
}