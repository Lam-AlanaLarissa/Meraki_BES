using Repositories.DTO;
using Repositories.Interfaces;
using Repositories.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IAccountRepository _accountRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IAccountRepository accountRepository)
        {
            _feedbackRepository = feedbackRepository;
            _accountRepository = accountRepository;
        }

        public async Task<string> AutoGenerateFeedbackId()
        {
            string newFeedbackId = "";
            string latestFeedbackId = await _feedbackRepository.GetLatestFeedbackIdAsync();
            if (string.IsNullOrEmpty(latestFeedbackId))
            {
                newFeedbackId = "FB00000001";
            }
            else
            {
                int numericpart = int.Parse(latestFeedbackId.Substring(2));
                int newnumericpart = numericpart + 1;
                newFeedbackId = $"FB{newnumericpart:d8}";
            }
            return newFeedbackId;
        }
        public async Task<dynamic> CreateFeedbackAsync(string accEmail, CreateFeedbackDTO feedbackDTO)
        {
            var acc = await _accountRepository.GetAccountByEmailAsync(accEmail);
            if (acc == null)
            {
                return new
                {
                    Message = "This account is not already exist",
                    Status = 404
                };
            }
            if (feedbackDTO.Rating == 5)
            {
                feedbackDTO.Detail = "Excellent";
            }
            else if (feedbackDTO.Rating == 4)
            {
                feedbackDTO.Detail = "Good";
            }
            else if (feedbackDTO.Rating == 3)
            {
                feedbackDTO.Detail = "Normal";
            }
            else if (feedbackDTO.Rating == 2)
            {
                feedbackDTO.Detail = "Not Satisfied";
            }
            else if (feedbackDTO.Rating == 1)
            {
                feedbackDTO.Detail = "Bad";
            }

            if (feedbackDTO.Rating >= 4)
            {
                feedbackDTO.IsGoodReview = true;
            }
            else
            {
                feedbackDTO.IsGoodReview = false;
            }
            var feedback = new Feedback
            {
                FeedbackId = await AutoGenerateFeedbackId(),
                Detail = feedbackDTO.Detail,
                Rating = feedbackDTO.Rating,
                Attachment = "Empty",
                AccountId = acc.AccountId,
                CreatedDate = DateTime.Now,
                IsGoodReview = feedbackDTO.IsGoodReview
            };
            var result = await _feedbackRepository.AddFeedbackAsync(feedback);
            return new
            {
                result,
                Message = "Create Feedback Successfull",
                Feedback = new
                {
                    feedback.FeedbackId,
                    feedback.Detail,
                    feedback.Rating,
                    feedback.AccountId,
                    feedback.CreatedDate,
                    feedback.IsGoodReview,
                }
            };
        }
        public async Task<(List<FeedbackListDTO> Feebacks, int TotalCount, int TotalPages)> GetListFeedbackOfProduct(int pageIndex, int pageSize, string sortBy, bool sortDesc, string search)
        {
            return await _feedbackRepository.GetListFeedbackOfProduct(pageIndex, pageSize, sortBy, sortDesc, search);
        }

    }
}
