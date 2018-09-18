using Business;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EnglishLearning.WebApp.Controllers.Api
{
    public class WordController : ApiController
    {
        [HttpPost]
        public async Task<bool> InsertWord(Word word)
        {
            return await WordManager.Instance.AddAsync(word);
        }
    }
}
