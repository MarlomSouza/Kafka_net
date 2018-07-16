﻿using System;
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
            return new JsonResult("ok");
        }

        [HttpPost]
        public async void PostAsync([FromBody]string[] links)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    await ocr.ExecuteOcrAsync(links[0]).ConfigureAwait(false);
                }
            }
            catch (System.Exception)
            {
                BadRequest();
            }

        }
    }
}
