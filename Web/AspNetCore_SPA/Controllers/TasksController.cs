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

        /// <summary>
        /// Completed tasks.
        /// </summary>
        /// <returns>Task records with status completed.</returns>
        /// <response code="200">Records returned.</response>
        [HttpGet("status/completed")]
        [ProducesResponseType(200)]
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

        /// <summary>
        /// Not completed tasks.
        /// </summary>
        /// <returns>Task records with status not completed.</returns>
        /// <response code="200">Records returned.</response>
        [HttpGet]
        [ProducesResponseType(200)]
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

        /// <summary>
        /// Returns Task by id if it exists.
        /// </summary>
        /// <param name="id">Id of task.</param>
        /// <returns>Body of task.</returns>
        /// <response code="200">Record found.</response>
        /// <response code="404">Record with given id was not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TaskModel))]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Creates a new Task record or updates existing one if it already exists.
        /// </summary>
        /// <param name="task">Body of task.</param>
        /// <returns>Newly created / updated task record' id.</returns>
        /// <remarks>
        /// Location response header's value may be used to retrieve created / updated object.
        /// </remarks>
        /// <response code="200">Task updated successfully.</response>
        /// <response code="201">Task created successfully.</response>
        /// <response code="400">Passed model was invalid (validation rules were not met).</response>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(TaskModel))]
        [ProducesResponseType(201, Type = typeof(TaskModel))]
        [ProducesResponseType(400)]
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

                    return CreatedAtAction(nameof(GetByIdAsync), new { id = createdTaskId }, createdTaskId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes Task record if it exists.
        /// </summary>
        /// <param name="id">Id of task.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Deletion successful.</response>
        /// <response code="404">Record with given id was not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                bool exists = await _service.ExistsAsync(id);
                if (exists)
                {
                    await _service.DeleteAsync(id);
                    await _service.SaveAsync();

                    _logger.LogInformation($"DeleteAsync({id}) OK.");
                    return NoContent();
                }
                else
                {
                    _logger.LogWarning($"GetById({id}) NOT FOUND during DeleteAsync({id})");
                    return NotFound();
                }

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