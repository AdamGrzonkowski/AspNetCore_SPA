using Interfaces.Tasks;
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
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger _logger;

        private object _lockObject = new object();
        private static bool _seeded;

        public TasksController(ITaskRepository taskRepository, ILogger<TasksController> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;

            SeedTasks();
        }

        [HttpGet("status/completed")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCompletedAsync()
        {
            try
            {
                var result = await _taskRepository.GetAllCompletedAsync();
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
                var result = await _taskRepository.GetAllNotCompletedAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Entities.Tasks.Task))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _taskRepository.FindAsync(x => x.Id == id);
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
        [ProducesResponseType(201, Type = typeof(Entities.Tasks.Task))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PostAsync([FromBody]Entities.Tasks.Task task)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _taskRepository.ExistsAsync(x => x.Id == task.Id))
                {
                    await _taskRepository.UpdateAsync(task);
                    _logger.LogDebug($"UpdateAsync with Name:'{task.Name}' successful.");
                }
                else
                {
                    await _taskRepository.AddAsync(task);
                    _logger.LogDebug($"AddAsync with Name:'{task.Name}' successful.");
                }

                await _taskRepository.SaveAsync();

                return CreatedAtAction(nameof(GetByIdAsync), new { id = task.Id }, task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                int result = await _taskRepository.DeleteAsync(id);
                await _taskRepository.SaveAsync();

                _logger.LogInformation($"DeleteAsync({id}) OK.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool SeedTasks()
        {
            try
            {
                if (_seeded == false)
                {
                    lock (_lockObject)
                    {
                        if (_seeded == false)
                        {
                            _taskRepository.AddAsync(new Entities.Tasks.Task
                            {
                                Name = "Write to Adam",
                                Description = "Give Adam some feedback on this app"
                            });

                            _taskRepository.AddAsync(new Entities.Tasks.Task
                            {
                                Name = "Get eggs",
                                Description = "From the local grocery"
                            });


                            _taskRepository.AddAsync(new Entities.Tasks.Task
                            {
                                Name = "Write SPA app",
                                Description = "Create SPA web app based on ASP.NET Core",
                                Completed = true
                            });

                            _taskRepository.SaveAsync();
                            _seeded = true;

                            _logger.LogDebug("Db seeded with sample tasks records");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}