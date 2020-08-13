using AplicativoMeditacao.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AplicativoMeditacao.Interfaces
{
    public interface ICustomListDialog
    {
        Task<string> showCustomListDialog(Dictionary<string, string> listaDeItens, string audioAtual);
    }
}
