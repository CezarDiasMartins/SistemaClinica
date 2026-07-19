using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaClinica.Application.UseCases.Pacientes.Commands.Create;
using SistemaClinica.Application.UseCases.Pacientes.Commands.Delete;
using SistemaClinica.Application.UseCases.Pacientes.Commands.Update;
using SistemaClinica.Application.UseCases.Pacientes.Queries.Get;
using SistemaClinica.Application.UseCases.Pacientes.Queries.List;
using SistemaClinica.Application.UseCases.Pacientes.Queries.Search;

namespace SistemaClinica.API.Controllers;

[ApiController]
[Authorize(Roles = "Administrador,Recepcionista")]
[Route("api/[controller]")]
public class PacientesController(IMediator mediator) : ControllerBase
{
    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] SearchPacienteQuery query, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ListPacienteQuery(), cancellationToken));
    }

    [HttpGet("GetById/{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetPacienteQuery { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreatePacienteCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response) : BadRequest(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, UpdatePacienteCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeletePacienteCommand { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
