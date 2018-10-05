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
    public class ContentRecommenderController : ControllerBase
    {
        private IGraphService _graphService;
        public ContentRecommenderController(IGraphService graphService)
        {
            Console.WriteLine("Inside ContentRecommendController");
            _graphService = graphService;
        }

        [HttpGet("id/{id}/{domain}")]
        public List<ContentRecommender> GetContentByDomainandUSer([FromRoute] int id, [FromRoute] string domain)

        {

            List<ContentRecommender> li = _graphService.GetContentRecommendations(id, domain);
            li.GroupBy(x => x.conceptName).Select(y => y.First()).ToList();

            return li;

        }
    }
}