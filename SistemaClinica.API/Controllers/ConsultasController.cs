using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaClinica.Application.UseCases.Consultas.Commands.Create;
using SistemaClinica.Application.UseCases.Consultas.Commands.Delete;
using SistemaClinica.Application.UseCases.Consultas.Commands.Update;
using SistemaClinica.Application.UseCases.Consultas.Queries.Get;
using SistemaClinica.Application.UseCases.Consultas.Queries.List;
using SistemaClinica.Application.UseCases.Consultas.Queries.Search;

namespace SistemaClinica.API.Controllers;

[ApiController]
[Authorize(Roles = "Administrador,Recepcionista,Medico")]
[Route("api/[controller]")]
public class ConsultasController(IMediator mediator) : ControllerBase
{
    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] SearchConsultaQuery query, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new ListConsultaQuery(), cancellationToken));
    }

    [HttpGet("GetById/{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetConsultaQuery { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpPost]
    public async Task<IActionResult> Post(CreateConsultaCommand command, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? CreatedAtAction(nameof(GetById), new { id = response.Data!.Id }, response) : BadRequest(response);
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, UpdateConsultaCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var response = await mediator.Send(command, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [Authorize(Roles = "Administrador,Recepcionista")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteConsultaCommand { Id = id }, cancellationToken);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}
