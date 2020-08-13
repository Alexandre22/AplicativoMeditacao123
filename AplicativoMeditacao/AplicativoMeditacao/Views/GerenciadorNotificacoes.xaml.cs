using AplicativoMeditacao.Data;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.ViewModel;
using SQLite;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AplicativoMeditacao.Views
{
    public partial class GerenciadorNotificacoes : ContentPage
    {
        private GerenciadorNotificacoesViewModel gnvm;

        public GerenciadorNotificacoes(NotificacaoDAO notDao)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            gnvm = new GerenciadorNotificacoesViewModel(notDao, this);
            gnvm.Navigation = Navigation;
            
            BindingContext = gnvm;
            InitializeComponent();
        }
        
    }
}
