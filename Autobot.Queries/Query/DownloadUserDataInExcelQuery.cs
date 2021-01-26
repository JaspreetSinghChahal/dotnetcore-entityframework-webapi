using MediatR;

namespace Autobot.Queries.Query
{
    public class DownloadUserDataInExcelQuery : IRequest<byte[]>
    {
    }
}
