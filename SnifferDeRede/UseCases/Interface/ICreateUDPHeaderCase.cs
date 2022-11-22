using SnifferDeRede.Models;

namespace SnifferDeRede.UseCases.Interface
{
    public interface ICreateUDPHeaderCase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer">IPHeader.Data stores the data being carried by the IP datagram</param>
        /// <param name="nReceived">Length of the data field</param>
        /// <returns></returns>
        UDPHeader Execute(byte[] buffer, int nReceived);
    }
}
