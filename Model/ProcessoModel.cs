
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ProcessoModel
    {
        public int NroPro { get; set; }                    // Número do processo
        public int AnoPro { get; set; }                    // Ano do processo
        public string NomeExportador { get; set; }         // Nome da empresa exportadora
        public string NomeImportador { get; set; }         // Nome da empresa importadora
        public string NomeUsuario { get; set; }            // Nome do usuário responsável pelo processo
        public DateTime? DataConfirmacaoEmbarque { get; set; }  // Data de confirmação de embarque (nullable)
        public DateTime? DataConfirmacaoChegada { get; set; }  // Data de confirmação de chegada (nullable)
    }

}
