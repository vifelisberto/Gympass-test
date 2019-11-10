using System;
using System.Collections.Generic;
using System.Linq;

namespace GympassKart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Inicia a execução para mostrar o Resultado da corrida.
                List<Volta> resultado = Corrida.ResultadoCorrida(out Volta melhorVolta);

                TimeSpan tempoPrimeiraVolta = resultado.FirstOrDefault().HoraVolta;

                // Exibe o resultado da corrida no console.
                for (int i = 1; i <= resultado.Count; i++)
                {
                    Volta volta = resultado[i - 1];

                    // Exibe o resultado no console.
                    Console.WriteLine(volta.ToString(i, tempoPrimeiraVolta));
                }

                Console.WriteLine($"Melhor volta da corrida: ");
                Console.WriteLine(melhorVolta.ToString());

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao processar o resultado da corrida.");
                Console.WriteLine($"Log: {e.Message}");
            }
        }
    }
}
