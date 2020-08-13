using AplicativoMeditacao.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicativoMeditacao.Interfaces
{
    public interface IAudio
    {
        void tocarSomMeditacao(MeditacaoUMDPViewModel umdp);

        void pararMeditacao();
        void resumirMeditacao();
        void pausarMeditacao();
        void acrescentarMinuto();
        void reiniciaMeditacao();
    }
}
