using MyMac.DAL.Entities;


namespace MyMac.DAL.Interfaces
{

    public interface IMacService
    {
        public Task<Result> SaveEmpLogin(EmpLogin empLogin);
        Task<string> GetEmpLogin();
        Task<string> GetUserMacLogin();
        public Task<Result> SaveUserMacLogin(MacLogin macLogin);

        public Task<Result> SaveFailureLog(FailureLog failureLog);
    }
}
