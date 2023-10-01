using Dapper;
using MyMac.DAL.Entities;
using MyMac.DAL.Interfaces;

namespace MyMac.DAL.Services
{
    public class MacService : BaseRepo, IMacService
    {
      
        public async Task<string> GetEmpLogin()
        {
            return await QueryFirstOrDefaultAsync<string>("SP_GetEmpLogin");
        }

        public async Task<string> GetUserMacLogin()
        {
            return await QueryFirstOrDefaultAsync<string>("SP_GetUsersMac");
        }

        public async Task<Result> SaveEmpLogin(EmpLogin empLogin)
        {
            return await QueryFirstOrDefaultAsync<Result>("SP_SaveEmpLogin", empLogin);

        }

        public async Task<Result> SaveUserMacLogin(MacLogin macLogin)
        {
            return await QueryFirstOrDefaultAsync<Result>("SP_SaveUserMAC", macLogin);

        }
        public async Task<Result> SaveFailureLog(FailureLog failureLog)
        {
            return await QueryFirstOrDefaultAsync<Result>("SaveFailureLog", failureLog);
        }
    }
}
