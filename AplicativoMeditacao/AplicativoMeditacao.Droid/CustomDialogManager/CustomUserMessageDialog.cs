
using Android.App;
using Android.Content;
using Android.Text;
using Android.Text.Util;
using Android.Views;
using Android.Widget;
using AplicativoMeditacao.Droid.CustomDialogManager;
using AplicativoMeditacao.Interfaces;
using Java.Lang;
using Xamarin.Forms;

[assembly: Dependency(typeof(CustomUserMessageDialog))]
namespace AplicativoMeditacao.Droid.CustomDialogManager
{
    class CustomUserMessageDialog : ICustomUserMessageDialog
    {
        public void openUserMessageDialog(string texto)
        {
            var context = Android.App.Application.Context;
            var inflater = context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            var view = inflater.Inflate(Resource.Layout.layoutCustomUserMessageDialog, null);

            var txtUserMessage = view.FindViewById<TextView>(Resource.Id.txtUserMsg);
            var btOk = view.FindViewById<Android.Widget.Button>(Resource.Id.btOk);
            
            txtUserMessage.TextFormatted = Html.FromHtml("<a href='https://www.google.com.br'>GoogleSearch</a>");
            txtUserMessage.MovementMethod = Android.Text.Method.LinkMovementMethod.Instance;
            //txtUserMessage.Text = "click: https://www.google.com.br/ ";

            //Linkify.AddLinks(txtUserMessage, MatchOptions.WebUrls);
            txtUserMessage.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);
            
            txtUserMessage.Clickable = true;

            AlertDialog.Builder builder = new AlertDialog.Builder(Forms.Context)
                .SetView(view);
            
            var alert = builder.Create();

            alert.SetCanceledOnTouchOutside(false);

            alert.Show();

            btOk.Click += (senders, args) =>
            {
                alert.Dismiss();
            };
        }
    }
}