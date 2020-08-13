using AplicativoMeditacao.Data;
using AplicativoMeditacao.ViewModel;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AplicativoMeditacao.Views
{
    public partial class AntesMeditacao : ContentPage
    {
        public AntesMeditacaoViewModel amvm;
        
        public AntesMeditacao(MeditacaoDAO dao)
        {
            InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);
            amvm = new AntesMeditacaoViewModel(dao);
            amvm.Navigation = Navigation;
            BindingContext = amvm;
            
            xslider.Effects.Add(Effect.Resolve("EffectsSample.SliderEffect"));
            xsliderHumming.Effects.Add(Effect.Resolve("EffectsSample.SliderEffect"));
        }
        
        protected override void OnAppearing()
        {
            //base.OnAppearing();
            amvm.ajustaLimitesMaximosDeTempos();
            amvm.ajustarTempoTotalMeditado();
        }

    }
}
