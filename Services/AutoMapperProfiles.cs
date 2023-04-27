using AutoMapper;
using BudgetingApp.Models;
using System.Runtime.InteropServices;

namespace BudgetingApp.Services
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() {
            
            CreateMap<Account, AccountCreation>();
            CreateMap<UpdateTransactionModel, Transaction>().ReverseMap();

        }

    }
}
