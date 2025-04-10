using Google;
using Microsoft.EntityFrameworkCore;
using Repositories.DatabaseConnection;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class ReportRepository : IReportRepository
    {
        private readonly MerakiDbContext _context;

        public ReportRepository(MerakiDbContext context)
        {
            _context = context;
        }

        public async Task<dynamic> GetLatestReportId()
        {
            try
            {
                // Fetch the relevant data from the database
                var reportIds = await _context.Reports
                    .Select(u => u.ReportId)
                    .ToListAsync();

                // Process the data in memory to extract and order by the numeric part
                var latestReportId = reportIds
                    .Select(id => new { ReportId = id, NumericPart = int.Parse(id.Substring(2)) })
                    .OrderByDescending(u => u.NumericPart)
                    .ThenByDescending(u => u.ReportId)
                    .Select(u => u.ReportId)
                    .FirstOrDefault();

                return latestReportId;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
