using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Queries.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class BrandQueryHandler : IRequestHandler<BrandQuery, List<Brand>>
    {
        private readonly IAutobotDbContext _context;

        public BrandQueryHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<List<Brand>> Handle(BrandQuery request, CancellationToken cancellationToken)
        {
            var brands = from brand in _context.Brand
                         select new Brand
                         {
                             BrandId = brand.BrandId,
                             BrandName = brand.BrandName
                         };

            // Filter
            if (!String.IsNullOrEmpty(request.Filter))
            {
                brands = brands.Where(s => s.BrandName.ToLower().Contains(request.Filter.ToLower()));
            }
            // Sort
            switch (request.SortColumn.ToLower() + "_" + "" + request.SortOrder.ToLower())
            {
                case "brandname_desc":
                    brands = brands.OrderByDescending(s => s.BrandName);
                    break;
                case "brandname_asc":
                    brands = brands.OrderBy(s => s.BrandName);
                    break;
                case "brandid_desc":
                    brands = brands.OrderByDescending(s => s.BrandId);
                    break;
                case "brandid_asc":
                    brands = brands.OrderBy(s => s.BrandId);
                    break;
                default:
                    brands = brands.OrderByDescending(s => s.BrandId);
                    break;
            }

            int count = brands.Count();

            // Paginate
            brands = brands.Skip(request.PageNumber * request.PageSize).Take(request.PageSize);

            List<Brand> brandList = new List<Brand>();

            if (brandList != null)
            {
                // Update count
                brandList = brands.ToList();
                brandList.ForEach(x => x.FilteredCount = count);
            }

            return await Task.FromResult(brandList);
        }
    }
}
