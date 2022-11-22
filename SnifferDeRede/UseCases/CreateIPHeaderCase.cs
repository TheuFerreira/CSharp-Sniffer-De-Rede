using SnifferDeRede.Models;
using SnifferDeRede.UseCases.Interface;
using System;
using System.IO;
using System.Net;

namespace SnifferDeRede.UseCases
{
    public class CreateIPHeaderCase : ICreateIPHeaderCase
    {
        private readonly byte[] byIPData = new byte[4096];

        public IPHeader Execute(byte[] byBuffer, int nReceived)
        {
            //Create MemoryStream out of the received bytes
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            //Next we create a BinaryReader out of the MemoryStream
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            //The first eight bits of the IP header contain the version and
            //header length so we read them
            byte byVersionAndHeaderLength = binaryReader.ReadByte();

            //The next eight bits contain the Differentiated services
            byte byDifferentiatedServices = binaryReader.ReadByte();

            //Next eight bits hold the total length of the datagram
            ushort usTotalLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next sixteen have the identification bytes
            ushort usIdentification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next sixteen bits contain the flags and fragmentation offset
            ushort usFlagsAndOffset = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next eight bits have the TTL value
            byte byTTL = binaryReader.ReadByte();

            //Next eight represnts the protocol encapsulated in the datagram
            byte byProtocol = binaryReader.ReadByte();

            //Next sixteen bits contain the checksum of the header
            short sChecksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next thirty two bits have the source IP address
            uint uiSourceIPAddress = (uint)(binaryReader.ReadInt32());

            //Next thirty two hold the destination IP address
            uint uiDestinationIPAddress = (uint)(binaryReader.ReadInt32());

            //Now we calculate the header length

            byte byHeaderLength = byVersionAndHeaderLength;
            //The last four bits of the version and header length field contain the
            //header length, we perform some simple binary airthmatic operations to
            //extract them
            byHeaderLength <<= 4;
            byHeaderLength >>= 4;
            //Multiply by four to get the exact header length
            byHeaderLength *= 4;

            //Copy the data carried by the data gram into another array so that
            //according to the protocol being carried in the IP datagram
            Array.Copy(byBuffer,
                       byHeaderLength,  //start copying from the end of the header
                       byIPData, 0,
                       usTotalLength - byHeaderLength);

            IPHeader ipHeader = new IPHeader
            {
                Version = GetIPVersion(byVersionAndHeaderLength),
                HeaderLength = byHeaderLength,
                MessageLength = (ushort)(usTotalLength - byHeaderLength),
                DifferentiatedServices = string.Format("0x{0:x2} ({1})", byDifferentiatedServices, byDifferentiatedServices),
                Flags = GetFlag(usFlagsAndOffset),
                FragmentationOffset = GetFragmentationOffset(usFlagsAndOffset),
                TTL = byTTL,
                ProtocolType = GetProtocolType(byProtocol),
                Checksum = string.Format("0x{0:x2}", sChecksum),
                SourceAddress = new IPAddress(uiSourceIPAddress),
                DestinationAddress = new IPAddress(uiDestinationIPAddress),
                TotalLength = usTotalLength,
                Identification = usIdentification,
                Data = byIPData,
            };

            return ipHeader;
        }

        private string GetIPVersion(byte byVersionAndHeaderLength)
        {
            //Calculate the IP version

            //The four bits of the IP header contain the IP version
            if ((byVersionAndHeaderLength >> 4) == 4)
            {
                return "IP v4";
            }
            else if ((byVersionAndHeaderLength >> 4) == 6)
            {
                return "IP v6";
            }

            return "Unknown";
        }

        private string GetFlag(ushort usFlagsAndOffset)
        {
            //The first three bits of the flags and fragmentation field 
            //represent the flags (which indicate whether the data is 
            //fragmented or not)
            int nFlags = usFlagsAndOffset >> 13;
            if (nFlags == 2)
            {
                return "Don't fragment";
            }
            else if (nFlags == 1)
            {
                return "More fragments to come";
            }

            return nFlags.ToString();
        }

        private int GetFragmentationOffset(ushort usFlagsAndOffset)
        {
            //The last thirteen bits of the flags and fragmentation field 
            //contain the fragmentation offset
            int nOffset = usFlagsAndOffset << 3;
            nOffset >>= 3;

            return nOffset;
        }

        private Protocol GetProtocolType(byte byProtocol)
        {
            //The protocol field represents the protocol in the data portion
            //of the datagram
            if (byProtocol == 6)        //A value of six represents the TCP protocol
            {
                return Protocol.TCP;
            }
            else if (byProtocol == 17)  //Seventeen for UDP
            {
                return Protocol.UDP;
            }
            else
            {
                return Protocol.Unknown;
            }
        }
    }
}
