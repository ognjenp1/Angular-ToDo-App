using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using ToDoCore.Interfaces;
using ToDoCore.Models;

namespace ToDoApi.Controllers
{
    [Route("api/to-do-lists")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        private readonly ILogger _logger;

        public ToDoController(IToDoService toDoService, ILogger<ToDoController> logger)
        {
            _toDoService = toDoService;
            _logger = logger;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{toDoListId}/update-position")]
        public ActionResult UpdateListPosition(Guid toDoListId, [FromBody]int position)
        {
            try
            {
                _toDoService.UpdateListPosition(toDoListId, position);
                return NoContent();
            }
            catch(KeyNotFoundException e)
            {
                _logger.LogError(e.StackTrace, " UpdateListPosition() KeyNotFoundException!");
                return NotFound();
            }
            catch(Exception e)
            {
                _logger.LogError(e.StackTrace, " UpdateListPosition() exception!");
                return BadRequest();
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{toDoListId}/to-do-items/{toDoItemId}/update-position")]
        public ActionResult UpdateItemPosition(Guid toDoListId, Guid toDoItemId, [FromBody]int position)
        {
            try
            {
                _toDoService.UpdateItemPosition(toDoListId, toDoItemId, position);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e.StackTrace, " UpdateListPosition() KeyNotFoundException!");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace, " UpdateItemPosition() exception!");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ToDoList>), StatusCodes.Status200OK)]
        [Route("")]
        public ActionResult GetAllToDoLists()
        {
            IEnumerable<ToDoList> lists = _toDoService.GetAllToDoLists(GetOwnerEmail());
            _logger.LogDebug("ToDoList.GetAll() executed!");
            return Ok(lists);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ToDoList), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{toDoListId}")]
        public ActionResult GetToDoListById(Guid toDoListId)
        {
            try
            {
                ToDoList list = _toDoService.GetToDoListById(toDoListId);
                _logger.LogDebug("ToDoList.GetById() executed!");
                return Ok(list);
            }
            catch (KeyNotFoundException e)
            {
               _logger.LogError(e.StackTrace, " ToDoList.GetById() KeyNotFound exception!");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace, " ToDoList.GetById() exception!");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ToDoList>), StatusCodes.Status200OK)]
        [Route("search/{text}")]
        public ActionResult SearchToDoLists(string text)
        {
            IEnumerable<ToDoList> list = _toDoService.SearchToDoLists(text, GetOwnerEmail());
            _logger.LogDebug("ToDoList.GetAll() started!");
            return Ok(list);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ToDoList), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("")]
        public ActionResult AddToDoList([FromBody] ToDoList toDoList)
        {
            try
            {
                _toDoService.AddToDoList(toDoList, GetOwnerEmail());
                _logger.LogDebug("ToDoList.Add() executed!");
                return CreatedAtAction(nameof(GetToDoListById), new { toDoListId = toDoList.Id }, toDoList);
            }
            catch(Exception e)
            {
                _logger.LogError(e.StackTrace, "ToDoList.Add() exception!");
                return BadRequest();
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(IEnumerable<ToDoList>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{toDoListId}")]
        public ActionResult EditToDoList(Guid toDoListId, [FromBody] ToDoList updatedToDoList)
        {
            try
            {
                _toDoService.UpdateToDoList(toDoListId, updatedToDoList);
                _logger.LogDebug("ToDoList.Update() executed!");
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e.StackTrace, " ToDoList.Update() KeyNotFound exception!");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace, " ToDoList.Update() exception!");
                return BadRequest();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{toDoListId}")]
        public ActionResult DeleteToDoListbyId(Guid toDoListId)
        {
            try
            {
                _toDoService.DeleteToDoList(toDoListId);
                _logger.LogDebug("ToDoList.DeleteById() executed!");
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e.StackTrace, " ToDoList.DeleteById() KeyNotFound exception!");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace, " ToDoList.DeleteById() exception!");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ToDoItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{toDoListId}/to-do-items/{toDoItemId}")]
        public ActionResult GetToDoItemById(Guid toDoListId, Guid toDoItemId)
        {
            try
            {
                ToDoItem item = _toDoService.GetToDoItemById(toDoListId, toDoItemId);
                _logger.LogDebug("ToDoItem.GetById() executed!");
                return Ok(item);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e.StackTrace, " ToDoItem.GetById() KeyNotFound exception!");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace, " ToDoItem.GetById() exception!");
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ToDoItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{toDoListId}/to-do-items")]
        public ActionResult AddToDoListItem(Guid toDoListId, [FromBody] ToDoItem toDoItem)
        {
            try
            {
                _toDoService.AddToDoItem(toDoListId, toDoItem, GetOwnerEmail());
                _logger.LogDebug("ToDoItem.GetById() executed!");
                return Ok();
            }
            catch(KeyNotFoundException e)
            {
                _logger.LogError(e.StackTrace, " ToDoItem.Add() KeyNotFound exception!");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace, " ToDoItem.Add() exception!");
                return BadRequest();
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(List<ToDoItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{toDoListId}/to-do-items/{toDoItemId}")]
        public ActionResult EditToDoItem(Guid toDoListId, Guid toDoItemId, [FromBody] ToDoItem updatedToDoItem)
        {
            try
            {
                _toDoService.UpdateToDoItem(toDoListId, toDoItemId, updatedToDoItem);
                _logger.LogDebug("ToDoItem.Update() executed!");
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e.StackTrace, " ToDoItem.Update() KeyNotFound exception!");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace, " ToDoItem.Update() exception!");
                return BadRequest();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{toDoListId}/to-do-items/{toDoItemId}")]
        public ActionResult DeleteToDoItemById(Guid toDoListId, Guid toDoItemId)
        {
            try
            {
                _toDoService.DeleteToDoItem(toDoListId, toDoItemId);
                _logger.LogDebug("ToDoItem.DeleteById() executed!");
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e.StackTrace, " ToDoItem.DeleteById() KeyNotFound exception!");
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace, " ToDoItem.DeleteById() exception!");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ToDoItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{toDoListId}/to-do-items")]
        public ActionResult<IEnumerable<ToDoItem>> GetAllToDoItemsByToDoListId(Guid toDoListId)
        {
            try
            {
                IEnumerable<ToDoItem> items = _toDoService.GetToDoItemsByListId(toDoListId);
                _logger.LogDebug("ToDoItem.GetByListId() executed!");
                return Ok(items);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogError(e.StackTrace, " ToDoItem.GetByListId() exception!");
                return NotFound();
            }
        }

        private string GetOwnerEmail()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                string ownerEmail = identity.FindFirst("https://todo.com/email").Value;
                return ownerEmail;
            }
            return "";
        }
    }

}
