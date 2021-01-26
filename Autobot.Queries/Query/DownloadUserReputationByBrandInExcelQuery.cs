using MediatR;

namespace Autobot.Queries.Query
{
    public class DownloadUserReputationByBrandInExcelQuery : IRequest<byte[]>
    {
    }
}