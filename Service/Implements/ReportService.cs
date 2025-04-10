using Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.DatabaseConnection;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class ReportService
    {
        private readonly MerakiDbContext _context;
        private readonly IReportRepository _reportRepository;
        private readonly IAccountService _accountService;
        private readonly IProductRepository _instructorRepository;
        public ReportService(MerakiDbContext context, IReportRepository reportRepository, IAccountService accountService, IProductRepository instructorRepository)
        {
            _context = context;
            _reportRepository = reportRepository;
            _accountService = accountService;
            _instructorRepository = instructorRepository;
        }

        public async Task<string> AutoGenerateReportId()
        {
            string newReportId = "";
            string latestReportId = await _reportRepository.GetLatestReportId();
            if (string.IsNullOrEmpty(latestReportId))
            {
                newReportId = "RE00000001";
            }
            else
            {
                int numericpart = int.Parse(latestReportId.Substring(2));
                int newnumericpart = numericpart + 1;
                newReportId = $"RE{newnumericpart:d8}";
            }
            return newReportId;
        }

        public async Task<dynamic> SubmitReportAsync(Report report)
        {
            if (report == null || string.IsNullOrEmpty(report.ProductId) || string.IsNullOrEmpty(report.Issue))
            {
                return "Invalid newRport details.";
            }


           ;
            var user = await _accountService.GetUserAccountAsync(report.ReportId);
            if (user == null)
            {
                return "User not found or invalid token.";
            }

            var product = await _context.Products.Include(c => c.Account).FirstOrDefaultAsync(c => c.ProductId == report.ProductId);
            if (product == null)
            {
                return "Course is not exist";
            }
            if (report.Attachment != null)
            {

                var newRport = new Report
                {
                    ReportId = await AutoGenerateReportId(),
                    Issue = report.Issue,
                    Content = report.Content,

                    CreatedBy = user.UserId,
                    CreatedDate = DateTime.Now,
                    ProductId = report.ProductId
                };


                _context.Reports.Add(newRport);
                await _context.SaveChangesAsync();


                return "Report submitted successfully.";

            }
            //===========================

            else
            {
                var newReport = new Report
                {
                    ReportId = await AutoGenerateReportId(),
                    Issue = report.Issue,
                    Content = report.Content,
                    Attachment = "",
                    CreatedBy = user.UserId,
                    CreatedDate = DateTime.Now,
                    ProductId = report.ProductId
                };


                _context.Reports.Add(newReport);
                await _context.SaveChangesAsync();

                return "Report submitted successfully.";
            }
        }
    }
}
