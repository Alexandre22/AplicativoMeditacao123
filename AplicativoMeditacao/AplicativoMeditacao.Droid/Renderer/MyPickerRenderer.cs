using Android.Text;
using AplicativoMeditacao.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(MyPickerRenderer))]
namespace AplicativoMeditacao.Droid.Renderer
{
    class MyPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.InputType = InputTypes.TextFlagNoSuggestions;
                Control.SetHighlightColor(Android.Graphics.Color.Black);
            }

        }
    }
}