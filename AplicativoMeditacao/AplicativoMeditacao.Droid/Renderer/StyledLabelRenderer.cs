using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using AplicativoMeditacao.Droid.Renderer;
using AplicativoMeditacao.Control;
using Android.Graphics;

[assembly: ExportRenderer(typeof(StyledLabel), typeof(StyledLabelRenderer))]
namespace AplicativoMeditacao.Droid.Renderer
{
    class StyledLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var styledLabel = (StyledLabel)Element;

            Control.TextAlignment = Android.Views.TextAlignment.Center;

            //Control.SetTypeface(null, TypefaceStyle.Bold);

        }
    }
}