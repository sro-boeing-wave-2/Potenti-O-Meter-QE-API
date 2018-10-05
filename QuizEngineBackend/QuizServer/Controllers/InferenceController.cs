using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizServer.Models;
using QuizServer.Service;

namespace QuizServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InferenceController : ControllerBase
    {
        private IGraphService _graphService;
        public InferenceController(IGraphService graphService)
        {
            Console.WriteLine("Inside InferenceController");
            _graphService = graphService;
        }

        [HttpGet("/bestdomain/{userid}")]
        public List<BestDomain> BestDomain([FromRoute]int userid)
        {
           List<BestDomain> li =  _graphService.GetBestDomain(userid);
            return li;
           
        }
        [HttpGet("/bestconceptofdomain/{userid}/{domain}")]
        public List<BestConceptOfDomain> BestConceptOfDomain([FromRoute]int userid, [FromRoute]string domain)
        {
            List<BestConceptOfDomain> li = _graphService.GetBestConceptOfDomain(userid,domain);
            return li;

        }

    }
}