namespace SnifferDeRede.Models
{
    public class UDPHeader
    {
        public UDPHeader() { }

        /// <summary>
        /// The first sixteen bits contain the source port
        /// </summary>
        public ushort SourcePort { get; set; }

        /// <summary>
        /// The next sixteen bits contain the destination port
        /// </summary>
        public ushort DestinationPort { get; set; }

        /// <summary>
        /// The next sixteen bits contain the length of the UDP packet
        /// </summary>
        public ushort Length { get; set; }

        /// <summary>
        /// The next sixteen bits contain the checksum
        /// (checksum can be negative so taken as short) 
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// Data carried by the UDP packet
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// If the port is equal to 53 then the underlying protocol is DNS
        /// Note: DNS can use either TCP or UDP thats why the check is done twice
        /// </summary>
        /// <returns></returns>
        public bool CheckIfIsDns()
        {
            return DestinationPort.ToString() == "53" || SourcePort.ToString() == "53";
        }

        public override string ToString()
        {
            return $"Source Port: {SourcePort} | Destination Port: {DestinationPort} | Length: {Length} | Checksum: {Checksum} | Data: {Data}";
        }
    }
}