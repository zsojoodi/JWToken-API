using JWTAuthentication.Model;
using JWTAuthentication.Model.Todo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private Model.Todo.ApplicationDbContext _context;

        public TodoController(Model.Todo.ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all todo list 
        /// </summary>
        /// <returns>List<Todo></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<IEnumerable<Todo>> GetAllTodos()
        {
            var todos = _context.Todos.ToList();
            return Ok(todos);
        }

        /// <summary>
        /// Create Todo Item
        /// </summary>
        /// <param name="todo">
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<Todo> CreateToDo(Todo todo)
        {
            try
            {
                if (todo == null)
                {
                    return NoContent();
                }

                _context.Todos.Add(todo);
                _context.SaveChanges();
                return Ok(new { status = "Success", message = "Todo Added!!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "Fail", message = ex.Message });
            }

        }
    }
}
