using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMac.DAL.Entities;
using MyMac.DAL.Interfaces;

namespace MyMAC.API.Controllers
{
  
    [ApiController]
    public class UserController : ControllerBase
    {
        public IMacService macService;
        public UserController(IMacService _macService)
        {
            macService = _macService;
        }

        [HttpGet]
        [Route("api/GetEmpLogin")]
        public async Task<string> GetEmpLogin()
        {
            try
            {
                return await macService.GetEmpLogin();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/GetUserMacLogin")]
        public async Task<string> GetUserMacLogin()
        {
            try
            {
                return await macService.GetUserMacLogin();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("api/SaveEmpLogin")]
        public async Task<Result> SaveEmpLogin(EmpLogin empLogin)
        {
            try
            {
                return await macService.SaveEmpLogin(empLogin);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("api/SaveUserMacLogin")]
        public async Task<Result> SaveUserMacLogin(MacLogin macLogin)
        {
            try
            {
                return await macService.SaveUserMacLogin(macLogin);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("api/SaveFailureLog")]
        public async Task<Result> SaveFailureLog(FailureLog failureLog)
        {
            try
            {
                return await macService.SaveFailureLog(failureLog);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
