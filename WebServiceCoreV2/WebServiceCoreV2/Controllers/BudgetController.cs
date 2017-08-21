using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServiceCoreV2.Models.Payments;
using WebServiceCoreV2.Models;
using WebServiceCoreV2.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using static WebServiceCoreV2.Storage.GlobalVariable;

namespace WebServiceCoreV2.Controllers
{
    [Produces("application/json")]
    [Route("api/Budget")]
    public class BudgetController : Controller
    {
        /*
        // GET: api/Budget
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        */
        // GET: api/Budget/5
        [HttpGet("{typePayment}/{datePay}", Name = "Get")]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<ShowDto<MethodPay>> GetAsync(string typePayment, DateTime datePay)
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                List<MethodPay> result = await sqlServer.GetListPayment(typePayment: typePayment, datePay: datePay);
                ShowDto<MethodPay> showDto = new ShowDto<MethodPay> { Name = $"{typePayment} List", ListDto = result };
                return showDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting data: {ex.Message}");
                // new List<MethodPay> { new MethodPay { TypePayment = "Error", PaymentValue = new Payment { Name = $"Error getting Data is: {ex.Message}" } } }
                return new ShowDto<MethodPay> { Name = $"Error getting data: {ex.Message}", ListDto = new List<MethodPay>() };
            }
        }
        // GET: api/Budget/5
        [HttpGet("GetPaymentFilteredAsync/{filter}/{dateBuy}/{typePayment}/{typeConsult}", Name = "GetPaymentFilteredAsync")]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<ShowDto<PaymentFiltered>> GetPaymentFilteredAsync(string filter, string dateBuy, string typePayment, string typeConsult)
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                List<PaymentFiltered> result = await sqlServer.GetListPaymentFiltedAsync(filter: filter, typePayment: typePayment, typeConsult: typeConsult, dateBuy: dateBuy);
                ShowDto<PaymentFiltered> showDto = new ShowDto<PaymentFiltered> { Name = $"{filter} List", ListDto = result };
                return showDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting data: {ex.Message}");
                return new ShowDto<PaymentFiltered> { Name = $"Error getting data: {ex.Message}", ListDto = new List<PaymentFiltered>() };
            }
        }

        // POST: api/Budget
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        [HttpPost("PostPay")]
        public async Task<TransferData> Post([FromBody]MethodPay value)
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                String result = await sqlServer.InsertPaymentAsync(mPay: value);
                return new TransferData { Data = result };
            }
            catch (Exception ex)
            {
                return new TransferData { Data = $"Result Post Budget: {ex.Message}" };
            }
        }

        // PUT: api/Budget/
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        [HttpPut("{oldTypePayment}")]
        public async Task<TransferData> Put(string oldTypePayment, [FromBody]MethodPay mPay)
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                String result = await sqlServer.UpdatePaymentAsync(mPay: mPay, oldTypePayment: oldTypePayment);
                return new TransferData { Data = result };
            }
            catch (Exception ex)
            {
                return new TransferData { Data = $"Result Update Budget: {ex.Message}" };
            }
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}/{typePayment}")]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<TransferData> Delete(int id, string typePayment)
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(credential: DataBaseCredentials);
                String result = await sqlServer.DeletePaymentAsync(idReg: id, typePayment: typePayment);
                return new TransferData { Data = result };
            }
            catch (Exception ex)
            {
                return new TransferData { Data = $"Result Delete Budget: {ex.Message}" };
            }
        }
    }
}
