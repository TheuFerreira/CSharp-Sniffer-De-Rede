using System.Net;

namespace SnifferDeRede.Models
{
    public class IPHeader
    {
        public IPHeader() { }

        /// <summary>
        /// Eight bits for version and header length
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Header length
        /// </summary>
        public ushort HeaderLength { get; set; }

        /// <summary>
        /// MessageLength = Total length of the datagram - Header length
        /// </summary>
        public ushort MessageLength { get; set; }

        /// <summary>
        /// Returns the differentiated services in hexadecimal format
        /// </summary>
        public string DifferentiatedServices { get; set; }

        /// <summary>
        /// Eight bits for flags and fragmentation offset
        /// </summary>
        public string Flags { get; set; }

        /// <summary>
        /// Eight bits for flags and fragmentation offset
        /// </summary>
        public int FragmentationOffset { get; set; }

        /// <summary>
        /// Eight bits for TTL (Time To Live)
        /// </summary>
        public byte TTL { get; set; }

        /// <summary>
        /// Eight bits for the underlying protocol
        /// </summary>
        public Protocol ProtocolType { get; set; }

        /// <summary>
        /// Sixteen bits containing the checksum of the header (checksum can be negative so taken as short)
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// Thirty two bit source IP Address
        /// </summary>
        public IPAddress SourceAddress { get; set; }

        /// <summary>
        /// Thirty two bit destination IP Address
        /// </summary>
        public IPAddress DestinationAddress { get; set; }

        /// <summary>
        /// Sixteen bits for total length of the datagram (header + message)
        /// </summary>
        public ushort TotalLength { get; set; }

        /// <summary>
        /// Sixteen bits for identification
        /// </summary>
        public ushort Identification { get; set; }

        /// <summary>
        /// Data carried by the datagram
        /// </summary>
        public byte[] Data { get; set; }

        public override string ToString()
        {
            return $"Version: {Version} | Header Length: {HeaderLength} | Message Length: {MessageLength} | Differentiated Services: {DifferentiatedServices} | Flags: {Flags} | Fragmentation Offset: {FragmentationOffset} | TTL: {TTL} | Protocol Type: {ProtocolType} | Checksum: {Checksum} | Source Address: {SourceAddress} | Destination Address: {DestinationAddress} | Total Length: {TotalLength} | Identification: {Identification} | Data: {Data}";
        }
    }
}
