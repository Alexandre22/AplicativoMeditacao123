using AplicativoMeditacao.Data;
using AplicativoMeditacao.Model;
using AplicativoMeditacao.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AplicativoMeditacao.Views
{
    public partial class SalvaNotificacao : ContentPage
    {
        private SalvaNotificacaoViewModel snvm;

        public SalvaNotificacao(NotificacaoDAO dao)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            snvm = new SalvaNotificacaoViewModel(dao);
            snvm.Navigation = Navigation;
            BindingContext = snvm;
            InitializeComponent();
        }

        public SalvaNotificacao(NotificacaoDAO dao, Notificacao notificacao)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            snvm = new SalvaNotificacaoViewModel(dao, notificacao);
            snvm.Navigation = Navigation;
            BindingContext = snvm;
            InitializeComponent();
        }
    }
}
