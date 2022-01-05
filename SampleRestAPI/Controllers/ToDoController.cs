using Microsoft.AspNetCore.Mvc;
using SampleRestAPI.Database.Entity;
using SampleRestAPI.Services;
using SampleRestAPI.Services.Exceptions;
using static SampleRestAPI.Database.Entity.ToDo;
using static SampleRestAPI.Services.ToDoService;

namespace SampleRestAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoService toDoService;

        public ToDoController(ToDoService toDoService)
        {
            this.toDoService = toDoService;
        }


        /// <summary>
        /// Get all todos from database
        /// </summary>
        [Route("ToDo")]
        [HttpGet]
        public IEnumerable<ToDo> GetAll()
        {
            return toDoService.GetAll();
        }


        /// <summary>
        /// Get incoming todos
        /// </summary>
        [Route("ToDo/incomng/{when}")]
        [HttpGet]
        public IEnumerable<ToDo> GetIncoming(ToDoWhen when)
        {
            return toDoService.GetIncoming(when);
        }


        /// <summary>
        /// Get specific todo by id
        /// </summary>
        [Route("ToDo/{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ToDo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public IActionResult GetOne(int id)
        {
            var obj = toDoService.GetOne(id);
            if (obj == null) return NotFound();
            else return Ok(obj);
        }



        /// <summary>
        /// Add new todo
        /// </summary>
        [Route("ToDo")]
        [HttpPost]
        public ToDo Insert(ToDo item)
        {
            return toDoService.Insert(item);
        }


        /// <summary>
        /// Update existing todo
        /// </summary>
        [Route("ToDo/{id}")]
        [HttpPut]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public StatusCodeResult Update(int id, ToDo item)
        {
            try
            {
                toDoService.Update(id, item);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Set status as done
        /// </summary>
        [Route("ToDo/{id}/done")]
        [HttpPut]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public StatusCodeResult SetAsDone(int id)
        {
            try
            {
                toDoService.SetAsDone(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Set compeltion percentage
        /// </summary>
        [Route("ToDo/{id}/completion")]
        [HttpPut]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public StatusCodeResult SetAsDone(int id, [FromBody]decimal percentage)
        {
            try
            {
                toDoService.SetCompletionPercentage(id, percentage);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }



        /// <summary>
        /// Delete todo by id
        /// </summary>
        [Route("ToDo/{id}")]
        [HttpDelete]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public StatusCodeResult Delete(int id)
        {
            try
            {
                toDoService.Delete(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
