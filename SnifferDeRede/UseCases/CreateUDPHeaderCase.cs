using SnifferDeRede.Models;
using SnifferDeRede.UseCases.Interface;
using System;
using System.IO;
using System.Net;

namespace SnifferDeRede.UseCases
{
    public class CreateUDPHeaderCase : ICreateUDPHeaderCase
    {
        private byte[] byUDPData = new byte[4096];

        public UDPHeader Execute(byte[] buffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(buffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            //The first sixteen bits contain the source port
            ushort usSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The next sixteen bits contain the destination port
            ushort usDestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The next sixteen bits contain the length of the UDP packet
            ushort usLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The next sixteen bits contain the checksum
            short sChecksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Copy the data carried by the UDP packet into the data buffer
            Array.Copy(buffer,
                       8,               //The UDP header is of 8 bytes so we start copying after it
                       byUDPData,
                       0,
                       nReceived - 8);

            UDPHeader udpHeader = new UDPHeader
            {
                SourcePort = usSourcePort,
                DestinationPort = usDestinationPort,
                Length = usLength,
                Checksum = string.Format("0x{0:x2}", sChecksum),
                Data = byUDPData,
            };

            binaryReader.Close();
            memoryStream.Close();

            binaryReader.Dispose();
            memoryStream.Dispose();

            return udpHeader;
        }
    }
}
