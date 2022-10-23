using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QUEST
{
    public class Cons
    {
        public static void Run()
        {

            Func.ColorMessage("Escolha uma das opções :");
            int user = Func.ReadInt("1 - Professor\n2 - Aluno");
            Console.Clear();
            while (user > 2 || user < 1)
            {
                Console.WriteLine("Digite uma opção válida por favor");
                Console.WriteLine();
                user = Func.ReadInt("1 - Professor\n2 - Aluno");
                Console.Clear();
            }
            if (user == 1)
            {
                Func.PrintString("Insira o seu ID");
                int idAProfessor = int.Parse(Console.ReadLine());
                Console.Clear();

                MenuProf();
            }
            if (user == 2)
            {
                Func.PrintString("Insira o seu ID");
                int idAluno = int.Parse(Console.ReadLine());
                Console.Clear();

                var aluno = Aluno.GetAlunoById(@"Alunos\alunos.json", idAluno);

                while (aluno == null)
                {
                    Func.ErrorColorMessage("Esse ID não existe. Insira de novo");
                    idAluno = int.Parse(Console.ReadLine());
                    aluno = Aluno.GetAlunoById(@"Alunos\alunos.json", idAluno);
                    Console.Clear();
                }

                MenuAluno(aluno);
            }
        }


        public static void MenuAluno(Aluno aluno)
        {

            Console.WriteLine($"Bem vindo {aluno.Name}!");
            Console.WriteLine();
            Func.ColorMessage("Escolha uma das opções :");
            int opcao = Func.ReadInt("1 -> Ver notas\n2 -> Fazer questionário\n3 -> Fazer teste");

            do
            {

                if (opcao > 3 || opcao < 1)
                {
                    Func.ErrorColorMessage("Escreva uma opção válida");
                    opcao = Func.ReadInt("1 -> Ver notas\n2 -> Fazer questionário\n3 -> Fazer teste");
                }

                switch (opcao)
                {
                    case 1:
                        Console.Clear();
                        Func.ColorMessage("Escolha uma das opções");
                        var ans = Func.ReadInt("1 -> Questionários\n2 -> Testes");
                        Func.WhileLoopInt("1 -> Questionários\n2 -> Testes", ref ans, 2, 1);
                        if (ans == 1)
                        {
                            Console.Clear();
                            Aluno.VerNotasQuest(aluno);

                        }
                        else
                        {
                            Console.Clear();
                            Aluno.VerNotasTeste(aluno);

                        }
                        break;
                    case 2:
                        Console.Clear();
                        Aluno.AnsQuestionario(aluno);
                        break;
                    case 3:
                        Console.Clear();
                        var testesDisponiveis = Teste.GetTestesJson(@"Testes\testes.json");
                        if (testesDisponiveis.Any(r => r.Alunos.Any(s => s == aluno.ID)))
                        {
                            Aluno.AnsTeste(aluno);
                        }
                        else
                        {
                            Func.ErrorColorMessage("Não obteve nenhuma nota superior a 80%, logo não tem qualquer teste disponível.");
                            int ans2 = Func.ReadInt("Deseja responder a questionários?\n1 - Sim \n2 - Não");
                            Func.WhileLoopInt("Deseja responder a questionários?\n1 - Sim \n2 - Não", ref ans2, 2, 1);
                            if (ans2 == 1)
                                Aluno.AnsQuestionario(aluno);
                            else
                            {
                                Console.Clear();
                                MenuAluno(aluno);
                            }
                        }
                        break;
                }

            } while (opcao > 3 || opcao < 1);
        }

        public static void MenuProf()
        {
            Console.WriteLine($"Bem vindo professor!");
            Console.WriteLine();
            Func.ColorMessage("Escolha uma das opções :");
            int opcao = Func.ReadInt("1 - Visualizar testes e notas\n2 - Criar questão\n3 - Criar Teste");

            do
            {

                if (opcao > 3 || opcao < 1)
                {
                    Func.ErrorColorMessage("Escreva uma opção válida");
                    opcao = Func.ReadInt("1 -> Ver notas\n2 -> Fazer questionário\n3 -> Fazer teste");
                }

                switch (opcao)
                {
                    case 1:
                        Console.Clear();
                        Teste.VerTestes();
                        break;
                    case 2:
                        Console.Clear();
                        Questions.NewQuestion();
                        break;
                    case 3:
                        Console.Clear();
                        Func.ColorMessage("Como prefere criar o teste?");
                        int teste = Func.ReadInt("1 - Questões aleatorias \n2 - Escolher as questões");
                        Func.WhileLoopInt("1 - Questões aleatorias \n2 - Escolher as questões", ref teste, 2, 1);
                        if (teste == 1)
                        {
                            Console.Clear();
                            Teste.CreateTesteRandom();
                        }
                        else
                        {
                            Console.Clear();
                            Teste.CreateTesteManual();
                        }
                        break;
                    default:
                        Console.WriteLine("Digite uma opção válida");
                        break;
                }

            } while (opcao > 3 || opcao < 1);
        }

    }


}








