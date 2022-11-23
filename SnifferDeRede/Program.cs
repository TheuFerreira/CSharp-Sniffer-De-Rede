using System;
using System.Threading;

namespace SnifferDeRede
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        public static void Main()
        {

            Application app = new Application();

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Carregando o trojan");
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(100);
                    DrawTextProgressBar(i, 100);
                }

                app.Start();
                while (true)
                {
                    Console.Write("Seus dados são nossos agora ");
                    Console.Write("Obrigado pelas senhas ");
                    Console.Write("Sandrinha e Paulo juntos ");
                    Console.Write("Spy Spy Spy ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MJsnifferForm());
        }

        private static void DrawTextProgressBar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
        }
    }
}
