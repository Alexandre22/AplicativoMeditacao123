using System.Collections.Generic;

namespace AplicativoMeditacao.Util
{
    public static class ListasDeSonsMeditacao
    {
        public static readonly Dictionary<string, string> listaDeSonsHumming = new Dictionary<string, string>()
        {
            { "Didgeridoo", "didgeridooHumming" },
            { "Silencioso", "semSom"}
        };

        public static readonly Dictionary<string, string> listaDeSonsUMP = new Dictionary<string, string>()
        {
            { "Percebendo", "percebendonivel5" },
            { "Sonzinho", "sonzinhonivel5" },
            { "Silencioso", "semSom" }
        };
        
    }
}
