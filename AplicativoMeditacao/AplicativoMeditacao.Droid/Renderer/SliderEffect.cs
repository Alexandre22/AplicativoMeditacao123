using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using AplicativoMeditacao.Droid.Renderer;

[assembly: ResolutionGroupName("EffectsSample")]
[assembly: ExportEffect(typeof(SliderEffect), "SliderEffect")]
namespace AplicativoMeditacao.Droid.Renderer
{
    public class SliderEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var seekBar = (SeekBar)Control;
            seekBar.ProgressDrawable.SetColorFilter(new PorterDuffColorFilter(Xamarin.Forms.Color.FromHex("#4729DA").ToAndroid(), PorterDuff.Mode.SrcIn));
            seekBar.Thumb.SetColorFilter(new PorterDuffColorFilter(Xamarin.Forms.Color.FromHex("#4729DA").ToAndroid(), PorterDuff.Mode.SrcIn));
            //E5EFC2
        }

        protected override void OnDetached()
        {
            // Use this method if you wish to reset the control to original state
        }
    }
}