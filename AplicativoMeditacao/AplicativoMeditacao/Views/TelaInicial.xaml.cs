using AplicativoMeditacao.Data;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.ViewModel;
using SQLite;
using System;

using Xamarin.Forms;

namespace AplicativoMeditacao.Views
{
    public partial class TelaInicial : ContentPage
    {
        TelaInicialViewModel tivm;
        
        public TelaInicial()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            tivm = new TelaInicialViewModel(this);
            tivm.Navigation = Navigation;
            BindingContext = tivm;
        }

    }
}
