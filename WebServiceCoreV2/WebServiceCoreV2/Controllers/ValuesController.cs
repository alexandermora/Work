using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using WebServiceCoreV2.Storage;
using WebServiceCoreV2.Models;
using static WebServiceCoreV2.Storage.GlobalVariable;

namespace WebServiceCoreV2.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [Route("GetCountriesAsync")]
        [HttpGet]
        [AllowAnonymous]
        [EnableCors("CorsPolicy")]
        public async Task<ShowDto<ListShow>> GetCountriesAsync()
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                return new ShowDto<ListShow> { Name = "Contries", ListDto = await sqlServer.GetContriesAsync() };
            }
            catch (Exception ex)
            {
                return new ShowDto<ListShow> { Name="Error", ListDto = new List<ListShow>() { new ListShow { Id = -1, Name = $"Error GetCountriesAsync is: {ex.Message}" } } };
            }
        }

        [Route("GetGroupAsync")]
        [HttpGet]
        [AllowAnonymous]
        [EnableCors("CorsPolicy")]
        public async Task<ShowDto<ListShow>> GetGroupAsync()
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                return new ShowDto<ListShow> { Name = "Group", ListDto = await sqlServer.GetGroupAsync() };
            }
            catch (Exception ex)
            {
                return new ShowDto<ListShow> { Name = "Error", ListDto = new List<ListShow>() { new ListShow { Id = -1, Name = $"Error GetGroupAsync is: {ex.Message}" } } };
            }
        }

        [Route("GetPrivileges/{username}")]
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<TransferData> GetPrivileges(string username)
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                string result = await sqlServer.GetPrivilegesAsync(username);
                return new TransferData { Data = result };
            }
            catch (Exception ex)
            {
                return new TransferData { Data = $"Error GetGroupAsync is: {ex.Message}" };
            }
        }

        [Route("GetTimeAsync", Name = "GetTimeAsync")]
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<ShowDto<ListShow>> GetTimeAsync()
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                return new ShowDto<ListShow> { Name = "Time", ListDto = await sqlServer.GetTimeAsync() };
            }
            catch (Exception ex)
            {
                return new ShowDto<ListShow> { Name = "Error", ListDto = new List<ListShow>() { new ListShow { Id = -1, Name = $"Error GetCountriesAsync is: {ex.Message}" } } };
            }
        }

        [Route("GetCataloguesAsync", Name = "GetCataloguesAsync")]
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<ShowDto<ListShow>> GetCataloguesAsync()
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                return new ShowDto<ListShow> { Name = "Catalogue", ListDto = await sqlServer.GetCataloguesAsync() };
            }
            catch (Exception ex)
            {
                return new ShowDto<ListShow> { Name = "Error", ListDto = new List<ListShow>() { new ListShow { Id = -1, Name = $"Error GetCountriesAsync is: {ex.Message}" } } };
            }
        }

        [Route("GetCurrencyAsync", Name = "GetCurrencyAsync")]
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<ShowDto<ListShow>> GetCurrencyAsync()
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                return new ShowDto<ListShow> { Name = "Currency", ListDto = await sqlServer.GetCurrencyAsync() };
            }
            catch (Exception ex)
            {
                return new ShowDto<ListShow> { Name = "Error", ListDto = new List<ListShow>() { new ListShow { Id = -1, Name = $"Error GetCountriesAsync is: {ex.Message}" } } };
            }
        }

        [Route("FilterAsync", Name = "FilterAsync")]
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<ShowDto<ListShow>> FilterAsync()
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                return new ShowDto<ListShow> { Name = "Currency", ListDto = await sqlServer.GetFilterAsync() };
            }
            catch (Exception ex)
            {
                return new ShowDto<ListShow> { Name = "Error", ListDto = new List<ListShow>() { new ListShow { Id = -1, Name = $"Error GetCountriesAsync is: {ex.Message}" } } };
            }
        }

        [Route("TypeConsultAsync", Name = "TypeConsultAsync")]
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<ShowDto<ListShow>> TypeConsultAsync()
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                return new ShowDto<ListShow> { Name = "Currency", ListDto = await sqlServer.GetTypeConsultAsync() };
            }
            catch (Exception ex)
            {
                return new ShowDto<ListShow> { Name = "Error", ListDto = new List<ListShow>() { new ListShow { Id = -1, Name = $"Error GetCountriesAsync is: {ex.Message}" } } };
            }
        }
        // POST api/values
        [HttpPost]
        public async Task<TransferData> PostAsync([FromBody]ShowDto<ListShow> value)
        {
            ManageSqlServer manageSqlServer = new ManageSqlServer(credential: DataBaseCredentials);
            string privileges = await manageSqlServer.GetPrivilegesAsync(username: CredentialUser.Username);
            switch (privileges)
            {
                case "Administrator":
                    {
                        string result = await manageSqlServer.InsertOptionsAsync(value);
                        return new TransferData { Data = result };
                    }
                default:
                    {
                        return new TransferData { Data = "You not have privileges" };
                    }
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
