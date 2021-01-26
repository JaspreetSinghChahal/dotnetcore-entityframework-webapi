using MediatR;

namespace Autobot.Queries.Query
{
    public class DownloadQRCodeQuery : IRequest<byte[]>
    {
        public string BatchId { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Color { get; set; }
        public string FileType { get; set; }
    }
}