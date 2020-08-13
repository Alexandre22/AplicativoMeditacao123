using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Ads;

namespace AplicativoMeditacao.Droid
{
    //[Activity(Label = "Um Minuto de Presença", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    [Activity(Label = "Um Minuto de Presença", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            MobileAds.Initialize(ApplicationContext, "ca-app-pub-6064842131511058/7391097929");

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            
            //nao deixa a tela apagar
            //this.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

        }

    }
}

