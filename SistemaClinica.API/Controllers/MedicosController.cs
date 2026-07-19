using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaClinica.Application.UseCases.Medicos.Commands.Create;
using SistemaClinica.Application.UseCases.Medicos.Commands.Delete;
using SistemaClinica.Application.UseCases.Medicos.Commands.Update;
using SistemaClinica.Application.UseCases.Medicos.Queries.Get;
using SistemaClinica.Application.UseCases.Medicos.Queries.List;
using SistemaClinica.Application.UseCases.Medicos.Queries.Search;

namespace SistemaClinica.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MedicosController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] SearchMedicoQuery query, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ListMedicoQuery(), cancellationToken));
    }

    [Authorize(Roles = "Administrador,Recepcionista,Medico")]
    [HttpGet("GetById/{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetMedicoQuery { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPost]
    public async Task<IActionResult> Post(CreateMedicoCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response) : BadRequest(response);
    }

    [Authorize(Roles = "Administrador")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, UpdateMedicoCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteMedicoCommand { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
