using SistemaClinica.API.Extensions;
using SistemaClinica.API.Middlewares;
using SistemaClinica.API.Services;
using SistemaClinica.API.Workers;
using SistemaClinica.Application.Extensions;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUsuarioLogadoService, UsuarioLogadoService>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendAngular", policy =>
        //policy.AllowAnyOrigin("http://localhost:4200")
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddHostedService<RelatorioConsultasDoDiaWorker>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
