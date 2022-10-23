using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace QUEST
{
    public class Aluno
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<NotaQuest> NotaQuest { get; set; }
        public List<NotaTeste> NotaTeste { get; set; }
        //public PasswordPropertyTextAttribute PasswordProperty { get; set; }
        public Aluno()
        {
        }
        public Aluno(int id, string name)
        {
            ID = id;
            Name = name;
        }
        public Aluno(int id, string name, List<NotaQuest> notas)
        {
            ID = id;
            Name = name;
            NotaQuest = notas;
        }


        public static void AlunosInseridos()
        {
            var alunos = GetAlunosJson(@"Alunos\alunos.json");

            //List<Aluno> novosalunos = new List<Aluno>();

            Aluno Aluno6 = new Aluno(alunos.Count + 1, "Ana Maria");
            alunos.Add(Aluno6);

            Serealize(@"Alunos\alunos.json", alunos);

            var listalunos = GetAlunosJson(@"Alunos\alunos.json");
            foreach (var item in listalunos)
            {
                Console.WriteLine(item.Name + "" + item.ID);
            }
        }

        public static void Serealize(string path, List<Aluno> alunos)
        {
            var jsonOptions = new JsonSerializerOptions(); ////////////// isto tudo guarda as perguntas em json
            jsonOptions.WriteIndented = true;


            var json = JsonSerializer.Serialize(alunos, jsonOptions);
            File.WriteAllText(path, json);
        }

        public static List<Aluno> GetAlunosJson(string path)
        {
            var jsonQuestions = File.ReadAllText(path);

            var alunos = JsonSerializer.Deserialize<List<Aluno>>(jsonQuestions); /////ler file e guarda informcao numa lista
            alunos = alunos.OrderBy(c => c.ID).ToList();
            return alunos;
        }

        public static Aluno GetAlunoById(string path, int idAluno)
        {
            var jsonQuestions = File.ReadAllText(path);

            var alunos = JsonSerializer.Deserialize<List<Aluno>>(jsonQuestions); /////ler file e guarda informcao numa lista
            var aluno = alunos.Find(x => x.ID == idAluno);
            return aluno;
        }



        public static void AnsTeste(Aluno aluno)
        {
            var testesDisponiveis = Teste.GetTestesJson(@"Testes\testes.json");

            var testesFiltrados = testesDisponiveis.FindAll(r => r.Alunos.Any(r => r == aluno.ID)).ToList();

            string assunto = "", nivel = "";
            List<int> testesValid = new List<int> { };

            foreach (var item in testesFiltrados)
            {

                if (DateTime.Now >= item.DataInicio && DateTime.Now <= item.DataFim) //////verifica se o teste (item) que esta a ser verificado já existe na lista de notas do aluno! E se esta entre as data disponiveis
                {
                    if (aluno.NotaTeste == null || !aluno.NotaTeste.Any(r => r.IdTeste == item.ID))
                    {
                        var desc = Func.getdescnivelassunto(item.Assunto, item.Nivel);
                        Console.WriteLine($"Teste nº{item.ID}  |  Assunto -> {item.Assunto} : {desc.assunto}  |  Nível -> {item.Nivel} : {desc.nivel}  |  Diponível até {item.DataFim.ToShortDateString()} às {item.DataFim.ToShortTimeString()}h");
                        testesValid.Add(item.ID);
                    }
                    /*else if (!aluno.NotaTeste.Any(r => r.IdTeste == item.ID))
                    {
                        var desc = Func.getdescnivelassunto(item.Assunto, item.Nivel);
                        Console.WriteLine($"Teste nº{item.ID}  |  Assunto -> {item.Assunto} : {desc.assunto}  |  Nível -> {item.Nivel} : {desc.nivel}  |  Diponível até {item.DataFim.ToShortDateString()} às {item.DataFim.ToShortTimeString()}h");
                    }    */

                }
            }

            Console.WriteLine();

            bool validNum = true;
            int idteste = 0;

            if (testesValid.Count() == 0)
            {
                validNum = false;
                Func.ErrorColorMessage("Não tem qualquer teste para realizar");
                Cons.MenuAluno(aluno);
                return;
            };

            do
            {
                Func.ColorMessage("Deseja realizar um destes testes?");
                int ans2 = Func.ReadInt("1 -> Sim\n2 -> Não");
                Func.WhileLoopInt("1 -> Sim\n2 -> Não", ref ans2, 2,1);
                if(ans2 == 2)
                {
                    Console.Clear();
                    Cons.MenuAluno(aluno);
                }
                else
                { 
                idteste = Func.ReadInt("Digite o nº do teste que deseja efetuar");
                Console.WriteLine();
                if (!testesValid.Contains(idteste))
                {
                    validNum = false;
                    Func.ErrorColorMessage("O teste introduzido não faz parte da lista disponivel");
                }

                else
                {
                    validNum = true;
                }
                }
            } while (validNum == false);


            var getteste = testesFiltrados.Find(x => x.ID == idteste);
            int certas = 0;
            double result = 0;

            for (int i = 0; i < 5; i++)
            {

                Questions pergunta = new Questions();

                if (getteste.PergRespRandom == 1)
                {
                    Random R = new Random();
                    int randomquestion = 0;
                    randomquestion = R.Next(0, getteste.Perguntas.Count);
                    pergunta = getteste.Perguntas.ElementAt(randomquestion);

                    getteste.Perguntas.RemoveAt(randomquestion);
                }

                else
                {
                    pergunta = getteste.Perguntas.ElementAt(i);
                }

                Console.Clear();
                Console.WriteLine(pergunta.Nome);
                Console.WriteLine();

                switch (pergunta.Tipo)
                {
                    case 1:

                        List<string> respostasCheckBox = new List<string> { pergunta.Resposta1, pergunta.Resposta2, pergunta.Resposta3, pergunta.Resposta4 };
                        Func.ColorMessage("Escolha uma ou mais opções corretas!");

                        if (getteste.PergRespRandom == 1)
                        {
                            var respCopiaCB = new List<string>(respostasCheckBox);
                            List<string> respostasRndCB = new List<string>();


                            for (int k = 0; k < 4; k++)
                            {
                                Random R = new Random();
                                int randomansCB = 0;
                                randomansCB = R.Next(0, respCopiaCB.Count);
                                string nomeResposta = respCopiaCB.ElementAt(randomansCB);

                                respCopiaCB.RemoveAt(randomansCB);

                                Console.WriteLine($"{k + 1} -> " + nomeResposta);
                                respostasRndCB.Add(nomeResposta);
                            }

                            var ansCB = Console.ReadLine();
                            Func.ExitTesteCB(ref ansCB);
                            List<int> ansUserCB = ansCB.Split(',').Select(int.Parse).ToList();
                            ansUserCB.Sort();

                            bool between14 = false;

                            while (between14 == false)
                            {
                                foreach (int x in ansUserCB)
                                {
                                    if (x < 0 || x > 4)
                                    {
                                        Console.WriteLine("Escolha uma opção entre 1 e 4");
                                        ansCB = Console.ReadLine();
                                        ansUserCB = ansCB.Split(',').Select(int.Parse).ToList();
                                        ansUserCB.Sort();
                                        between14 = false;
                                    }
                                    else { between14 = true; }
                                }
                            }
                            while (ansUserCB.Count > 3)
                            {
                                Func.ErrorColorMessage($"Só pode responder no máximo a 3 questões");

                                ansCB = Console.ReadLine();
                                ansUserCB = ansCB.Split(',').Select(int.Parse).ToList();
                                ansUserCB.Sort();
                            }

                            int respostasMult = 0;

                            foreach (var item in ansUserCB)
                            {
                                string respostaUserCB = respostasRndCB.ElementAt(item - 1);

                                if (respostasCheckBox.Any(x => x == respostaUserCB))
                                {
                                    respostasMult++;
                                }
                            }

                            result = result + (respostasMult * (20d / (double)pergunta.Valid.Count));
                        }
                        else
                        {

                            for (int k = 0; k < 4; k++)
                            {
                                Console.WriteLine($"{k + 1} -> " + respostasCheckBox.ElementAt(k));
                            }

                            var ans1 = Console.ReadLine();
                            Func.ExitTesteCB(ref ans1);

                            List<int> resposta = ans1.Split(',').Select(int.Parse).ToList();
                            resposta.Sort();

                            bool entre14 = false;

                            while (entre14 == false)
                            {
                                foreach (int x in resposta)
                                {
                                    if (x < 0 || x > 4)
                                    {
                                        Console.WriteLine("Escolha uma opção entre 1 e 4");
                                        ans1 = Console.ReadLine();
                                        resposta = ans1.Split(',').Select(int.Parse).ToList();
                                        resposta.Sort();
                                        entre14 = false;
                                    }
                                    else { entre14 = true; }
                                }
                            }

                            while (resposta.Count > 3)
                            {
                                Func.ErrorColorMessage($"Só pode responder no máximo a 3 questões");

                                ans1 = Console.ReadLine();
                                resposta = ans1.Split(',').Select(int.Parse).ToList();
                                resposta.Sort();
                            }

                            var valid = pergunta.Valid;

                            int respostasMult = 0;
                            for (int t = 0; t < valid.Count; t++)
                            {
                                for (int j = 0; j < resposta.Count; j++)
                                {
                                    if (valid.ElementAt(t) == resposta.ElementAt(j))
                                    {
                                        respostasMult++;
                                        certas++;
                                    }

                                }
                            }
                            result = result + (respostasMult * (20d / (double)valid.Count));
                        }

                        break;

                    case 2:

                        List<string> respostasDropDown = new List<string> { pergunta.Resposta1, pergunta.Resposta2, pergunta.Resposta3 };

                        Func.ColorMessage("Escolha a opção correta!");

                        if (getteste.PergRespRandom == 1)
                        {
                            var respCopia = new List<string>(respostasDropDown);
                            List<string> respostasRnd = new List<string>();


                            for (int k = 0; k < 3; k++)
                            {
                                Random R = new Random();
                                int randomans = 0;
                                randomans = R.Next(0, respCopia.Count);
                                string nomeResposta = respCopia.ElementAt(randomans);

                                respCopia.RemoveAt(randomans);

                                Console.WriteLine($"{k + 1} -> " + nomeResposta);
                                respostasRnd.Add(nomeResposta);
                            }

                            var ans2_ = int.Parse(Console.ReadLine());
                            Func.ExitTeste(ref ans2_);
                            while (ans2_ > 3 || ans2_ < 1)
                            {
                                Func.ErrorColorMessage("Insira uma opção válida");
                                ans2_ = int.Parse(Console.ReadLine());
                            }

                            string respostaUser = respostasRnd.ElementAt(ans2_ - 1);

                            string respostaValid = respostasDropDown.ElementAt(pergunta.Valid.ElementAt(0) - 1);

                            if (respostaUser == respostaValid)
                            {
                                certas++;
                                result = result + 20;
                            }
                        }
                        else
                        {

                            for (int k = 0; k < 3; k++)
                            {
                                Console.WriteLine($"{k + 1} -> " + respostasDropDown.ElementAt(k));
                            }

                            var ans2 = int.Parse(Console.ReadLine());
                            Func.ExitTeste(ref ans2);

                            while (ans2 > 3 || ans2 < 1)
                            {
                                Func.ErrorColorMessage("Insira uma opção válida");
                                ans2 = int.Parse(Console.ReadLine());
                            }

                            if (pergunta.Valid.ElementAt(0) == ans2)
                            {
                                certas++;
                                result = result + 20;
                            }
                        }

                        break;

                    case 3:

                        Func.ColorMessage("Escolha a opção correta!");
                        Console.WriteLine("1 -> SIM\n2 -> NÃO");
                        var ans3 = Console.ReadLine();
                        Func.ExitTesteCB(ref ans3);



                        while (ans3 != "1" && ans3 != "2" )
                        {

                            Func.ErrorColorMessage("Escreva uma opção válida");
                            ans3 = Console.ReadLine();
                            
                        }


                        if (pergunta.Resposta1 == ans3)
                        {
                            certas++;
                            result = result + 20;
                        }
                        break;
                }

            }



            if (aluno.NotaTeste == null)
            {
                aluno.NotaTeste = new List<NotaTeste>();
            }

            NotaTeste nota = new NotaTeste(aluno.NotaTeste.Count + 1, getteste.ID, getteste.Nivel, getteste.Assunto, result, DateTime.Now);
            aluno.NotaTeste.Add(nota);

            var alunosJson = Aluno.GetAlunosJson(@"Alunos\alunos.json");

            alunosJson.RemoveAll(x => x.ID == aluno.ID);

            alunosJson.Add(aluno);

            Serealize(@"Alunos\alunos.json", alunosJson);

            result = Math.Round(result, 0);

            Func.PrintResultColor($"Nota Final: {result}", result, 50);
            if (result >= 50)
            {
                Func.ColorMessage("Deseja ver se tem mais algum teste disponível?");
                int ans = Func.ReadInt("1 -> Sim\n2 -> Não");
                Func.WhileLoopInt("1 -> Sim\n2 -> Não", ref ans, 2, 1);
                if (ans == 1)
                {
                    Console.Clear();
                    AnsTeste(aluno);
                    return;
                }
                else
                {
                    Console.Clear();
                    Cons.MenuAluno(aluno);
                    return;
                }
            }


        }

        public static void AnsQuestionario(Aluno aluno)
        {
            var quest = Questionario.GetRandomQuestions();
            int certas = 0;
            double result = 0;

            //Console.WriteLine("A QUALQUER MOMENTO PODE DESISTIR DO EXAME.BASTA DIGITAR 'EXIT' ");

            /*do
            {
            */


            for (int i = 0; i < quest.Perguntas.Count; i++)
            {

                Func.ColorMessage($"Pergunta {i + 1}:");
                Console.WriteLine(quest.Perguntas.ElementAt(i).Nome);

                Console.WriteLine();

                switch (quest.Perguntas.ElementAt(i).Tipo)
                {
                    case 1:
                        Func.ColorMessage("Escolha uma ou mais opções corretas!");
                        Console.WriteLine("1 -> " + quest.Perguntas.ElementAt(i).Resposta1);
                        Console.WriteLine("2 -> " + quest.Perguntas.ElementAt(i).Resposta2);
                        Console.WriteLine("3 -> " + quest.Perguntas.ElementAt(i).Resposta3);
                        Console.WriteLine("4 -> " + quest.Perguntas.ElementAt(i).Resposta4);
                        var ans1 = Console.ReadLine();
                        Console.Clear();
                        /* if (ans1.ToLower() != "exit")
                             break;*/

                        List<int> resposta = ans1.Split(',').Select(int.Parse).ToList();
                        resposta.Sort(); // ordena o array para os casos do user colocar 2,4,3 e resposta ser 2,3,4

                        bool between14 = false;

                        while (between14 == false)
                        {
                            foreach (int x in resposta)     //para cada elemento do array da resposta ex 1,5 verifica se estao entre 1 e 4. 
                            {
                                if (x < 0 || x > 4) // se nao estiver pede de novo para escrever e guarda o novo array com a resposta
                                {
                                    Console.WriteLine("Escolha uma opção entre 1 e 4");
                                    ans1 = Console.ReadLine();
                                    resposta = ans1.Split(',').Select(int.Parse).ToList();
                                    resposta.Sort();
                                    between14 = false;
                                }
                                else { between14 = true; } // so quando todos os numeros forem validos sai do while
                            }
                        }

                        var valid = quest.Perguntas.ElementAt(i).Valid;

                        int respostasMult = 0;
                        for (int t = 0; t < valid.Count; t++)
                        {
                            for (int j = 0; j < resposta.Count; j++)
                            {
                                if (valid.ElementAt(t) == resposta.ElementAt(j))
                                {
                                    respostasMult++;
                                    certas++;
                                }

                            }
                        }
                        result = result + (respostasMult * (20d / (double)valid.Count));

                        break;
                    case 2:
                        Func.ColorMessage("Escolha a opção correta!");
                        Console.WriteLine("1 -> " + quest.Perguntas.ElementAt(i).Resposta1);
                        Console.WriteLine("2 -> " + quest.Perguntas.ElementAt(i).Resposta2);
                        Console.WriteLine("3 -> " + quest.Perguntas.ElementAt(i).Resposta3);
                        var ans2 = int.Parse(Console.ReadLine());
                        Console.Clear();


                        /*if (ans2.ToLower() != "exit")
                            break;*/

                        while (ans2 > 3 || ans2 < 1)
                        {
                            Func.ErrorColorMessage("Insira uma opção válida");
                            ans2 = int.Parse(Console.ReadLine());
                        }

                        if (quest.Perguntas.ElementAt(i).Valid.ElementAt(0) == ans2)
                        {
                            certas++;
                            result = result + 20;
                        }
                        break;
                    case 3:
                        Func.ColorMessage("Escolha a opção correta!");
                        Console.WriteLine("1 -> SIM\n2 -> NÃO");
                        var ans3 = Console.ReadLine();
                        Console.Clear();

                        while (ans3 != "1" && ans3 != "2")
                        {
                            Console.WriteLine("Escreva uma opção válida");
                            ans3 = Console.ReadLine();
                        }


                        if (quest.Perguntas.ElementAt(i).Resposta1 == ans3)
                        {
                            certas++;
                            result = result + 20;
                        }
                        break;
                }
            }

            if (aluno.NotaQuest == null)
            {
                aluno.NotaQuest = new List<NotaQuest>();
            }

            NotaQuest nota1 = new NotaQuest(aluno.NotaQuest.Count + 1, quest.Nivel, quest.Assunto, result, DateTime.Now);
            aluno.NotaQuest.Add(nota1);

            //List<Aluno> alunosJson = new List<Aluno>();
            var alunosJson = Aluno.GetAlunosJson(@"Alunos\alunos.json");

            alunosJson.RemoveAll(x => x.ID == aluno.ID);

            alunosJson.Add(aluno);

            Serealize(@"Alunos\alunos.json", alunosJson);

            result = Math.Round(result, 0);

            Func.PrintResultColor($"Nota Final: {result}", result, 80);
            Console.WriteLine();

            if (result >= 80)
            {
                Console.WriteLine("Parabéns, já está apto para fazer testes.");
                Console.WriteLine();
                Func.ColorMessage("Deseja visualizar os testes disponíveis?");
                int ans = Func.ReadInt("1 -> Sim\n2 -> Não");
                Func.WhileLoopInt("1 -> Sim\n2 -> Não", ref ans, 2, 1);
                if (ans == 1)
                {
                    Console.Clear();
                    AnsTeste(aluno);
                    return;
                }
                else
                {
                    Console.Clear();
                    Cons.MenuAluno(aluno);
                    return;
                }

            }
            else
            {
                Console.WriteLine("Para ficar apto para fazer testes, necessita de tirar pelo menos 80.");
                Console.WriteLine();
                Func.ColorMessage("Deseja fazer outro questionário?");
                int ans = Func.ReadInt("1 -> Sim\n2 -> Não");
                Func.WhileLoopInt("1 -> Sim\n2 -> Não", ref ans, 2, 1);
                if (ans == 1)
                {
                    AnsQuestionario(aluno);
                    return;
                }
                else
                {
                    Cons.MenuAluno(aluno);
                    return;
                }
            }
        }

        public static void VerNotasQuest(Aluno aluno)
        {
            string assunto = "", nivel = "";

            Func.ColorMessage("Notas de Questionários : ");
            if (aluno.NotaQuest == null)
            {
                Func.ErrorColorMessage("Ainda não tem qualquer nota em sistema");
                Cons.MenuAluno(aluno);
                return;
            }

            foreach (var item in aluno.NotaQuest)
            {
                var desc = Func.getdescnivelassunto(item.Assunto, item.Nivel);

                Console.WriteLine($"Data -> {item.DataRealizacao.ToShortDateString()} | Assunto -> {item.Assunto}: {desc.assunto}  |  tNível -> {item.Nivel}: {desc.nivel}  |  Nota -> {item.Resultado}");
            }
            Console.WriteLine();
            Func.ColorMessage("Deseja ver as notas dos testes?");
            var ans2 = Func.ReadInt("1 -> Sim\n2 -> Não");
            Func.WhileLoopInt("1 -> Sim\n2 -> Não", ref ans2, 2, 1);
            if (ans2 == 1)
            {
                Console.Clear();
                Console.WriteLine();
                Aluno.VerNotasTeste(aluno);
            }
            else
            {
                Console.Clear();
                Cons.MenuAluno(aluno);
                return;
            }
        }
        public static void VerNotasTeste(Aluno aluno)
        {
            string assunto = "", nivel = "";

            Func.ColorMessage("Notas de Testes : ");
            if (aluno.NotaTeste == null)
            {
                Func.ErrorColorMessage("Ainda não tem qualquer nota em sistema");
                Console.WriteLine();
                Cons.MenuAluno(aluno);
                return;
            }

            foreach (var item in aluno.NotaTeste)
            {
                var desc = Func.getdescnivelassunto(item.Assunto, item.Nivel);

                Console.WriteLine($"Data -> {item.DataRealizacao.ToShortDateString()} | Assunto -> {item.Assunto}: {desc.assunto}  |  tNível -> {item.Nivel}: {desc.nivel}  |  Nota -> {item.Resultado}");
            }
            Console.WriteLine();
            Func.ColorMessage("Deseja ver as notas dos questionários?");
            var ans2 = Func.ReadInt("1 -> Sim\n2 -> Não");
            Func.WhileLoopInt("1 -> Sim\n2 -> Não", ref ans2, 2, 1);
            if (ans2 == 1)
            {
                Console.Clear();
                Aluno.VerNotasQuest(aluno);
            }
            else
            {
                Console.Clear();
                Cons.MenuAluno(aluno);
                return;
            }
        }

    }


}

