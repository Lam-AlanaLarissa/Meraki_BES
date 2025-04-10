using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class CustomerWalletService : ICustomerWalletService
    {
        //private readonly ICustomerWalletRepository _wallet;
        //private readonly ICustomerRepository _customerRepository;
        //private readonly IAccountService _accountService;
        //public CustomerWalletService(ICustomerWalletRepository wallet, ICustomerRepository customerRepository, IAccountService accountService)
        //{
        //    _wallet = wallet;
        //    _customerRepository = customerRepository;
        //    _accountService = accountService;
        //}

        //private async Task<string> AutoGenerateCustomerWalletId()
        //{
        //    string newwalletid = "";
        //    string latestWalletCustomerId = await _customerRepository.GetLatestCustomerWalletIdAsync();
        //    if (string.IsNullOrEmpty(latestWalletCustomerId))
        //    {
        //        newwalletid = "CW00000001";
        //    }
        //    else
        //    {
        //        int numericpart = int.Parse(latestWalletCustomerId.Substring(2)); // Correct substring to match "S000000001"
        //        int newnumericpart = numericpart + 1;
        //        newwalletid = $"CW{newnumericpart:d8}";
        //    }
        //    return newwalletid;
        //}
        //public async Task<CustomerWallet> GetCustomerWalletBalanceByTokenAsync(string accId)
        //{
        //    return await _customerRepository.GetCustomerWalletByIdAsync(accId);
        //}
        //public async Task<CustomerWallet> GetCustomerWalletBalanceByIdAsync(string accId)
        //{
        //    return await _customerRepository.GetCustomerWalletByIdAsync(accId);
        //}
        //public async Task UpdateCustomerWalletAsync(CustomerWalletDTO CustomerWalletDTO, string accId)
        //{
        //    var wallet = await _customerRepository.GetCustomerWalletByIdAsync(accId);
        //    if (wallet != null)
        //    {
        //        wallet.Balance = wallet.Balance - CustomerWalletDTO.balance;
        //        await _customerRepository.UpdateCustomerWalletAsync(wallet, CustomerWalletDTO);
        //    }
        //}
        //public async Task UpdateSystemWalletAsync(CustomerWalletDTO CustomerWalletDTO, string accID, string token)
        //{

        //    var CustomerWallet = await _customerRepository.GetCustomerWalletByIdAsync(accID);

        //    var wallet = await _customerRepository.GetSystemWalletAsync();
        //    if (wallet != null)
        //    {
        //        wallet.Balance = wallet.Balance - CustomerWalletDTO.balance;
        //        await _customerRepository.UpdateSystemWalletAsync(wallet, CustomerWalletDTO, CustomerWallet, await _accountService.GetAccountIdByToken(token));
        //    }
        //}
    }
}
