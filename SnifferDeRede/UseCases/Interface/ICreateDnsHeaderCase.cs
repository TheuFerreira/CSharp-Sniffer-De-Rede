using SnifferDeRede.Models;

namespace SnifferDeRede.UseCases.Interface
{
    public interface ICreateDnsHeaderCase
    {
        DNSHeader Execute(byte[] byBuffer, int nReceived);
    }
}
