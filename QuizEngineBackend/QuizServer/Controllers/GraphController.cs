using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver.V1;
using QuizServer.Models;
using QuizServer.Service;

namespace QuizServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly IGraphService _service;
        public GraphController(IGraphService service)
        {
            _service = service;
        }
        //[HttpPost]
        //public IActionResult CreateConceptNode([FromBody] Concept node)
        //{
        //    bool exists = _service.ConceptExists(node);
        //    IStatementResult result;
        //    if (!exists)
        //    {
        //        result = _service.CreateConceptNode(node);
        //    }
        //    else
        //    {
        //        return BadRequest(error: "Concept already exists");
        //    }
        //    if (result == null)
        //    {
        //        return BadRequest();
        //    }
        //    _service.Dispose();
        //    return Ok();
        //}
        [HttpPost]
        public IActionResult CreateQuestionNode([FromBody]QuestionIdNode node)
        {
            bool exists = _service.QuestionIdExists(node);
            IStatementResult result;
            if (!exists)
            {
                result = _service.CreateQuestionIdNode(node);
            }
            else
            {
                return BadRequest(error: "Concept already exists");
            }
            if (result == null)
            {
                return BadRequest();
            }
            _service.Dispose();
            return Ok();
        }
        [HttpPost("associated")]
        public IActionResult CreateConceptwithAssociatedConcepts([FromBody]ConceptMap concept)
        {
            IStatementResult result;

            bool sourceExists = _service.ConceptExists(concept.Source);
            if (sourceExists)
            {
                bool targetExists = _service.ConceptExists(concept.Target);
                if (!targetExists)
                {
                    result = _service.CreateConceptwithAssociatedConcepts(concept);
                    //result = _service.CreateConcept(node);
                }
                else
                {
                    return BadRequest(error: "Associated concept already exists");
                }
            }
            else
            {
                return BadRequest(error: "Source concept does not exist");
            }

            if (result == null)
            {
                return BadRequest();
            }
            _service.Dispose();
            return Ok(result);
        }
    }
}