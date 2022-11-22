using SnifferDeRede.Models;
using SnifferDeRede.UseCases.Interface;
using System.IO;
using System.Net;

namespace SnifferDeRede.UseCases
{
    class CreateDnsHeaderCase : ICreateDnsHeaderCase
    {
        public DNSHeader Execute(byte[] byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            //First sixteen bits are for identification
            ushort usIdentification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next sixteen contain the flags
            ushort usFlags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Read the total numbers of questions in the quesion list
            ushort totalQuestions = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Read the total number of answers in the answer list
            ushort totalAnswerRRs = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Read the total number of entries in the authority list
            ushort totalAuthorityRRs = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Total number of entries in the additional resource record list
            ushort totalAdditionalRRs = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            DNSHeader dnsHeader = new DNSHeader
            {
                Identification = string.Format("0x{0:x2}", usIdentification),
                Flags = string.Format("0x{0:x2}", usFlags),
                TotalQuestions = totalQuestions,
                TotalAnswerRRs = totalAnswerRRs,
                TotalAuthorityRRs = totalAuthorityRRs,
                TotalAdditionalRRs = totalAdditionalRRs
            };

            binaryReader.Close();
            memoryStream.Close();

            binaryReader.Dispose();
            memoryStream.Dispose();

            return dnsHeader;
        }
    }
}
