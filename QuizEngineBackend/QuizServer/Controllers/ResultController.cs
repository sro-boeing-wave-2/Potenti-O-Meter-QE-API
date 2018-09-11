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
    public class ResultController : ControllerBase
    {
        private IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpPost]
        public async Task<IActionResult> PostToResultCollection([FromBody] UserInfo userInfo)
        {
            await _resultService.PostUserInfo(userInfo);

            return Ok(userInfo);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetQuestionsById([FromRoute] int id)
        {
            var questions = await _resultService.GetByID(id);
            return Ok(questions);
        }
    }
}