using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GympassKart
{
    /// <summary>
    /// Classe com métodos para realizar um resultado de uma corrida.
    /// </summary>
    public class Corrida
    {
        private readonly string NOME_ARQUIVO_RESULTADO = @"CorridaKart.txt";
        private readonly string REGEX_BUSCA_VOLTA = @"^(\d{2}:\d{2}:\d{2}.\d{3})|(\d{3} – [A-Z|.]*)|(\d{1})[\t| ]|[\t| ](\d{1,2}:\d{2}.\d{3})|(\d{2,3}\,\d{2,3})";
        private readonly int NUM_ULTIMA_VOLTA = 4;

        /// <summary>
        /// Método que realiza a leitura de um log de corrida e retorna o resultado da mesma.
        /// </summary>
        public void ResultadoCorrida()
        {
            // Busca o arquivo.
            string diretorio = Directory.GetFiles($@"{Directory.GetCurrentDirectory()}\..\..\..\..").FirstOrDefault(_ => _.Contains(NOME_ARQUIVO_RESULTADO));

            // Realiza a leitura do arquivo.
            string[] logs = File.ReadAllLines(diretorio);

            List<Volta> voltas = logs.Skip(1).Select(_ => ConverteVolta(_)).ToList();
            List<Volta> ultimaVolta = voltas.Where(_ => _.NumVolta == NUM_ULTIMA_VOLTA).ToList();

            // O(log n) Utiliando o Merge Sort
            ultimaVolta = MergeSort(ultimaVolta);

            // Exibe o resultado da ultima volta.
            int i = 1;
            for (i = 1; i <= ultimaVolta.Count; i++)
            {
                Volta volta = ultimaVolta[i - 1];
                List<Volta> voltasPiloto = voltas.Where(_ => _.NumeroPiloto == volta.NumeroPiloto).ToList();
                TimeSpan tempoTotal = new TimeSpan(TempoTotal(voltasPiloto.Select(_ => _.TempoVolta)));

                Console.WriteLine($"Posição de chegada: {i}º");
                Console.WriteLine($"Cód. Piloto: {volta.NumeroPiloto}");
                Console.WriteLine($"Nome Piloto: {volta.NomePiloto}");
                Console.WriteLine($"Qtde Voltas: {voltasPiloto.Count()}");
                Console.WriteLine($"Tempo Total: {tempoTotal}");
                Console.WriteLine("");
            }

            // Exibe o resultado de corredors que não completaram a ultima volta.
            // TODO: Arrumar a busca.
            List<Volta> naoFinalizados = voltas.Where(_ => !ultimaVolta.Any(x => x.NumeroPiloto == _.NumeroPiloto)).ToList();
            foreach(Volta volta in naoFinalizados)
            {
                List<Volta> voltasPiloto = voltas.Where(_ => _.NumeroPiloto == volta.NumeroPiloto).ToList();
                TimeSpan tempoTotal = new TimeSpan(TempoTotal(voltasPiloto.Select(_ => _.TempoVolta)));

                Console.WriteLine($"Posição de chegada: {i++}º");
                Console.WriteLine($"Cód. Piloto: {volta.NumeroPiloto}");
                Console.WriteLine($"Nome Piloto: {volta.NomePiloto}");
                Console.WriteLine($"Qtde Voltas: {voltasPiloto.Count()}");
                Console.WriteLine($"Tempo Total: {tempoTotal}");
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// Método que converte uma string em uma instância de Volta.
        /// </summary>
        /// <param name="textoVolta">String com os dados que serão convertidos.</param>
        /// <returns>Dados Convertidos.</returns>
        private Volta ConverteVolta(string textoVolta)
        {
            Volta voltaConvertida = null;

            // Extrai os dados via regex.
            MatchCollection dadosVolta = Regex.Matches(textoVolta, REGEX_BUSCA_VOLTA, RegexOptions.Compiled);

            // Verifica se encontrou todos os campos.
            if (dadosVolta.Count == 5)
            {
                // Realiza a conversão dos dados de hora/tempo/velocidade/piloto.
                TimeSpan.TryParse(dadosVolta[0].Value, out TimeSpan horaVolta);
                int.TryParse(dadosVolta[1].Value.Substring(0, 3), out int numPiloto);
                // Retira o número do piloto e pega somente o nome.
                string nomePiloto = dadosVolta[1].Value.Substring(6, dadosVolta[1].Value.Length - 6).Trim();
                int.TryParse(dadosVolta[2].Value, out int numVolta);
                string tempoVoltaS = dadosVolta[3].Value.Trim();
                // Adidiona 00:0 na frente para ser possível converte em TimeSpan.
                TimeSpan.TryParse(tempoVoltaS.Length == 8 ? $"00:0{tempoVoltaS}" : tempoVoltaS, out TimeSpan tempoVolta);
                float.TryParse(dadosVolta[4]?.Value, out float velocidadeVolta);

                voltaConvertida = new Volta()
                {
                    HoraVolta = horaVolta,
                    NumeroPiloto = numPiloto,
                    NomePiloto = nomePiloto,
                    NumVolta = numVolta,
                    TempoVolta = tempoVolta,
                    VelocidadeMediaVolta = velocidadeVolta
                };
            }

            return voltaConvertida;
        }


        private long TempoTotal(IEnumerable<TimeSpan> tempo)
        {
            return tempo.Sum(_ => _.Ticks);
        }

        private static List<Volta> MergeSort(List<Volta> unsorted)
        {
            if (unsorted.Count <= 1)
                return unsorted;

            List<Volta> left = new List<Volta>();
            List<Volta> right = new List<Volta>();

            int middle = unsorted.Count / 2;
            for (int i = 0; i < middle; i++)  //Dividing the unsorted list
            {
                left.Add(unsorted[i]);
            }
            for (int i = middle; i < unsorted.Count; i++)
            {
                right.Add(unsorted[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);
            return Merge(left, right);
        }

        private static List<Volta> Merge(List<Volta> left, List<Volta> right)
        {
            List<Volta> result = new List<Volta>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {
                    if (left.First()?.HoraVolta <= right.First()?.HoraVolta)  //Comparing First two elements to see which is smaller
                    {
                        result.Add(left.First());
                        left.Remove(left.First());      //Rest of the list minus the first element
                    }
                    else
                    {
                        result.Add(right.First());
                        right.Remove(right.First());
                    }
                }
                else if (left.Count > 0)
                {
                    result.Add(left.First());
                    left.Remove(left.First());
                }
                else if (right.Count > 0)
                {
                    result.Add(right.First());

                    right.Remove(right.First());
                }
            }
            return result;
        }
    }
}
