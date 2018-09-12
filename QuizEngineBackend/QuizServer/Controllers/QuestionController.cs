using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QuizServer.Models;
using QuizServer.Service;

namespace QuizServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private IHubContext<QuestionHub> _questionHubContext;
        private IQuizEngineService _quizEngineService;
        private IResultService _resultService;
        

        public QuestionController(IHubContext<QuestionHub> questionHubContext, IQuizEngineService quizEngineService, IResultService resultService)
        {
            _questionHubContext = questionHubContext;
            _quizEngineService = quizEngineService;
            _resultService = resultService;
            
        }

        public IActionResult Post()
        {
            _questionHubContext.Clients.All.SendAsync("send", "This is a question");
            return Ok();
        }


        [HttpPost("post")]
        public async Task<IActionResult> PostToResultCollection([FromBody] UserInfo userInfo)
        {
            await _resultService.PostUserInfo(userInfo);
            Console.WriteLine("Inside post , UserInfo == > " + userInfo);

            return Ok(userInfo);
        }


        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var questions = await _resultService.GetByID(id);
            Console.WriteLine("Inside get by Id method " + id);
            return Ok(questions);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllEntries()
        {
            var entries = await _resultService.GetAll();
            Console.WriteLine("Inside get all entries " + entries);

            return Ok(entries);
        }
        //public IActionResult PostUserId()
        //{
        //    _questionHubContext.Clients.All.SendAsync("onConnectionMapping", "This is a userID");
        //    return Ok();
        //}
        //public IActionResult Get()
        //{
        //    _questionHubContext.Clients.All.SendAsync("endOfQuiz", "This is the end of Quiz... BYE");
        //    return Ok();
        //}

        //[HttpGet("domain/{domain}")]
        //public async Task<IActionResult> GetQuestionsBydomain()
        //{
        //    var questions = await _quizEngineService.GetQuestionByDomain();
        //    return Ok(questions);
        //}




    }
}