using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QUEST
{
    public class Questionario
    {
        public int ID { get; set; }
        public DateTime Data { get; set; }

        public int Nivel { get; set; }
        public int Assunto { get; set; }

        public List<Questions> Perguntas { get; set; }
        static Random rnd = new();

        public Questionario(int id, DateTime data, int nivel, int assunto, List<Questions> perguntas) 
        {
        ID = id;
        Data = data;
        Nivel = nivel;
        Assunto = assunto;
        Perguntas = perguntas;
        }


      
        internal static List<Questions> GetPerguntas(int subjectq, int levelq)
        {
            var todasPerguntas = Questions.GetQuestionsJson(@"perguntas\questions.json");     
            var perguntasFiltradas = todasPerguntas.FindAll(x => (x.Assunto == subjectq) && (x.Nivel == levelq) && (x.Onlyquest == 1));
            var perguntasFinal = new List<Questions>();
            Questions perguntaRnd;

            for (int i = 0; i < 5; i++)
            {
                Random R = new Random();

                int randomquestion = 0;
                randomquestion = R.Next(0, perguntasFiltradas.Count());
                perguntaRnd = perguntasFiltradas.ElementAt(randomquestion);
                perguntasFinal.Add(perguntaRnd);
                perguntasFiltradas.RemoveAt(randomquestion);

            }

            return perguntasFinal;
        }

        public static Questionario GetRandomQuestions()
        {
            Func.ColorMessage("Digite o assunto que deseja?");
            int subjectq = Func.ReadInt("1 -> Cinema \n2 -> Música \n3 -> Cultura Geral");
            Console.Clear();
            Func.ColorMessage("Digite o nível que deseja?");
            int levelq = Func.ReadInt("1 -> Básico \n2 -> Intermédio \n3 -> Avançado");
            Console.Clear();
            var perguntas = GetPerguntas(subjectq, levelq);
            Questionario questionario = new Questionario(1, DateTime.Now, levelq, subjectq, perguntas);
            return questionario;
        }
    }
}
