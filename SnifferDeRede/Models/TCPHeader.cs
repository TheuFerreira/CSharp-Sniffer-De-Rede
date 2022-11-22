namespace SnifferDeRede.Models
{
    public class TCPHeader
    {
        public TCPHeader() { }

        /// <summary>
        /// Sixteen bits for the source port number
        /// </summary>
        public ushort SourcePort { get; set; }

        /// <summary>
        /// Sixteen bits for the destination port number
        /// </summary>
        public ushort DestinationPort { get; set; }

        /// <summary>
        /// Thirty two bits for the sequence number
        /// </summary>
        public uint SequenceNumber { get; set; }

        /// <summary>
        /// Thirty two bits for the acknowledgement number
        /// </summary>
        public string AcknowledgementNumber { get; set; }

        /// <summary>
        /// Header length
        /// </summary>
        public byte HeaderLength { get; set; }

        /// <summary>
        /// Sixteen bits for the window size
        /// </summary>
        public ushort WindowSize { get; set; }

        /// <summary>
        /// The following sixteen contain the urgent pointer
        /// </summary>
        public string UrgentPointer { get; set; }

        /// <summary>
        /// The next sixteen bits hold the flags and the data offset
        /// </summary>
        public string Flags { get; set; }

        /// <summary>
        /// Sixteen bits for the checksum (checksum can be negative so taken as short)
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// Data carried by the TCP packet
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Message length = Total length of the TCP packet - Header length
        /// </summary>
        public ushort MessageLength { get; set; }

        /// <summary>
        /// If the port is equal to 53 then the underlying protocol is DNS
        /// Note: DNS can use either TCP or UDP thats why the check is done twice
        /// </summary>
        /// <returns></returns>
        public bool CheckIfIfDns()
        {
            return DestinationPort.ToString() == "53" || SourcePort.ToString() == "53";
        }

        public override string ToString()
        {
            return $"Source Port: {SourcePort} | Destination Port: {DestinationPort} | Sequence Number: {SequenceNumber} | Acknowledgement Number: {AcknowledgementNumber} | Header Length: {HeaderLength} | Window Size: {WindowSize} | Urgent Pointer: {UrgentPointer} | Flags: {Flags} | Checksum: {Checksum} | Data: {Data} | Message Length: {MessageLength}";
        }
    }
}