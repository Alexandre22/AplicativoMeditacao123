using AplicativoMeditacao.Data;
using AplicativoMeditacao.ViewModel;
using Xamarin.Forms;

namespace AplicativoMeditacao.Views
{
    public partial class Creditos : ContentPage
    {
        public CreditosViewModel cvm;

        public Creditos(ConteudoDaPaginaDAO dao)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            cvm = new CreditosViewModel(layoutConteudo, dao);
            BindingContext = cvm;
            
        }
    }
}
