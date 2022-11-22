using SnifferDeRede.Models;

namespace SnifferDeRede.UseCases.Interface
{
    public interface ICreateIPHeaderCase
    {
        IPHeader Execute(byte[] byBuffer, int nReceived);
    }
}
