using AplicativoMeditacao.Data;
using AplicativoMeditacao.ViewModel;
using Xamarin.Forms;

namespace AplicativoMeditacao.Views
{
    public partial class TelaEspacoPresenca : ContentPage
    {
        public EspacoPresencaViewModel epvm;

        public TelaEspacoPresenca(ConteudoDaPaginaDAO dao)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            epvm = new EspacoPresencaViewModel(layoutConteudo, dao);
            BindingContext = epvm;
            
        }
    }
}
