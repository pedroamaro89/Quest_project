using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QUEST
{
    public class Func
    {


        public static void ExitTeste(ref int ans)
        {
            if (ans == 0)
            {
                Console.WriteLine();
                Func.ErrorColorMessage("Deseja sair do teste sem o concluir?");
                int ans2 = Func.ReadInt("1 -> Sim\n2 -> Não");
                WhileLoopInt("1 -> Sim\n2 -> Não", ref ans2, 2, 1);
                if (ans2 == 1)
                {
                    Console.Clear();
                    Cons.Run();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Nesse caso responda de novo à questão");
                    ans = int.Parse(Console.ReadLine());
                }
            }

        }
        public static void ExitTesteCB(ref string ans)
        {
            if (ans == "0")
            {
                Console.WriteLine();
                Func.ErrorColorMessage("Deseja sair do teste sem o concluir?");
                int ans2 = Func.ReadInt("1 -> Sim\n2 -> Não");
                WhileLoopInt("1 -> Sim\n2 -> Não", ref ans2, 2, 1);

                if (ans2 == 1)
                {
                    Console.Clear();
                    Cons.Run();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Nesse caso responda de novo à questão");
                    ans = Console.ReadLine();
                }
            }

        }



        public static void ColorMessage(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mensagem);
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void ErrorColorMessage(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensagem);
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void PrintResultColor(string message, double result, int positiva)
        {
            if (result >= positiva)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public static void PrintString(string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mensagem);
            Console.ResetColor();
        }



        public static int ReadInt(string mensagem)
        {
            Console.WriteLine(mensagem);

            string str = Console.ReadLine();

            int result;
            var isOk = int.TryParse(str, out result);

            return result;
        }


        public static string ReadString(string mensagem)
        {
            Console.WriteLine(mensagem);
            string ans = Console.ReadLine();
            return ans;
        }

        public static void WhileLoopInt(string mensagem, ref int cond, int greaterthan, int lowerthan)
        {
            while (cond > greaterthan || cond < lowerthan)
            {
                Func.ErrorColorMessage("Digite uma opção válida por favor");
                Console.WriteLine();
                cond = Func.ReadInt(mensagem);
                Console.Clear();
            }
        }

        public static (string assunto, string nivel) getdescnivelassunto(int idAssunto, int idNivel)
        {

            string assunto = "", nivel = "";
            switch (idAssunto)
            {
                case 1:
                    assunto = "Cinema";
                    break;
                case 2:
                    assunto = "Música";
                    break;
                case 3:
                    assunto = "Cultura Geral";
                    break;

                default:
                    break;
            }
            switch (idNivel)
            {
                case 1:
                    nivel = "Básico";
                    break;
                case 2:
                    nivel = "Intermédio";
                    break;
                case 3:
                    nivel = "Avançado";
                    break;

                default:
                    break;
            }

            return (assunto, nivel);
        }




    }
}
