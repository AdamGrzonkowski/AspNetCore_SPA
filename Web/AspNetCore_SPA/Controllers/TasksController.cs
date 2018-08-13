using Application.Model.Tasks;
using Application.Services.Interfaces.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AspNetCore_SPA.Controllers
{
    [Route("api/tasks")]
    public class TasksController : Controller
    {
        private readonly ITaskService _service;
        private readonly ILogger _logger;

        private readonly object _lockObject = new object();
        private static bool _seeded;

        public TasksController(ILogger<TasksController> logger, ITaskService service)
        {
            _logger = logger;
            _service = service;

            SeedTasks();
        }

        [HttpGet("status/completed")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCompletedAsync()
        {
            try
            {
                var result = await _service.GetAllCompletedAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetNotCompletedAsync()
        {
            try
            {
                var result = await _service.GetAllNotCompletedAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TaskModel))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning($"GetById({id}) NOT FOUND");
                    return NotFound();
                }
                _logger.LogDebug($"GetById({id}) OK");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(TaskModel))]
        [ProducesResponseType(201, Type = typeof(TaskModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody]TaskModel task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _service.ExistsAsync(task.Id))
                {
                    await _service.UpdateAsync(task);
                    _logger.LogDebug($"UpdateAsync with Name:'{task.Name}' successful.");
                    await _service.SaveAsync();

                    return Ok(task.Id);
                }
                else
                {
                    Guid createdTaskId = _service.Add(task);
                    _logger.LogDebug($"Add with Name:'{task.Name}' successful.");
                    await _service.SaveAsync();

                    return CreatedAtAction(nameof(GetByIdAsync), new { id = createdTaskId }, task);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                await _service.SaveAsync();

                _logger.LogInformation($"DeleteAsync({id}) OK.");
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private void SeedTasks()
        {
            try
            {
                if (_seeded == false)
                {
                    lock (_lockObject)
                    {
                        if (_seeded == false)
                        {
                            _service.Add(new TaskModel
                            {
                                Name = "Write to Adam",
                                Description = "Give Adam some feedback on this app"
                            });

                            _service.Add(new TaskModel
                            {
                                Name = "Get eggs",
                                Description = "From the local grocery"
                            });


                            _service.Add(new TaskModel
                            {
                                Name = "Write SPA app",
                                Description = "Create SPA web app based on ASP.NET Core",
                                Completed = true
                            });

                            _service.SaveAsync();
                            _seeded = true;

                            _logger.LogDebug("Db seeded with sample tasks records");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _seeded = false;
            }
        }
    }
}