using AplicativoMeditacao.ViewModel;
using System.Threading.Tasks;

namespace AplicativoMeditacao.Interfaces
{
    public interface ICustomDialog
    {
        void callCustomDialog(bool hummingMaximo, bool umdpMaximo, MeditacaoUMDPViewModel umdpvm);
    }
}
