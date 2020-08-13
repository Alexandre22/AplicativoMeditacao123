using AplicativoMeditacao.Data;
using AplicativoMeditacao.ViewModel;

using Xamarin.Forms;

namespace AplicativoMeditacao.Views
{
    public partial class MeditacaoUMDP : ContentPage
    {
        public MeditacaoUMDPViewModel umdpvm;
        public MeditacaoUMDP(string audioHumming, string audioUMP, int repeticoes, int repeticoesHumming, MeditacaoDAO dao) : base()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            umdpvm = new MeditacaoUMDPViewModel(audioHumming, audioUMP, repeticoes, repeticoesHumming, this, dao);
            umdpvm.Navigation = Navigation;
            BindingContext = umdpvm;
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            umdpvm.pararMeditacao();

            this.Navigation.PopAsync();
            return true;
        }

    }
}
