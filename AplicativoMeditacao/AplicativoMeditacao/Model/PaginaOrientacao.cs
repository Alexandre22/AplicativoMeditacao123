using System.Collections.Generic;
using Xamarin.Forms;

namespace AplicativoMeditacao.Model
{
    public class PaginaOrientacao
    {
        public FormattedString Titulo { get; set; }
        public List<string> Texto { get; set; }
        public int Pagina { get; set; }

        public PaginaOrientacao()
        {
        }

        public PaginaOrientacao(FormattedString title, List<string> text, int page)
        {
            Titulo = title;
            Texto = text;
            Pagina = page;
        }
    }
}
