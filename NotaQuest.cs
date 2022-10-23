using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUEST
{
    public class NotaQuest
    {
        public int Id { get; set; }
        //public int IdAluno { get; set; }
  
        public int Nivel { get; set; }
        public int Assunto { get; set; }
        public double Resultado { get; set; }
        public DateTime DataRealizacao { get; set; }

        public NotaQuest(int id, int nivel, int assunto, double resultado, DateTime datarealizacao)
            {
            Id = id;
            Nivel = nivel;
            Assunto = assunto;
            Resultado = resultado;
            DataRealizacao = datarealizacao;
            }
    }
   


}
