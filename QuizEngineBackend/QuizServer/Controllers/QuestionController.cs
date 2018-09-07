using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace QuizServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private IHubContext<QuestionHub> _questionHubContext;

        public QuestionController(IHubContext<QuestionHub> questionHubContext)
        {
            _questionHubContext = questionHubContext;
        }

        public IActionResult Post()
        {
            _questionHubContext.Clients.All.SendAsync("send", "This is a question");
            return Ok();
        }
        public IActionResult PostUserId()
        {
            _questionHubContext.Clients.All.SendAsync("onConnectionMapping", "This is a userID");
            return Ok();
        }
        public IActionResult Get()
        {
            _questionHubContext.Clients.All.SendAsync("endOfQuiz", "This is the end of Quiz... BYE");
            return Ok();
        }

    }
}