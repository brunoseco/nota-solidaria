using Common.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSolidaria.Gen
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Deseja Atualizar/Criar todos os Contextos (s/n)");
            var accept = Console.ReadLine();

            Console.WriteLine("Gerar direto na pasta dos projetos (s/n)");
            var usePathProjects = Console.ReadLine();

            if (accept == "s")
                MainWithOutConfirmation(args, usePathProjects == "s");
            else
                MainWithConfirmation(args, usePathProjects == "s");
        }


        static void MainWithConfirmation(string[] args, bool UsePathProjects)
        {

            var context = new ConfigContext().GetConfigContext();
            var helper = new HelperSysObjects(context);

            foreach (var item in helper.Contexts)
            {
                Console.WriteLine("Deseja Atualizar/Criar o Contexto {0} (s/n)", item.Namespace);
                var accept = Console.ReadLine();
                if (accept == "s")
                {
                    Console.WriteLine("Deseja Escolher uma classe,Digite o nome dela?");
                    var className = Console.ReadLine();
                    if (!string.IsNullOrEmpty(className))
                        helper.MakeClass(item, className, UsePathProjects);
                    else
                        helper.MakeClass(item);
                }

            }

        }

        static void MainWithOutConfirmation(string[] args,bool UsePathProjects)
        {

            var context = new ConfigContext().GetConfigContext();
            var helper = new HelperSysObjects(context);

            foreach (var item in helper.Contexts)
            {
                Console.WriteLine(item.Namespace);
                helper.MakeClass(item,UsePathProjects);
            }
        }
    }
}
