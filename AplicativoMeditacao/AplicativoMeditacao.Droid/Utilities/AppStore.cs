using System;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.Droid.Utilities;
using Xamarin.Forms;
using Android.Content;

[assembly: Dependency(typeof(AppStore))]
namespace AplicativoMeditacao.Droid.Utilities
{
    public class AppStore : IOpenAppStore
    {
        public void openAppStore()
        {
            var context = Android.App.Application.Context;

            Intent googleAppStoreIntent;
            try
            {
                googleAppStoreIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + context.PackageName));
                googleAppStoreIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(googleAppStoreIntent);
            }
            catch (ActivityNotFoundException anfe)
            {
                googleAppStoreIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + context.PackageName));
                googleAppStoreIntent.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(googleAppStoreIntent);
            }
        }
    }
}