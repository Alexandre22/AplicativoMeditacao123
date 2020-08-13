using AplicativoMeditacao.Control;
using AplicativoMeditacao.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Gms.Ads;
using Android.Widget;
using System;

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace AplicativoMeditacao.Droid.Renderer
{
    public class AdMobRenderer : ViewRenderer<AdMobView, AdView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AdMobView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var ad = new AdView(Forms.Context);
                ad.AdSize = AdSize.Banner;
                ad.AdUnitId = "ca-app-pub-6064842131511058/7391097929";

                //var adParams = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
                //ad.LayoutParameters = adParams;

                var requestbuilder = new AdRequest.Builder();
                ad.LoadAd(requestbuilder.Build());

                SetNativeControl(ad);

            }
        }
    }
}