using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    }
}