using Repositories.DTO;
using Repositories.Models;
using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        public Task<Feedback> GetFeedbackByIdAsync(string feedbackId);
        public Task<List<Feedback>> GetListOfFeedbackAsync();
        public Task<string> GetLatestFeedbackIdAsync();
        public Task<dynamic> AddFeedbackAsync(Feedback feedback);
        public Task<dynamic> UpdateFeedbackAsync(Feedback feedback);
        public Task<(List<FeedbackListDTO> Feebacks, int TotalCount, int TotalPages)> GetListFeedbackOfProduct(int pageIndex, int pageSize, string sortBy, bool sortDesc, string search);
        public Task<(List<FeedbackListDTO> Feedbacks, int TotalCount)> GetFeedbackbyProductIdAsync(string productId, int pageIndex, int pageSize, string sortBy, bool sortDesc, string search);
    }
}
