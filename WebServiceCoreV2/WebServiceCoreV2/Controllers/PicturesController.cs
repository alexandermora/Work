using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using WebServiceCoreV2.Models;
using WebServiceCoreV2.Storage;
using static WebServiceCoreV2.Storage.GlobalVariable;

namespace WebServiceCoreV2.Controllers
{
    [Route("api/[controller]")]
    public class PicturesController : Controller
    {
        public IHostingEnvironment Host { get; }

        public PicturesController(IHostingEnvironment host)
        {
            Host = host;
        }
        // GET: api/values
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/pictures/5
        [HttpGet("{user}")]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<TransferData> GetAsync(string user)
        {
            ManageSqlServer sqlServer = new ManageSqlServer(DataBaseCredentials);
            string urlImageUserHome = await sqlServer.GetImageUrl(user);
            return new TransferData { Data = urlImageUserHome };
        }

        // POST api/pictures
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public async Task<TransferData> PostAsync(IFormFile file)
        {
            try
            {
                ManageSqlServer sqlServer = new ManageSqlServer(DataBaseCredentials);
                MAX_BYTES = int.Parse(await sqlServer.GetPropertyAsync("MaxBytes"));
                string acceptedFile = await sqlServer.GetPropertyAsync("AcceptedImageFile");
                ACCEPTED_FILE_TYPE = acceptedFile.Split(',');

                string uploadFolderPath = Path.Combine(Host.WebRootPath, $"upload");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }
                if (file == null)
                {
                    return new TransferData { Data = "Null File" };
                }
                if (file.Length == 0)
                {
                    return new TransferData { Data = "Empty File" };
                }
                if (file.Length > MAX_BYTES)
                {
                    return new TransferData { Data = "A big size, please reduce the photo" };
                }
                if (!ACCEPTED_FILE_TYPE.Any(s => s.ToLower() == Path.GetExtension(file.FileName).ToLower()))
                {
                    return new TransferData { Data = "It's no a photo" };
                }
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadFolderPath, fileName);
                FileStream stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                return new TransferData { Data = fileName };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while update picture: {ex.Message}");
                return new TransferData { Data = $"Error while update picture: {ex.Message}" };
            }
        }

        // PUT api/pictures/5
        [HttpPut("{id}")]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/pictures/5
        [HttpDelete("{id}")]
        [EnableCors("CorsPolicy")]
        [Authorize(Policy = "BudgetUser")]
        public void Delete(int id)
        {
        }
    }
}
