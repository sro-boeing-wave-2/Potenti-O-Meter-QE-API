using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QuizServer.Service;

namespace QuizServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private IHubContext<QuestionHub> _questionHubContext;
        private IQuizEngineService _quizEngineService;

        public QuestionController(IHubContext<QuestionHub> questionHubContext, IQuizEngineService quizEngineService)
        {
            _questionHubContext = questionHubContext;
            _quizEngineService = quizEngineService;
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

        [HttpGet("domain/{domain}")]
        public async Task<IActionResult> GetQuestionsBydomain()
        {
            var questions = await _quizEngineService.GetQuestionByDomain();
            return Ok(questions);
        }

    }
}