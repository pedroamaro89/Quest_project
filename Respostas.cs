using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUEST
{
    public class Respostas
    {
        public int IdResposta { get; set; }
        public int Nome { get; set; }
        public int Posicao { get; set; }


        public int Resposta1 { get; set; }
        public int Resposta2 { get; set; }
        public int Resposta3 { get; set; }
        public int Resposta4 { get; set; }

        public Respostas()
        {
          
        }
        public Respostas(int resposta1, int resposta2, int resposta3, int resposta4)
        {
            Resposta1 = resposta1;
            Resposta2 = resposta2;
            Resposta3 = resposta3;
            Resposta4 = resposta4;

            

        }
    }
}
