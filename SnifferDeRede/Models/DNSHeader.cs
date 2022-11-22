namespace SnifferDeRede.Models
{
    public class DNSHeader
    {
        /// <summary>
        /// Sixteen bits for identification
        /// </summary>
        public string Identification { get; set; }

        /// <summary>
        /// Sixteen bits for DNS flags
        /// </summary>
        public string Flags { get; set; }

        /// <summary>
        /// Sixteen bits indicating the number of entries in the questions list
        /// </summary>
        public ushort TotalQuestions { get; set; }

        /// <summary>
        /// Sixteen bits indicating the number of entries in the answer resource record list
        /// </summary>
        public ushort TotalAnswerRRs { get; set; }

        /// <summary>
        /// Sixteen bits indicating the number of entries in the authority resource record list
        /// </summary>
        public ushort TotalAuthorityRRs { get; set; }

        /// <summary>
        /// Sixteen bits indicating the number of entries in the additional resource record list
        /// </summary>
        public ushort TotalAdditionalRRs { get; set; }
    }
}
