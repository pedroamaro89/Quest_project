using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QUEST
{
    public class Questions
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public int Assunto { get; set; }
        public int Nivel { get; set; }
        public string Tag { get; set; }

        public int Tipo { get; set; }
        public int Onlyquest { get; set; }
        //public string Respostas { get; set; }
        public string Resposta1 { get; set; }
        public string Resposta2 { get; set; }
        public string Resposta3 { get; set; }
        public string Resposta4 { get; set; }



        public List<int> Valid { get; set; }

        public Questions() //para o deserialize json
        {
        }

        public Questions(int id, int assunto, int nivel, string tag, string nome, int tipo, int onlyquest, string resposta1) //yes no
        {
            Id = id;
            Nome = nome;
            Assunto = assunto;
            Nivel = nivel;
            Tag = tag;
            Tipo = tipo;
            Onlyquest = onlyquest;
            Resposta1 = resposta1;


        }


        public Questions(int id, int assunto, int nivel, string tag, string nome, int tipo, int onlyquest, string resposta1, string resposta2, string resposta3, string resposta4, List<int> valid) //checkbox
        {
            Id = id;
            Nome = nome;
            Assunto = assunto;
            Nivel = nivel;
            Tag = tag;
            Tipo = tipo;
            Onlyquest = onlyquest;
            Resposta1 = resposta1;
            Resposta2 = resposta2;
            Resposta3 = resposta3;
            Resposta4 = resposta4;
            Valid = valid;
        }
        public Questions(int id, int assunto, int nivel, string tag, string nome, int tipo, int onlyquest, string resposta1, string resposta2, string resposta3, List<int> valid) //dropdown
        {
            Id = id;
            Nome = nome;
            Assunto = assunto;
            Nivel = nivel;
            Tag = tag;
            Tipo = tipo;
            Onlyquest = onlyquest;
            Resposta1 = resposta1;
            Resposta2 = resposta2;
            Resposta3 = resposta3;
            Valid = valid;

        }

        public Questions(int id, int assunto, int nivel, string tag, string nome, int tipo, int onlyquest)
        {
            Id = id;
            Nome = nome;
            Assunto = assunto;
            Nivel = nivel;
            Tag = tag;
            Tipo = tipo;
            Onlyquest = onlyquest;
        }

        public static void Serealize(string path, List<Questions> list)
        {
            var jsonOptions = new JsonSerializerOptions(); ////////////// isto tudo guarda as perguntas em json
            jsonOptions.WriteIndented = true;


            var json = JsonSerializer.Serialize(list, jsonOptions);
            File.WriteAllText(path, json);
        }

        public static List<Questions> GetQuestionsJson(string path)
        {
            var jsonQuestions = File.ReadAllText(path);

            var questions = JsonSerializer.Deserialize<List<Questions>>(jsonQuestions); /////ler file e guarda informcao numa lista
            return questions;
        }

        public static void NewQuestion()
        {

            var questions1 = GetQuestionsJson(@"perguntas\questions.json");

            Func.ColorMessage("Digite o nível que deseja?");
            int getlevel = Func.ReadInt("1 - Básico\n2 - Intermédio \n3 - Avançado");
            Console.Clear();

            while (getlevel > 3 || getlevel < 1)
            {
                Console.WriteLine("Digite uma opção válida por favor");
                Console.WriteLine();
                getlevel = Func.ReadInt("1 - Básico \n2 - Intermédio \n3 - Avançado");
                Console.Clear();

            }

            string gettag = Func.ReadString("Escreva uma TAG relacionada com a questão a colocar");


            Func.ColorMessage("Digite o assunto que deseja");
            int getsubjet = Func.ReadInt("1 - Cinema \n2 - Musica \n3 - Cultura Geral");
            Console.Clear();

            while (getsubjet > 3 || getsubjet < 1)
            {
                Console.WriteLine("Digite uma opção válida por favor");
                Console.WriteLine();
                getsubjet = Func.ReadInt("1 - Cinema n2 - Musica \n3 - Cultura Geral");
                Console.Clear();
            }

            Func.ColorMessage("Digite a opção que deseja");
            int getifonlyquest = Func.ReadInt("1 - A pergunta pode ser utilizada tanto em testes como em questionários \n2 - A pergunta apenas pode ser utilizada em testes");
            Console.Clear();
            while (getifonlyquest > 2 || getifonlyquest < 0)
            {
                Console.WriteLine("Digite uma opção válida por favor");
                Console.WriteLine();
                getifonlyquest = Func.ReadInt("1 - A pergunta pode ser utilizada tanto em testes como em questionários \n2 - A pergunta apenas pode ser utilizada em testes"); Console.Clear();

            }

            //string getquestion = Func.ReadString("Escreva a pergunta");

            QuestionAnswer(questions1, getsubjet, getlevel, gettag, getifonlyquest);


        }


        public static void QuestionAnswer(List<Questions> questions1, int getsubjet, int getlevel, string gettag, int getifonlyquest)
        {
            string getquestion = Func.ReadString("Escreva a pergunta");

            Func.ColorMessage("Digite o tipo que deseja?");
            int gettype = Func.ReadInt("1 - Checkbox (uma ou mais respostas certas) \n2 - Dropdown (uma única resposta certa) \n3 - YesNo (Uma opção (Sim/Não)");
            Console.Clear();

            while (gettype > 3 || gettype < 1)
            {
                Console.WriteLine("Digite uma opção válida por favor");
                Console.WriteLine();
                gettype = Func.ReadInt("1 - Checkbox (uma ou mais respostas certas) \n2 - Dropdown (uma única resposta certa) \n3 - YesNo (Uma opção (Sim/Não)");
                Console.Clear();

            }

            //string getanswer = Func.ReadString("Escreva a resposta");
            if (gettype == 1)
            {
                List<int> checkboxvalid = new List<int>();

                //int[] certas = new int[4] { 0, 0, 0, 0 };
                Console.WriteLine(getquestion);
                string firstans = Func.ReadString("Escreva a primeira resposta");
                int ifvalid1 = Func.ReadInt("Esta resposta é 1 - correta; 2 - errada");
                Func.WhileLoopInt("Esta resposta é 1 - correta; 2 - errada", ref ifvalid1, 2, 0);

                if (ifvalid1 == 1)
                {
                    //certas[0] = 1;
                    checkboxvalid.Add(1);
                }
                string secondans = Func.ReadString("Escreva a segunda resposta");
                int ifvalid2 = Func.ReadInt("Esta resposta é 1 - correta; 2 - errada");
                Func.WhileLoopInt("Esta resposta é 1 - correta; 2 - errada", ref ifvalid2, 2, 0);

                if (ifvalid2 == 1)
                {
                    //certas[1] = 1;
                    checkboxvalid.Add(2);
                }
                string thirdtans = Func.ReadString("Escreva a terceira resposta");
                int ifvalid3 = Func.ReadInt("Esta resposta é 1 - correta; 2 - errada");
                Func.WhileLoopInt("Esta resposta é 1 - correta; 2 - errada", ref ifvalid3, 2, 0);

                if (ifvalid3 == 1)
                {
                    //certas[2] = 1;
                    checkboxvalid.Add(3);
                }
                string fourthans = Func.ReadString("Escreva a quarta resposta");
                int ifvalid4 = Func.ReadInt("Esta resposta é 1 - correta; 2 - errada");
                Func.WhileLoopInt("Esta resposta é 1 - correta; 2 - errada", ref ifvalid4, 2, 0);

                if (ifvalid4 == 1)
                {
                    //certas[3] = 1;
                    checkboxvalid.Add(4);
                }


                Questions checkboxquestions = new Questions(questions1.Count + 1, getsubjet, getlevel, gettag, getquestion, gettype, getifonlyquest, firstans, secondans, thirdtans, fourthans, checkboxvalid);

                questions1.Add(checkboxquestions);

                Questions.Serealize(@"perguntas\questions.json", questions1);

                QuestaoInserida();

            }

            if (gettype == 2)
            {
                List<int> dropdownvalid = new List<int>();

                //int[] certas = new int[3] { 0, 0, 0 };


                Console.WriteLine(getquestion);
                string firstans = Func.ReadString("Escreva a primeira resposta");
                int ifvalid1 = Func.ReadInt("Esta resposta é 1 - correta; 2 - errada");
                if (ifvalid1 == 1)
                {
                    //certas[0] = 1;
                    dropdownvalid.Add(1);
                }
                string secondans = Func.ReadString("Escreva a segunda resposta");
                int ifvalid2 = Func.ReadInt("Esta resposta é 1 - correta; 2 - errada");
                if (ifvalid2 == 1)
                {
                    //certas[0] = 1;
                    dropdownvalid.Add(2);
                }
                string thirdtans = Func.ReadString("Escreva a terceira resposta");
                int ifvalid3 = Func.ReadInt("Esta resposta é 1 - correta; 2 - errada");
                if (ifvalid3 == 1)
                {
                    //certas[0] = 1;
                    dropdownvalid.Add(3);
                }
                Questions dropdownquestions = new Questions(questions1.Count + 1, getsubjet, getlevel, gettag, getquestion, gettype, getifonlyquest, firstans, secondans, thirdtans, dropdownvalid);
                questions1.Add(dropdownquestions);

                Questions.Serealize(@"perguntas\questions.json", questions1);
               
                QuestaoInserida();
              
            }

            if (gettype == 3)
            {
                List<int> yesnovalid = new List<int>();
                int[] certas = new int[] { 0, 0 };
                Console.WriteLine(getquestion);
                int ifvalid = Func.ReadInt("1 - SIM -> resposta correta\n2 - NÃO -> resposta correta");

                Questions yesnoquestions = new Questions(questions1.Count + 1, getsubjet, getlevel, gettag, getquestion, gettype, getifonlyquest, ifvalid.ToString());
                questions1.Add(yesnoquestions);

                Questions.Serealize(@"perguntas\questions.json", questions1);

                QuestaoInserida();


            }


        }

        public static void QuestaoInserida()
        {
            Console.Clear();
            Console.WriteLine("A sua questão foi inserida!");
            Console.WriteLine();
            Func.ColorMessage("Escolha uma opção :");
            var ans = Func.ReadInt("1 -> Adicionar mais questões \n2 -> Voltar ao Menu Inicial");
            Func.WhileLoopInt("1 -> Adicionar mais questões \n2 -> Voltar ao Menu Inicial", ref ans, 2, 1);
            if (ans == 1)
            {
                Console.Clear();
                NewQuestion();
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                Cons.MenuProf();
                return;
            }
        }


    }



}
