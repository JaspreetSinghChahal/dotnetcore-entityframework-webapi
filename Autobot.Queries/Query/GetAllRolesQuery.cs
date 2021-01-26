using Autobot.Data.Models;
using MediatR;
using System.Collections.Generic;

namespace Autobot.Queries.Query
{
    public class GetAllRolesQuery : IRequest<List<Roles>>
    {

    }
}
