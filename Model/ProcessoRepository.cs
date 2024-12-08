using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProcessoRepository
{
    private readonly ConnectionContext _context;

    public ProcessoRepository(ConnectionContext context)
    {
        _context = context;
    }

    public async Task<List<ProcessoModel>> GetProcessosCompletoAsync()
    {
       return await _context.Database.SqlQuery<ProcessoModel>($@"
    SELECT version()
    ").ToListAsync();
    }

    public async Task<List<ProcessoStatus>> GetProcessosStatusAsync()
    {
        return await _context.Database.SqlQuery<ProcessoStatus>($@"
        SELECT version()
        ").ToListAsync();
    }


}
