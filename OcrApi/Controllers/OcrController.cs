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

        [HttpGet]
        public ActionResult Get()
        {
            System.Diagnostics.Debug.WriteLine("entrou 1");
            return new JsonResult("ok");
        }

        [HttpPost]
        public async void PostAsync([FromBody]string[] links)
        {
            System.Diagnostics.Debug.WriteLine("entrou");
            Console.WriteLine("entrou no metodo");
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Factory.StartNew(action: async () => { await ocr.ExecuteOcrAsync(links[0], i); });
                }
            }
            catch (System.Exception)
            {
                BadRequest();
            }

        }
    }
}
