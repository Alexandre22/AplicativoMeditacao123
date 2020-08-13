using AplicativoMeditacao.Model;
using AplicativoMeditacao.ViewModel;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace AplicativoMeditacao.Views
{
    public partial class Orientacoes : ContentPage
    {
        public OrientacoesViewModel ovm;

        public Orientacoes(PaginaOrientacao pagina, ObservableCollection<PaginaOrientacao> topico)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            ovm = new OrientacoesViewModel(layoutConteudo, layoutPageButtons, pagina, topico);
            ovm.Navigation = Navigation;
            BindingContext = ovm;
            
            Disappearing += (sender, e) =>
            {
                if(SoundPlayer.SoundPlayerDemonstration.player != null)
                {
                    SoundPlayer.SoundPlayerDemonstration.player.pararMeditacao();
                    SoundPlayer.SoundPlayerDemonstration.player = null;
                }
                
            };

        }
        
        
    }
}
