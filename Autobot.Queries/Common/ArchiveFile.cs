using System;

namespace Autobot.Queries.Common
{
    public class ArchiveFile
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public Byte[] FileBytes { get; set; }
    }
}
