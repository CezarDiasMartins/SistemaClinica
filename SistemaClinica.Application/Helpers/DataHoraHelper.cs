namespace SistemaClinica.Application.Helpers;

public static class DataHoraHelper
{
    public static DateTime ParaLocalSemFuso(DateTime dataHora)
    {
        var dataHoraLocal = dataHora.Kind == DateTimeKind.Utc
            ? dataHora.ToLocalTime()
            : dataHora;

        return DateTime.SpecifyKind(dataHoraLocal, DateTimeKind.Unspecified);
    }

    public static DateTime? ParaLocalSemFuso(DateTime? dataHora)
    {
        return dataHora.HasValue ? ParaLocalSemFuso(dataHora.Value) : null;
    }
}
