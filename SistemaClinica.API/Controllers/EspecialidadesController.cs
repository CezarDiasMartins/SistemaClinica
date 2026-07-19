using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaClinica.Application.UseCases.Especialidades.Commands.Create;
using SistemaClinica.Application.UseCases.Especialidades.Commands.Delete;
using SistemaClinica.Application.UseCases.Especialidades.Commands.Update;
using SistemaClinica.Application.UseCases.Especialidades.Queries.Get;
using SistemaClinica.Application.UseCases.Especialidades.Queries.List;
using SistemaClinica.Application.UseCases.Especialidades.Queries.Search;

namespace SistemaClinica.API.Controllers;

[ApiController]
[Authorize(Roles = "Administrador,Recepcionista")]
[Route("api/[controller]")]
public class EspecialidadesController(IMediator mediator) : ControllerBase
{
    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] SearchEspecialidadeQuery query, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ListEspecialidadeQuery(), cancellationToken));
    }

    [HttpGet("GetById/{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetEspecialidadeQuery { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> Post(CreateEspecialidadeCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response) : BadRequest(response);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, UpdateEspecialidadeCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteEspecialidadeCommand { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
