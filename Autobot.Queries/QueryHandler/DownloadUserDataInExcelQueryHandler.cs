using Autobot.Data.Model;
using Autobot.Infrastructure.Identity;
using Autobot.Infrastructure.OpenXml;
using Autobot.Queries.Query;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class DownloadUserDataInExcelQueryHandler : IRequestHandler<DownloadUserDataInExcelQuery, byte[]>
    {
        private readonly ISpreadsheetService _spreadsheetService;
        private readonly IUserManagerRepository _repository;
        private readonly IMapper _mapper;
        public DownloadUserDataInExcelQueryHandler(ISpreadsheetService spreadsheetService, IUserManagerRepository repository, IMapper mapper)
        {
            _spreadsheetService = spreadsheetService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<byte[]> Handle(DownloadUserDataInExcelQuery request, CancellationToken cancellationToken)
        {
            var userList = _repository.GetUserList();
            var users = _mapper.Map<List<UserDetails>>(userList);

            if (_spreadsheetService.CreateSpreadsheet() == true)
            {
                // Create a few columns
                _spreadsheetService.CreateColumnWidth(1);
                _spreadsheetService.CreateColumnWidth(2);
                _spreadsheetService.CreateColumnWidth(3);
                _spreadsheetService.CreateColumnWidth(4);
                _spreadsheetService.CreateColumnWidth(5);
                _spreadsheetService.CreateColumnWidth(6);
                _spreadsheetService.CreateColumnWidth(7);

                // Add column headers
                _spreadsheetService.AddHeader(new List<string>() { "Id", "FirstName", "LastName", "PhoneNumber", "Location", "OtherDetails", "DisplayPassword" });

                foreach (var user in users)
                {
                    _spreadsheetService.AddRow(new List<string>() { user.Id, user.FirstName, user.LastName, user.PhoneNumber, user.Location, user.OtherDetails, user.DisplayPassword });
                }

                _spreadsheetService.CloseSpreadsheet();
            }

            return await Task.FromResult(_spreadsheetService.SpreadsheetStream.ToArray());
        }
    }
}
