using SnifferDeRede.Models;

namespace SnifferDeRede.UseCases.Interface
{
    public interface ICreateTCPHeaderCase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byBuffer">IPHeader.Data stores the data being carried by the IP datagram</param>
        /// <param name="nReceived">Length of the data field</param>
        /// <returns></returns>
        TCPHeader Execute(byte[] byBuffer, int nReceived);
    }
}
