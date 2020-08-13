
using System;
using AplicativoMeditacao.Interfaces;

namespace AplicativoMeditacao.Util
{
    public class VerificadorDeVersao : IRespostaWeb
    {
        ICurrentAppVersion pagina;

        public VerificadorDeVersao(ICurrentAppVersion page)
        {
            pagina = page;
        }
        
        public void GetCurrentVersion(string link)
        {
            Util.LeitorConteudoDeSite.retornaConteudoSite(link, this);
        }
        
        public void respostaWeb(string texto)
        {
            if (texto.Equals("Erro!"))
            {
                pagina.GetCurrentAppVersion(texto);
            }
            else
            {
                var versao = Util.LeitorConteudoDeSite.ExtrairVersaoDoTexto(texto);
                pagina.GetCurrentAppVersion(versao);
            }
        }
    }
}
