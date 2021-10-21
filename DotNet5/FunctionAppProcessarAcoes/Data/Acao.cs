using System;
using Dapper.Contrib.Extensions;

namespace FunctionAppProcessarAcoes.Data
{
    [Table("Acoes")]
    public class Acao
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime DataReferencia { get; set; }
        public double? Valor { get; set; }
        public string CodCorretora { get; set; }
        public string NomeCorretora { get; set; }
    }
}