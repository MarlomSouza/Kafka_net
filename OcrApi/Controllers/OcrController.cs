using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OcrApi.Controllers
{
    [Route("api/[controller]")]
    public class OcrController : Controller
    {
        private readonly Ocr ocr;

        public OcrController()
        {
            ocr = new Ocr();
        }
        // POST api/values

        [HttpPost]
        public async void PostAsync([FromBody]string[] links)
        {
            try
            {
                foreach (var link in links)
                {
                    await ocr.ExecuteOcrAsync(link);
                }

            }
            catch (System.Exception)
            {
                BadRequest();
            }

        }
    }
}
