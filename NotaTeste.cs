using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QUEST
{
    public class NotaTeste
    {
        public int Id { get; set; }
       // public int IdAluno { get; set; }
        public int IdTeste { get; set; }
        public int Nivel { get; set; }
        public int Assunto { get; set; }
        public double Resultado { get; set; }
        public DateTime DataRealizacao { get; set; }

        public NotaTeste(int id, int idteste, int nivel, int assunto, double resultado, DateTime dataRealizacao)
        {
            Id = id;
            IdTeste = idteste;
            Nivel = nivel;
            Assunto = assunto;
            Resultado = resultado;
            DataRealizacao = dataRealizacao;
        }
     
    }
}
