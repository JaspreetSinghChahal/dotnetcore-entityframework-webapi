using Autobot.Data.Interfaces;
using Autobot.Data.Model;
using Autobot.Infrastructure.Identity;
using Autobot.Queries.Query;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, List<UserDetails>>
    {
        private readonly IAutobotDbContext _context;
        private readonly IUserManagerRepository _repository;
        private readonly IMapper _mapper;

        public GetUserListQueryHandler(IUserManagerRepository repository, IMapper mapper, IAutobotDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<List<UserDetails>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            // Get users from repository
            var users = _repository.GetUserList();
            var userList = _mapper.Map<List<UserDetails>>(users);

            var userScans = (from scan in _context.UserScans
                             where scan.ScannedDateTime >= DateTime.Now.AddDays(-14)
                             && scan.IsSuccess == true
                             && userList.Select(x => x.Id).Contains(scan.UserId)
                             select scan.UserId).ToList();

            foreach (var user in userList)
            {
                user.IsActive = userScans.Any(x => x == user.Id);
            }

            // Filter
            if (!string.IsNullOrEmpty(request.Filter))
            {
                var searchText = request.Filter.ToLower();
                userList = userList.Where(
                                     s => (s.FirstName.ToLower() + " " + s.LastName.ToLower()).Contains(searchText)
                                          || s.PhoneNumber.Contains(searchText)
                                          || s.Location.ToLower().Contains(searchText)
                                          || s.OtherDetails.ToLower().ToString().Contains(searchText)
                                          || s.IsActive.ToSuccessText().ToLower().Contains(searchText)
                                   ).ToList();
            }
            // Sort
            switch (request.SortColumn.ToLower() + "_" + "" + request.SortOrder.ToLower())
            {
                case "username_desc":
                    userList = userList.OrderByDescending(s => s.FirstName + " " + s.LastName).ToList();
                    break;
                case "username_asc":
                    userList = userList.OrderBy(s => s.FirstName + " " + s.LastName).ToList();
                    break;
                case "phonenumber_desc":
                    userList = userList.OrderByDescending(s => s.PhoneNumber).ToList();
                    break;
                case "phonenumber_asc":
                    userList = userList.OrderBy(s => s.PhoneNumber).ToList();
                    break;
                case "displaypassword_desc":
                    userList = userList.OrderByDescending(s => s.DisplayPassword).ToList();
                    break;
                case "displaypassword_asc":
                    userList = userList.OrderBy(s => s.DisplayPassword).ToList();
                    break;
                case "location_desc":
                    userList = userList.OrderByDescending(s => s.Location).ToList();
                    break;
                case "location_asc":
                    userList = userList.OrderBy(s => s.Location).ToList();
                    break;
                case "otherdetails_desc":
                    userList = userList.OrderByDescending(s => s.OtherDetails).ToList();
                    break;
                case "otherdetails_asc":
                    userList = userList.OrderBy(s => s.OtherDetails).ToList();
                    break;
                case "isactive_desc":
                    userList = userList.OrderByDescending(s => s.IsActive).ToList();
                    break;
                case "isactive_asc":
                    userList = userList.OrderBy(s => s.IsActive).ToList();
                    break;
                case "termsandconditonsacceptedon_desc":
                    userList = userList.OrderByDescending(s => s.TermsAndConditonsAcceptedOn).ToList();
                    break;
                case "termsandconditonsacceptedon_asc":
                    userList = userList.OrderBy(s => s.TermsAndConditonsAcceptedOn).ToList();
                    break;
                default:
                    userList = userList.OrderBy(s => s.FirstName + " " + s.LastName).ToList().ToList();
                    break;
            }

            int count = userList.Count();

            // Paginate
            userList = userList.Skip(request.PageNumber * request.PageSize).Take(request.PageSize).ToList();            

            List<UserDetails> userDetails = new List<UserDetails>();
            // Update count
            if (userList != null)
            {
                userDetails = userList.ToList();
                userDetails.ForEach(x => x.FilteredCount = count);
            }
            userDetails.ForEach(x => x.FilteredCount = count);

            return await Task.FromResult(userList.ToList());
        }
    }
}
