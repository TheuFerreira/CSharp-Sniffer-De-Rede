using SnifferDeRede.Models;
using SnifferDeRede.UseCases.Interface;
using System;
using System.IO;
using System.Net;

namespace SnifferDeRede.UseCases
{
    public class CreateTCPHeaderCase : ICreateTCPHeaderCase
    {
        private readonly byte[] byTCPData = new byte[4096];

        public TCPHeader Execute(byte[] byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            ushort usSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            ushort usDestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            uint uiSequenceNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
            uint uiAcknowledgementNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
            ushort usDataOffsetAndFlags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            ushort usWindow = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            short sChecksum = (short)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            ushort usUrgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The data offset indicates where the data begins, so using it we calculate the header length
            byte byHeaderLength = (byte)(usDataOffsetAndFlags >> 12);
            byHeaderLength *= 4;

            //Message length = Total length of the TCP packet - Header length
            ushort usMessageLength = (ushort)(nReceived - byHeaderLength);

            //Copy the TCP data into the data buffer
            Array.Copy(byBuffer, byHeaderLength, byTCPData, 0, nReceived - byHeaderLength);

            TCPHeader tcpHeader = new TCPHeader
            {
                SourcePort = usSourcePort,
                DestinationPort = usDestinationPort,
                SequenceNumber = uiSequenceNumber,
                AcknowledgementNumber = GetAcknowledgementNumber(usDataOffsetAndFlags, uiAcknowledgementNumber),
                HeaderLength = byHeaderLength,
                WindowSize = usWindow,
                UrgentPointer = GetUrgentPointer(usDataOffsetAndFlags, usUrgentPointer),
                Flags = GetFlags(usDataOffsetAndFlags),
                Checksum = string.Format("0x{0:x2}", sChecksum),
                Data = byTCPData,
                MessageLength = usMessageLength,
            };

            binaryReader.Close();
            memoryStream.Close();

            binaryReader.Dispose();
            memoryStream.Dispose();

            return tcpHeader;
        }

        private string GetAcknowledgementNumber(ushort usDataOffsetAndFlags, uint uiAcknowledgementNumber)
        {
            //If the ACK flag is set then only we have a valid value in
            //the acknowlegement field, so check for it beore returning 
            //anything
            if ((usDataOffsetAndFlags & 0x10) != 0)
                return uiAcknowledgementNumber.ToString();
            return "";
        }

        private string GetUrgentPointer(ushort usDataOffsetAndFlags, ushort usUrgentPointer)
        {
            //If the URG flag is set then only we have a valid value in
            //the urgent pointer field, so check for it beore returning 
            //anything
            if ((usDataOffsetAndFlags & 0x20) != 0)
                return usUrgentPointer.ToString();
            return "";
        }

        private string GetFlags(ushort usDataOffsetAndFlags)
        {
            //The last six bits of the data offset and flags contain the
            //control bits

            //First we extract the flags
            int nFlags = usDataOffsetAndFlags & 0x3F;

            string strFlags = string.Format("0x{0:x2} (", nFlags);

            //Now we start looking whether individual bits are set or not
            if ((nFlags & 0x01) != 0)
            {
                strFlags += "FIN, ";
            }
            if ((nFlags & 0x02) != 0)
            {
                strFlags += "SYN, ";
            }
            if ((nFlags & 0x04) != 0)
            {
                strFlags += "RST, ";
            }
            if ((nFlags & 0x08) != 0)
            {
                strFlags += "PSH, ";
            }
            if ((nFlags & 0x10) != 0)
            {
                strFlags += "ACK, ";
            }
            if ((nFlags & 0x20) != 0)
            {
                strFlags += "URG";
            }
            strFlags += ")";

            if (strFlags.Contains("()"))
            {
                strFlags = strFlags.Remove(strFlags.Length - 3);
            }
            else if (strFlags.Contains(", )"))
            {
                strFlags = strFlags.Remove(strFlags.Length - 3, 2);
            }

            return strFlags;
        }
    }
}
