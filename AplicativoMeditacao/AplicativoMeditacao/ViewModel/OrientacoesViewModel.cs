using AplicativoMeditacao.Control;
using AplicativoMeditacao.Interfaces;
using AplicativoMeditacao.Model;
using AplicativoMeditacao.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AplicativoMeditacao.ViewModel
{
    public class OrientacoesViewModel
    {
        public PaginaOrientacao Pagina { get; set; }
        Orientacoes orientacoes;
        public ObservableCollection<PaginaOrientacao> Topico;

        public INavigation Navigation { get; set; }
        
        private StackLayout layoutPageButtons;

        private StackLayout layoutConteudo;
        
        public OrientacoesViewModel(StackLayout layoutContent, StackLayout layoutButtons, PaginaOrientacao pagina, ObservableCollection<PaginaOrientacao> topico)
        {
            Pagina = pagina;
            Topico = topico;
            layoutPageButtons = layoutButtons;
            layoutConteudo = layoutContent;

            criarConteudo();
            criarBotoesdaPagina();
        }
        
        public void criarConteudo()
        {
            Util.GerenciadorDeConteudoDePagina.criarConteudo(layoutConteudo, Pagina.Texto);
        }
        
        public void criarBotoesdaPagina()
        {

            /*if(Pagina.Pagina != 0)
            {
                Button botaoVoltar = geradorDeBotao("<");

                botaoVoltar.Clicked += abreTopicoAnterior;

                layoutPageButtons.Children.Add(botaoVoltar);
            }*/
            
            for (int i=1; i<Topico.Count + 1; i++)
            {
                if(i != (Pagina.Pagina + 1))
                {
                    StyledButton button = geradorDeBotao(i + "");
                    
                    button.Clicked += TrocarPagina;

                    layoutPageButtons.Children.Add(button);
                } else
                {
                    Label label = new Label
                    {
                        Text = i + "",
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Color.FromHex("#000000"),
                        FontAttributes = FontAttributes.Bold
                    };

                    layoutPageButtons.Children.Add(label);
                }
            }

            /*if(Pagina.Pagina != (Topico.Count - 1))
            {
                Button botaoProximo = geradorDeBotao(">");

                botaoProximo.Clicked += abreProximoTopico;

                layoutPageButtons.Children.Add(botaoProximo);
            }*/
            
        }

        public StyledButton geradorDeBotao(string textoBotao)
        {
            StyledButton botao = new StyledButton
            {
                Text = textoBotao,
                Font = Font.SystemFontOfSize(NamedSize.Small),
                WidthRequest = 30,
                HeightRequest = 45,
                Style = (Style)Application.Current.Resources["buttonStyle1"],
                FontAttributes = FontAttributes.Bold
            };

            return botao;
        }

        void TrocarPagina(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int pgn = Int32.Parse(btn.Text) - 1;

            PaginaOrientacao t = Topico[pgn];
            orientacoes = new Orientacoes(t, Topico);
            Navigation.PopAsync();
            Navigation.PushAsync(orientacoes);
        }
        
        public void abreProximoTopico(Object sender, EventArgs e)
        {
            PaginaOrientacao t = Topico[Pagina.Pagina + 1];
            orientacoes = new Orientacoes(t, Topico);
            Navigation.PopAsync();
            Navigation.PushAsync(orientacoes);
        }

        public void abreTopicoAnterior(Object sender, EventArgs e)
        {
            PaginaOrientacao t = Topico[Pagina.Pagina - 1];
            orientacoes = new Orientacoes(t, Topico);
            Navigation.PopAsync();
            Navigation.PushAsync(orientacoes);
        }
        
        void TocarAudio(object sender, EventArgs e)
        {
            SoundPlayer.SoundPlayerDemonstration.player = DependencyService.Get<IAudioDemo>();
            SoundPlayer.SoundPlayerDemonstration.player.tocarDemonstracao();
        }

    }
}
