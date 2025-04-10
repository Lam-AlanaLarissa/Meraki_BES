using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFeedbackService
    {
        public Task<string> AutoGenerateFeedbackId();
        public Task<dynamic> CreateFeedbackAsync(string accEmail, CreateFeedbackDTO feedbackDTO);
        public Task<(List<FeedbackListDTO> Feebacks, int TotalCount, int TotalPages)> GetListFeedbackOfProduct(int pageIndex, int pageSize, string sortBy, bool sortDesc, string search);

    }
}
