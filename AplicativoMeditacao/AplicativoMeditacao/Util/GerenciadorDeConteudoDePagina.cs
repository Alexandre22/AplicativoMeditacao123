using AplicativoMeditacao.Control;
using AplicativoMeditacao.Interfaces;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AplicativoMeditacao.Util
{
    public static class GerenciadorDeConteudoDePagina
    {

        public static void criarConteudo(StackLayout layout, List<string> listaDeConteudo)
        {
            FontAttributes atributoDaFonte = FontAttributes.None;
            double tamanhoDaFonte = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

            foreach (string linha in listaDeConteudo)
            {
                if (linha.Equals("[botao]"))
                {
                    StyledButton button = new StyledButton
                    {
                        Text = "Áudio Demonstrativo",
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        Style = (Style)Application.Current.Resources["buttonStyle1"],
                        FontAttributes = FontAttributes.Bold
                    };

                    button.Clicked += TocarAudio;
                    layout.Children.Add(button);
                }
                else if (linha.Contains("[link]"))
                {
                    string linhaLink = linha;
                    linhaLink = linhaLink.Replace("[link]", "");
                    linhaLink = linhaLink.Replace("[/link]", "");

                    var label = new Label()
                    {
                        Text = linhaLink,
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.FromHex("#2D12DB") //8E97E1
                    };

                    string enderecoSite = label.Text.Trim();
                    
                    if (!(enderecoSite.Substring(0, 4).Equals("http")))
                    {
                        enderecoSite = enderecoSite.Insert(0,"http://");
                    }
                    
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => {
                        try
                        {
                            Device.OpenUri(new Uri(enderecoSite));
                        }
                        catch (Exception exc)
                        {
                            System.Diagnostics.Debug.WriteLine("ERRO: " + exc.Message);
                        }
                    };
                    label.GestureRecognizers.Add(tapGestureRecognizer);

                    layout.Children.Add(label);
                }
                else
                {
                    var label = new StyledLabel()
                    {
                        Style = (Style)Application.Current.Resources["labelStyle1"]
                    };
                    var s = new FormattedString();
                    if (linha.Contains("[b]") || linha.Contains("[i]") || linha.Contains("[small]") || linha.Contains("[large]"))
                    {
                        string linhaComAtributos = linha;
                        while (linhaComAtributos.Contains("[b]") || linhaComAtributos.Contains("[i]") || linhaComAtributos.Contains("[small]") || linhaComAtributos.Contains("[/b]") || linhaComAtributos.Contains("[/i]") || linhaComAtributos.Contains("[/small]") || linhaComAtributos.Contains("[large]") || linhaComAtributos.Contains("[/large]"))
                        {
                            string trecho = "";

                            int posicaoAtributoMaisProximo = linhaComAtributos.IndexOf('[');

                            if (posicaoAtributoMaisProximo != 0)
                            {
                                trecho = linhaComAtributos.Substring(0, posicaoAtributoMaisProximo);
                                linhaComAtributos = linhaComAtributos.Substring(posicaoAtributoMaisProximo);
                            }
                            else
                            {
                                if (linhaComAtributos.Substring(0, 3).Equals("[b]"))
                                {
                                    atributoDaFonte = FontAttributes.Bold;
                                    linhaComAtributos = linhaComAtributos.Substring(3);
                                }
                                else if (linhaComAtributos.Substring(0, 3).Equals("[i]"))
                                {
                                    atributoDaFonte = FontAttributes.Italic;
                                    linhaComAtributos = linhaComAtributos.Substring(3);
                                }
                                else if (linhaComAtributos.Substring(0, 4).Equals("[/b]"))
                                {
                                    atributoDaFonte = FontAttributes.None;
                                    linhaComAtributos = linhaComAtributos.Substring(4);
                                }
                                else if (linhaComAtributos.Substring(0, 4).Equals("[/i]"))
                                {
                                    atributoDaFonte = FontAttributes.None;
                                    linhaComAtributos = linhaComAtributos.Substring(4);
                                }
                                else if (linhaComAtributos.Contains("[small]") || linhaComAtributos.Contains("[/small]"))
                                {
                                    if (linhaComAtributos.Substring(0, 7).Equals("[small]"))
                                    {
                                        tamanhoDaFonte = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                                        linhaComAtributos = linhaComAtributos.Substring(7);
                                    }
                                    if (linhaComAtributos.Substring(0, 8).Equals("[/small]"))
                                    {
                                        tamanhoDaFonte = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                                        linhaComAtributos = linhaComAtributos.Substring(8);
                                    }
                                }
                                else if (linhaComAtributos.Contains("[large]") || linhaComAtributos.Contains("[/large]"))
                                {
                                    if (linhaComAtributos.Substring(0, 7).Equals("[large]"))
                                    {
                                        tamanhoDaFonte = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                                        linhaComAtributos = linhaComAtributos.Substring(7);
                                    }
                                    if (linhaComAtributos.Substring(0, 8).Equals("[/large]"))
                                    {
                                        tamanhoDaFonte = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                                        linhaComAtributos = linhaComAtributos.Substring(8);
                                    }
                                }
                                else
                                {
                                    trecho = "[";
                                    linhaComAtributos = linhaComAtributos.Substring(1);
                                }
                            }

                            s.Spans.Add(new Span { Text = trecho + "", FontSize = tamanhoDaFonte, FontAttributes = atributoDaFonte });
                        }
                        s.Spans.Add(new Span { Text = linhaComAtributos, FontSize = tamanhoDaFonte, FontAttributes = atributoDaFonte });
                    }
                    else
                    {
                        s.Spans.Add(new Span { Text = linha, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) });
                    }
                    label.FormattedText = s;
                    layout.Children.Add(label);
                }
            }
        }

        static void TocarAudio(object sender, EventArgs e)
        {
            SoundPlayer.SoundPlayerDemonstration.player = DependencyService.Get<IAudioDemo>();
            SoundPlayer.SoundPlayerDemonstration.player.tocarDemonstracao();
        }

    }
}
