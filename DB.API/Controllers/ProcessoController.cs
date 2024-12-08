using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProcessoController : ControllerBase
{
    private readonly string _connectionString;

    public ProcessoController(IConfiguration configuration)
    {
        _connectionString = "Server=localhost;Port=3306;Database=desafio_idata;User=root;Password=";
    }

    [HttpGet]
    [Route("version")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetVersion()
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            using var command = new MySqlCommand("SELECT VERSION();", connection);
            var version = await command.ExecuteScalarAsync();
            return Ok(new { DatabaseVersion = version.ToString() });
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message);
        }
    }

    [HttpGet]
    [Route("processos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProcessos()
    {
        try
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = @"
                SELECT 
                processo.*,
                emp_exportador.razaosocial_emp AS nome_exportador,
                emp_importador.razaosocial_emp AS nome_importador,
                processo_usuario.nome_usu AS nome_usuario,
                processotracking.nro_pro AS numero_processo,
                processotracking.ano_pro AS ano_processo
                FROM processo
                INNER JOIN empresa AS emp_exportador ON processo.cod_exportador = emp_exportador.cod_emp
                INNER JOIN empresa AS emp_importador ON processo.cod_importador = emp_importador.cod_emp
                INNER JOIN usuario AS processo_usuario ON processo.processo_usuario = processo_usuario.usuarioid
                INNER JOIN processotracking 
                ON processo.nro_pro = processotracking.nro_pro 
                AND processo.ano_pro = processotracking.ano_pro 
                AND processotracking.ordem = 1
                INNER JOIN processotracking AS pt_chegada 
                ON processo.nro_pro = pt_chegada.nro_pro 
                AND processo.ano_pro = pt_chegada.ano_pro 
                AND pt_chegada.ordem = 2";

            using var command = new MySqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();
            var processos = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var processo = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var value = reader.GetValue(i);
                    processo[reader.GetName(i)] = value == DBNull.Value ? null : value;
                }
                processos.Add(processo);
            }

            return Ok(processos);
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Database Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}