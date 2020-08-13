using AplicativoMeditacao.Interfaces;
using System;
using System.IO;
using System.Net;
//using Windows.Data.Xml.Dom;
using Xamarin.Forms;

namespace AplicativoMeditacao.Util
{
    public static class LeitorConteudoDeSite
    {

        public static string ExtrairVersaoDoTexto(string texto)
        {
            string versao = "";

            int comecoVersao = texto.IndexOf('{');
            
            while(!(texto.Substring(comecoVersao, 7).Equals("{versão")))
            {
                texto = texto.Substring(comecoVersao + 1);
                comecoVersao = texto.IndexOf('{');
            }

            texto = texto.Substring(comecoVersao + 1);

            int fimVersao = texto.IndexOf('}');

            texto = texto.Substring(0, fimVersao + 1);

            texto = texto.Replace("versão ", "");
            texto = texto.Replace("}", "");

            versao = texto;

            return versao;
        }

        public static void retornaConteudoSite(string site, IRespostaWeb page)
        {
            string texto = "";
            
            Uri uri = new Uri(site);
            WebRequest request = WebRequest.Create(uri);
            request.BeginGetResponse((result) =>
            {
                try
                {
                    Stream stream = request.EndGetResponse(result).GetResponseStream();
                    StreamReader reader = new StreamReader(stream);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        texto = reader.ReadToEnd();

                        try
                        {
                            texto = FiltraConteudoPagina(texto);
                            texto = texto.Replace("<p>", "");
                            texto = texto.Replace("\n", "");
                            texto = texto.Replace("&#8230;", "...");
                            texto = texto.Replace("&#8220;", "''");
                            texto = texto.Replace("&#8221;", "''");
                            texto = texto.Replace("&#8216;", "'");
                            texto = texto.Replace("&#8217;", "'");
                            texto = texto.Replace("&#8211;", "-");
                            texto = texto.Replace("[enter]", " ");
                            page.respostaWeb(texto);
                        }
                        catch (Exception e)
                        {
                            var message_error = e.Message;
                            page.respostaWeb("Erro!");
                        }

                    });
                }
                catch (Exception exc)
                {
                    page.respostaWeb("Erro!");
                }
            }, null);

        }

        private static string FiltraConteudoPagina(string texto)
        {
            string textoFormatado = texto;

            int posicaoComecoTexto = textoFormatado.IndexOf('<');

            while (!(textoFormatado.Substring(posicaoComecoTexto, 3).Equals("<p>")))
            {
                textoFormatado = textoFormatado.Substring(posicaoComecoTexto + 1);
                posicaoComecoTexto = textoFormatado.IndexOf('<');
            }

            int posicaoFinalTexto = textoFormatado.IndexOf('{');

            while (!(textoFormatado.Substring(posicaoFinalTexto, 5).Equals("{fim}")))
            {
                posicaoFinalTexto++;
            }

            textoFormatado = textoFormatado.Substring(posicaoComecoTexto, posicaoFinalTexto);

            return textoFormatado;
        }
        

    }
}
