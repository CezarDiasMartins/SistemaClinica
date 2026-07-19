using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaClinica.Application.UseCases.Recepcionistas.Commands.Create;
using SistemaClinica.Application.UseCases.Recepcionistas.Commands.Delete;
using SistemaClinica.Application.UseCases.Recepcionistas.Commands.Update;
using SistemaClinica.Application.UseCases.Recepcionistas.Queries.Get;
using SistemaClinica.Application.UseCases.Recepcionistas.Queries.List;
using SistemaClinica.Application.UseCases.Recepcionistas.Queries.Search;

namespace SistemaClinica.API.Controllers;

[ApiController]
[Authorize(Roles = "Administrador")]
[Route("api/[controller]")]
public class RecepcionistasController(IMediator mediator) : ControllerBase
{
    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] SearchRecepcionistaQuery query, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ListRecepcionistaQuery(), cancellationToken));
    }

    [HttpGet("GetById/{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetRecepcionistaQuery { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateRecepcionistaCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response) : BadRequest(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, UpdateRecepcionistaCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteRecepcionistaCommand { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
