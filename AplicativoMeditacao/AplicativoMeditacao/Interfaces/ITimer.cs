using AplicativoMeditacao.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicativoMeditacao.Interfaces
{
    public interface ITimer
    {
        void startServiceTimer(MeditacaoUMDPViewModel umdp);
        void stopTimer();
        void increaseTimer();
        void resetTimer();
        void pauseTimer();
        void resumeTimer();

    }
}
