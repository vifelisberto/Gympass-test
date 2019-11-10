using GympassKartBusiness;
using GympassKartModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GympassKart
{
    public class Program
    {
        private static readonly CorridaBusiness _bus = new CorridaBusiness();

        public static void Main(string[] args)
        {
            string textoDivisor = "--------------------------------------------------";

            try
            {
                // Inicia a execução para mostrar o Resultado da corrida.
                Stopwatch tempo = new Stopwatch();
                tempo.Start();

                List<Volta> resultado = _bus.ResultadoCorrida(out Volta melhorVolta);

                tempo.Stop();
                Console.WriteLine($"Tempo de execução para montar resultado: {tempo.Elapsed} \n\n");

                TimeSpan tempoPrimeiraVolta = resultado.FirstOrDefault().HoraVolta;

                Console.WriteLine("Resultado da corrida: ");
                Console.WriteLine(textoDivisor);
                // Exibe o resultado da corrida no console.
                for (int i = 1; i <= resultado.Count; i++)
                {
                    Volta volta = resultado[i - 1];

                    // Exibe o resultado no console.
                    Console.WriteLine(volta.ToString(i, tempoPrimeiraVolta));
                    Console.WriteLine(textoDivisor);
                }

                Console.WriteLine($"\nMelhor volta da corrida: ");
                Console.WriteLine(melhorVolta?.ToString());

                Console.WriteLine("Precione enter para fechar.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao processar o resultado da corrida.");
                Console.WriteLine($"Log: {e.Message}");
            }
        }
    }
}
