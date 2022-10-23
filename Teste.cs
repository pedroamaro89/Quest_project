using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QUEST
{
    public class Teste
    {
        public int ID { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int Assunto { get; set; }
        public int Nivel { get; set; }
        public int PergRespRandom { get; set; }
        public string TagEspecifico { get; set; }


        public List<Questions> Perguntas { get; set; }
        public List<int> Alunos { get; set; }

        public Teste()
        {


        }
        public Teste(int iD, DateTime datainicio, DateTime datafim, int assunto, int nivel, List<Questions> perguntas, List<int> alunos, int pergresprandom)
        {
            ID = iD;
            DataInicio = datainicio;
            DataFim = datafim;
            Assunto = assunto;
            Nivel = nivel;
            Perguntas = perguntas;
            Alunos = alunos;
            PergRespRandom = pergresprandom;
        }
        public Teste(int iD, DateTime datainicio, DateTime datafim, int assunto, int nivel, List<Questions> perguntas, List<int> alunos, int pergresprandom, string tagEspecifico)
        {
            ID = iD;
            DataInicio = datainicio;
            DataFim = datafim;
            Assunto = assunto;
            Nivel = nivel;
            Perguntas = perguntas;
            Alunos = alunos;
            PergRespRandom = pergresprandom;
            TagEspecifico = tagEspecifico;
        }

        internal static List<Questions> GetPerguntasRnd(int subjectq, int levelq)
        {
            var todasPerguntas = Questions.GetQuestionsJson(@"perguntas\questions.json");
            var perguntasFiltradas = todasPerguntas.FindAll(x => (x.Assunto == subjectq) && (x.Nivel == levelq) && (x.Onlyquest == 2));


            if (perguntasFiltradas.Count == 0)
                if (perguntasFiltradas.Count == 0)
                {
                    int ans = Func.ReadInt("Com os parâmtros escolhidos não existe qualquer questão. Deseja adicionar questões?\n1 -> Sim\n2 -> Não, prefiro criar um teste com outros parâmetros.");
                    Func.WhileLoopInt("Com os parâmtros escolhidos não existe qualquer questão. Deseja adicionar questões?\n1 -> Sim\n2 -> Não, prefiro criar um teste com outros parâmetros.", ref ans, 2, 1);
                    if (ans == 1)
                        Questions.NewQuestion();
                    else
                        CreateTesteRandom();

                }

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
        internal static List<Questions> GetPerguntasManual(int subjectq, int levelq)
        {
            var todasPerguntas = Questions.GetQuestionsJson(@"perguntas\questions.json");
            var perguntasFiltradas = todasPerguntas.FindAll(x => (x.Assunto == subjectq) && (x.Nivel == levelq) && (x.Onlyquest == 2));

            if (perguntasFiltradas.Count == 0)
            {
                int ans = Func.ReadInt("Com os parâmtros escolhidos não existe qualquer questão. Deseja adicionar questões?\n1 -> Sim\n2 -> Não, prefiro criar um teste com outros parâmetros.");
                Func.WhileLoopInt("1 -> Sim\n2 -> Não, prefiro criar um teste com outros parâmetros.", ref ans, 2, 1);

                if (ans == 1)
                    Questions.NewQuestion();
                else
                    CreateTesteManual();

            }


            var perguntasFiltradasFinal = new List<Questions>();

            for (int i = 0; i < perguntasFiltradas.Count; i++)
            {
                var question = perguntasFiltradas.ElementAt(i);
                perguntasFiltradasFinal.Add(question);

            }

            return perguntasFiltradasFinal;
        }

        public static void Geral(ref int subject, ref int level, ref DateTime dataInicial, ref DateTime dataFinal, ref int ifrandom, ref int tagyesno)
        {
            Func.ColorMessage("Digite o assunto que deseja?");
            subject = Func.ReadInt("1 -> Cinema \n2 -> Música \n3 -> Cultura Geral");
            Func.WhileLoopInt("Digite o assunto sobre o qual deseja realizar o teste.\n1 - Cinema \n2 - Musica \n3 - Cultura Geral", ref subject, 3, 1);
            Func.ColorMessage("Digite o nível que deseja?");
            level = Func.ReadInt("1 -> Básico \n2 -> Intermédio \n3 -> Avançado");
            Func.WhileLoopInt("Digite o nível sobre o qual deseja realizar o teste.\n1 - Basico \n2 - Intermédio \n3 - Avançado", ref level, 3, 1);
            Func.ColorMessage("Deseja que a ordem das questões e das respostas seja aleatória?");
            ifrandom = Func.ReadInt("1 -> Sim \n2 -> Não");
            Func.WhileLoopInt("Deseja que a ordem das questões e das respostas seja aleatória? \n1 -> Sim \n2 -> Não", ref ifrandom, 2, 1);
            var sub = Func.getdescnivelassunto(subject, level);

            bool valid = true;

            do
            {
                Func.ColorMessage("Data inícial");
                Console.WriteLine($"Introduza a data e hora inicial em que o teste fica disponível (Ex: 10/12/2022 09:30).\nSe a data inicial for a partir de agora digite 'atual'");
                var ansDataI = Console.ReadLine();
                if (ansDataI == "atual")
                {
                    dataInicial = DateTime.Now;
                    valid = true;
                }
                else
                {
                    try
                    {
                        dataInicial = DateTime.Parse(ansDataI);
                        valid = true;
                    }
                    catch (Exception)
                    {
                        valid = false;
                        Func.ErrorColorMessage("Erro : A data tem que ser formato 10/22/2020 09:30, ou 'atual'");
                        //throw;
                    }

                }

                Func.ColorMessage("Data final");
                Console.WriteLine("Introduza a data e hora em que termina a disponibilidade do teste (Ex: 10/12/2022 09:30) \nSe a data inicial for a partir de agora digite 'atual'");
                var ansDataF = Console.ReadLine();

                if (ansDataF == "atual")
                {
                    dataFinal = DateTime.Now;
                    valid = true;
                }
                else
                {
                    //dataFinal = DateTime.Parse(ansDataF);
                    try
                    {
                        dataFinal = DateTime.Parse(ansDataF);
                        valid = true;
                    }
                    catch (Exception)
                    {
                        valid = false;
                        Func.ErrorColorMessage("Erro : A data tem que ser formato 10/22/2020 09:30, ou 'atual'\n");
                        throw;
                    }
                }

                if (dataInicial > dataFinal)
                {
                    valid = false;
                    Console.Clear();
                    Func.ErrorColorMessage("A data de início não pode ser superior que a data final. Insira de novo");
                }
            } while (valid==false);
          
            Console.Clear();

            Func.ColorMessage("Deseja ver :");
            tagyesno = Func.ReadInt($"1 -> Todas as questões relativas ao assunto {sub.assunto}\n2 -> Escolher um TAG relativo ao assunto {sub.assunto}, e escolher questões relacionadas");
            Func.WhileLoopInt("Deseja ver :\n1 -> Todas as questões relativas ao assunto {sub.assunto}\n2 -> Escolher um TAG relativo ao assunto {sub.assunto}, e escolher questões relacionadas", ref tagyesno, 2, 1);

        }

        public static void CreateTesteManual()
        {

            int subject = 0, level = 0, ifrandom = 0, tagyesno = 0;
            DateTime dataInicial = DateTime.MinValue, dataFinal = DateTime.MinValue;
            Geral(ref subject, ref level, ref dataInicial, ref dataFinal, ref ifrandom, ref tagyesno);
            var listaperguntas = GetPerguntasManual(subject, level);
            string tagespecifico = "";

            if (tagyesno == 1)
            {
                Func.ColorMessage("Perguntas disponíveis para teste :");

                for (int i = 0; i < listaperguntas.Count; i++)
                {
                    Console.WriteLine($"{i +1} -> {listaperguntas[i].Nome}");
                    Console.WriteLine();
                }

            }
            else
            {
                tagespecifico = PerguntasTag(tagyesno, subject, level);
            }

            string perguntasescolhidas = Func.ReadString("Digite os números das questões que pretende para o teste. O teste tem que ter 5 questões!");

            var final = perguntasescolhidas.Split(',').Select(int.Parse).ToList();

            bool valid = true;


            Console.WriteLine();
            do
            {

                for (int i = 0; i < final.Count; i++)   ///////////////////////////perguitna 1 passa para 0
                {
                    if (final[i] > listaperguntas.Count)
                    {
                        valid = false;
                        break;
                    } else 
                    { 
                        valid = true;
                        final[i] = final[i] - 1;
                    }

                }

                if (valid==false)
                {
                    Func.ErrorColorMessage("Inseriu uma questão que não está na lista. Digite de novo");
                    perguntasescolhidas = Func.ReadString("Digite os números das questões que pretende para o teste. O teste tem que ter 5 questões!");
                    final = perguntasescolhidas.Split(',').Select(int.Parse).ToList();
                }
                

            } while (valid == false);



            while (final.Count != 5)
            {
                Func.ErrorColorMessage($"Adicionou {final.Count} questões. O teste para ser válido tem que ter 5 questões. Por favor digite de novo.");
                perguntasescolhidas = Func.ReadString("Digite os números das questões que pretende para o teste. O teste tem que ter 5 questões!");
                final = perguntasescolhidas.Split(',').Select(int.Parse).ToList();
            }

            var newteste = new List<Questions>();

            for (int i = 0; i < 5; i++)
            {
                Questions pergunta = listaperguntas.ElementAt(final.ElementAt(i));
                newteste.Add(pergunta);
            }


            var alunos = Aluno.GetAlunosJson(@"Alunos\alunos.json");
            //alunos = alunos.OrderBy(c=> c.ID).ToList();

            List<int> alunosValid = new List<int>();
            List<int> alunosValidFinal = new List<int>();

            Func.ColorMessage("Estes são os alunos válidos para realizarem o teste");

            foreach (var item in alunos)
            {
                if (item.NotaQuest != null && item.NotaQuest.Any(r => r.Resultado >= 80 && r.Assunto == subject && r.Nivel == level))
                {

                    Console.WriteLine($"ID : {item.ID}  |  Nome : {item.Name}");

                    alunosValid.Add(item.ID);
                }
            }

            if (alunosValid.Count == 0)
            {
                Func.ErrorColorMessage("Não existem alunos disponíveis para realizar o teste");
                Console.WriteLine();
                Cons.MenuProf();
                return;
            }
            Console.WriteLine();

            Func.ColorMessage("Escolha uma das opções :");
            var ans = Func.ReadInt("1 -> Adicionar todos os alunos ao teste\n2 - Desejo selecionar os alunos a adicionar");
            Func.WhileLoopInt("Deseja adicioná-los todos ao teste criado?\n1 -Sim\n2 - Não", ref ans, 2, 1);

            if (ans == 1)
            {
                alunosValidFinal = alunosValid;
            }

            else
            {
                var ans2 = Func.ReadString("Digite o número dos alunos que vão a teste.");
                alunosValidFinal = ans2.Split(',').Select(int.Parse).ToList();
            }


            var sublev = Func.getdescnivelassunto(subject, level);

            Console.Clear();
            Func.ColorMessage("Aqui fica o teste que criou: ");
            Console.WriteLine($"Assunto : {sublev.assunto}  |  Nível : {sublev.nivel}");
            Console.WriteLine();
            Func.ColorMessage("Questões :");
            Console.WriteLine();

            foreach (var item in newteste)
            {
                Console.WriteLine($"{item.Nome}");
            }

            Func.ColorMessage("Alunos :");
            Console.WriteLine();
            foreach (var item in alunos)
            {
                foreach (var x in alunosValidFinal)
                {
                    if (item.ID == x)
                        Console.WriteLine($"ID : {item.ID}  |  Nome : {item.Name}");
                }
            }

            var testesDisponiveis = GetTestesJson(@"Testes\testes.json");

            Teste teste = new Teste(testesDisponiveis.Count + 1, dataInicial, dataFinal, subject, level, newteste, alunosValidFinal, ifrandom, tagespecifico);


            Console.WriteLine();
            Func.ColorMessage("Deseja guardar este teste?");
            int ans3 = Func.ReadInt("Deseja guardar este teste?\n1 -> Sim\n2 -> Não, prefiro criar um novo");
            Func.WhileLoopInt("1 -> Sim\n2 -> Não, prefiro criar um novo", ref ans3, 2, 1);

            if (ans3 == 2)
            {
                Console.Clear();
                CreateTesteManual();
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                Cons.MenuProf();
                return;
            }


            testesDisponiveis.Add(teste);
            Serealize(@"Testes\testes.json", testesDisponiveis);

            //Serealize(@"Testes\testes.json", testesDisponiveis);              
        }



        public static void CreateTesteRandom()
        {
            int subject = 0, level = 0, ifrandom = 0, tagyesno = 0;
            DateTime dataInicial = new DateTime(), dataFinal = new DateTime();

            Geral(ref subject, ref level, ref dataInicial, ref dataFinal, ref ifrandom, ref tagyesno);

            var alunos = Aluno.GetAlunosJson(@"Alunos\alunos.json");

            List<int> alunosValid = new List<int>();
            List<int> alunosValidFinal = new List<int>();


            var testesDisponiveis = GetTestesJson(@"Testes\testes.json");    


            if (tagyesno == 2)
            {
                var todasPerguntas = Questions.GetQuestionsJson(@"perguntas\questions.json");
                List<string> perguntastag = new List<string>();
                var perguntasFiltradas = todasPerguntas.FindAll(x => (x.Assunto == subject) && (x.Nivel == level) && (x.Onlyquest == 2));

                foreach (var item in perguntasFiltradas)
                {
                    string tags = item.Tag;
                    perguntastag.Add(tags);
                }

                List<string> listatags = perguntastag.Distinct().ToList(); /////lista sem duplicados

                Func.ColorMessage("Tags disponíveis :");

                for (int i = 0; i < listatags.Count; i++)
                {
                    Console.WriteLine($"{i + 1} -> {listatags[i]}");
                }
                Console.WriteLine();

                var tag = Func.ReadInt("Digite o número relativo à tag sobre a qual deseja efetuar o teste");
                Func.WhileLoopInt("Digite o número relativo à tag sobre a qual deseja efetuar o teste", ref tag, listatags.Count + 1, 1);
                Console.Clear();

                var perguntasfiltradastag = todasPerguntas.FindAll(x => (x.Assunto == subject) && (x.Nivel == level) && (x.Onlyquest == 2) && x.Tag == listatags[tag - 1]);

                var perguntasrandomtag = new List<Questions>();
                Questions perguntaRnd;

                for (int i = 0; i < 5; i++)
                {
                    Random R = new Random();

                    int randomquestion = 0;
                    randomquestion = R.Next(0, perguntasfiltradastag.Count());
                    perguntaRnd = perguntasfiltradastag.ElementAt(randomquestion);
                    perguntasrandomtag.Add(perguntaRnd);
                    perguntasfiltradastag.RemoveAt(randomquestion);
                }

                var sublev = Func.getdescnivelassunto(subject, level);

                Console.WriteLine($"Assunto : {sublev.assunto}  |  Nível : {sublev.nivel}  |  Tag : {listatags[tag - 1]}");
                Console.WriteLine();
                Func.ColorMessage("Questões");
                foreach (var item in perguntasrandomtag)
                {
                    Console.WriteLine(item.Nome);
                    Console.WriteLine();
                }
                Teste teste = new Teste(testesDisponiveis.Count + 1, dataInicial, dataFinal, subject, level, perguntasrandomtag, alunosValidFinal, ifrandom, listatags[tag - 1]);
                testesDisponiveis.Add(teste);                                    /////////////// Adiciona o teste

            }

            else
            {
               var perguntas = GetPerguntasRnd(subject, level);

                var sublev = Func.getdescnivelassunto(subject, level);

                Console.WriteLine($"Assunto : {sublev.assunto}  |  Nível : {sublev.nivel}");
                Console.WriteLine();
                Func.ColorMessage("Questões");
                foreach (var item in perguntas)
                {
                    Console.WriteLine(item.Nome);
                    Console.WriteLine();
                }

                Teste teste = new Teste(testesDisponiveis.Count + 1, dataInicial, dataFinal, subject, level, perguntas, alunosValidFinal, ifrandom);
                testesDisponiveis.Add(teste);                           
            }

            Func.ColorMessage("Alunos válidos para realizar o teste :");

            foreach (var item in alunos)
            {

                if (item.NotaQuest != null && item.NotaQuest.Any(r => r.Resultado >= 80 && r.Assunto == subject && r.Nivel == level))
                {
                    Console.WriteLine($"ID : {item.ID}  |  Nome : {item.Name}");
                    alunosValid.Add(item.ID);
                }
            }

            if (alunosValid.Count == 0)
            {
                Func.ErrorColorMessage("Não existem alunos disponíveis para realizar o teste");
                return;
            }

            Console.WriteLine();
            Func.ColorMessage("Escolha uma das opções :");
            var ans = Func.ReadInt("1 -> Adicionar todos os alunos ao teste\n2 - Desejo selecionar os alunos a adicionar");
            Func.WhileLoopInt("Estes são os alunos válidos para realizarem o teste. Deseja adicioná-los todos ao teste criado?\n1 -Sim\n2 - Não", ref ans, 2, 1);
            Console.WriteLine();

            if (ans == 1)
            {
                alunosValidFinal = alunosValid;
            }
            else
            {
                var ans2 = Func.ReadString("Digite o número dos alunos que vão a teste.");
                alunosValidFinal = ans2.Split(',').Select(int.Parse).ToList();
            }

            Console.Clear();

            Func.ColorMessage("Deseja guardar este teste?");
            int ans3 = Func.ReadInt("1 -> Sim\n2 -> Não, prefiro criar um novo");
            Func.WhileLoopInt("1 -> Sim\n2 -> Não, prefiro criar um novo", ref ans3, 2, 1);

            if (ans3 == 2)
            {
                Console.Clear();
                CreateTesteRandom();
                return;

            }
            else
            {
                Console.Clear();
                Cons.MenuProf();
                return;
            }

            Serealize(@"Testes\testes.json", testesDisponiveis);            
        }


        public static void VerTestes()
        {
            List<int> testeid = new List<int>();
            var testes = GetTestesJson(@"Testes\testes.json");
            foreach (var item in testes)
            {
                var getsublev = Func.getdescnivelassunto(item.Assunto, item.Nivel);

                Console.WriteLine($"ID : {item.ID}  |  Assunto : {getsublev.assunto}  |  Nível : {getsublev.nivel}");
                testeid.Add(item.ID);
            }
            Console.WriteLine();
            var ans = Func.ReadInt("Digite o ID do teste que deseja visualizar");

            while (!testeid.Contains(ans))
            {

                ans = Func.ReadInt("Inseriu um ID inválido. Insira de novo");

            }

            Console.WriteLine();
            Console.Clear();

            bool temAlunos = false;
            var teste = testes.Find(x => x.ID == ans);

            Func.ColorMessage("Questões");
            foreach (var item in teste.Perguntas)
            {
                Console.WriteLine(item.Nome);
            }
            Console.WriteLine();
            Func.ColorMessage("Resultados");

            foreach (var item in teste.Alunos)
            {
                var aluno = Aluno.GetAlunoById(@"Alunos\alunos.json", item);

                if (aluno.NotaTeste != null)
                {

                    Console.WriteLine($"Nome: {aluno.Name}  | IdAluno: {aluno.ID}");
                    foreach (var item1 in aluno.NotaTeste)
                    {
                        if (item1.IdTeste == ans)
                        {
                            Console.WriteLine($"Teste: {item1.IdTeste}  |  Resultado: {item1.Resultado}");
                            temAlunos = true;
                        }

                    }

                }
                else
                {
                    temAlunos = false;
                }
            }

            if (temAlunos == false)
            {
                Console.WriteLine();
                Console.Clear();
                Func.ErrorColorMessage("Este teste ainda não foi realizado por qualquer aluno.");
                Console.WriteLine();
                VerTestes();

                return;
            }

            Console.WriteLine();

            var ans2 = Func.ReadInt("Deseja ver mais testes?\n1 -> Sim\n2 - Não");
            Func.WhileLoopInt("Deseja ver mais testes?\n1 -> Sim\\n2 - Não", ref ans2, 2, 1);
            if (ans2 == 1)
            {
                VerTestes();
                Console.Clear();
            }
            else
            {
                Console.Clear();
                Console.WriteLine();
                Cons.MenuProf();
                return;
            }

        }

        public static void Serealize(string path, List<Teste> testes)
        {
            var jsonOptions = new JsonSerializerOptions(); ////////////// isto tudo guarda as perguntas em json
            jsonOptions.WriteIndented = true;


            var json = JsonSerializer.Serialize(testes, jsonOptions);
            File.WriteAllText(path, json);
        }


        public static List<Teste> GetTestesJson(string path)
        {
            var jsonQuestions = File.ReadAllText(path);

            var testes = JsonSerializer.Deserialize<List<Teste>>(jsonQuestions);
            testes = testes.OrderBy(c => c.ID).ToList();
            return testes;
        }

        public static string PerguntasTag(int tagyesno, int subject, int level)
        {
            List<string> listatags = new List<string>();
            int tag = 0;
            if (tagyesno == 2)
            {
                var todasPerguntas = Questions.GetQuestionsJson(@"perguntas\questions.json");
                List<string> perguntastag = new List<string>();
                var perguntasFiltradas = todasPerguntas.FindAll(x => (x.Assunto == subject) && (x.Nivel == level) && (x.Onlyquest == 2));


                foreach (var item in perguntasFiltradas)
                {

                    string tags = item.Tag;
                    perguntastag.Add(tags);
                }

                listatags = perguntastag.Distinct().ToList(); /////lista sem duplicados

                //listatags.ForEach(i => Console.WriteLine($"{i}"));

                Func.ColorMessage("Tags disponíveis :");

                for (int i = 0; i < listatags.Count; i++)
                {
                    Console.WriteLine($"{i + 1} -> {listatags[i]}");
                }
                Console.WriteLine();

                tag = Func.ReadInt("Digite o número relativo à tag sobre a qual deseja efetuar o teste");
                Func.WhileLoopInt("Digite o número relativo à tag sobre a qual deseja efetuar o teste", ref tag, listatags.Count + 1, 1);
                Console.Clear();

                var perguntasfiltradastag = todasPerguntas.FindAll(x => (x.Assunto == subject) && (x.Nivel == level) && (x.Onlyquest == 2) && x.Tag == listatags[tag - 1]);

                Func.ColorMessage($"Perguntas disponíveis para teste com a tag {listatags[tag - 1]} :");
                for (int i = 0; i < perguntasfiltradastag.Count; i++)
                {
                    Console.WriteLine($"{i} -> {perguntasfiltradastag[i].Nome}");
                    Console.WriteLine();
                }
            }

            return listatags[tag - 1];

        }

        public static void TestePrint(int subject, int level, List<Questions> teste, List<Aluno> alunos, List<int> alunosvalidfinal)
        {
            var sublev = Func.getdescnivelassunto(subject, level);

            Console.Clear();
            Func.ColorMessage("Aqui fica o teste que criou: ");
            Console.WriteLine($"Assunto : {sublev.assunto}  |  Nível : {sublev.nivel}");
            Console.WriteLine();
            Func.ColorMessage("Questões :");
            Console.WriteLine();
            foreach (var item in teste)
            {
                Console.WriteLine($"{item.Nome}");
            }
            Func.ColorMessage("Alunos :");
            Console.WriteLine();
            foreach (var item in alunos)
            {
                foreach (var x in alunosvalidfinal)
                {
                    if (item.ID == x)
                        Console.WriteLine($"ID : {item.ID}  |  Nome : {item.Name}");
                }
            }
        }

    }
}
