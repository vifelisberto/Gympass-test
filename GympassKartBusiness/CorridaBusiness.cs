using GympassKartModel;
using GympassKartRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GympassKartBusiness
{
    public class CorridaBusiness
    {
        private readonly CorridaRepository _corridaRep = new CorridaRepository();
        private const string REGEX_BUSCA_VOLTA = @"^(\d{2}:\d{2}:\d{2}.\d{3})|(\d{3} – [A-Z|.]*)|(\d{1})[\t| ]|[\t| ](\d{1,2}:\d{2}.\d{3})|(\d{2,3}\,\d{2,3})";

        /// <summary>
        /// Método que realiza a leitura de um log de corrida, para assim escrever o resultado.
        /// </summary>
        public List<Volta> ResultadoCorrida(out Volta melhor)
        {
            List<Volta> resultado = new List<Volta>();
            melhor = null;

            // Separa as linhas pela quebra de linha.
            string[] logs = _corridaRep.RetornaLogCorrida().Split("\n");

            // Adiciona as voltas na lista de resultado. O(n)
            foreach (string log in logs.Skip(1))
            {
                Volta volta = ConverteVolta(log);

                // Verifica se já existe alguma volta do piloto na lista.
                int indexExistente = resultado.FindIndex(_ => _.NumeroPiloto == volta.NumeroPiloto);
                if (indexExistente != -1)
                {
                    // Se existir, altera a volta que ele está e soma o tempo, velocidade e melhor volta.
                    Volta existente = resultado[indexExistente];

                    if (volta.TempoVolta <= existente.TempoVolta)
                    {
                        existente.MelhorVolta = volta.NumVolta;
                    }

                    existente.NumVolta = volta.NumVolta;
                    existente.HoraVolta = volta.HoraVolta;
                    existente.VelocidadeMedia += volta.VelocidadeMedia;
                    existente.TempoTotal += volta.TempoVolta;
                    existente.TempoVolta = volta.TempoVolta;
                }
                else
                {
                    // Se não existir, adiciona o piloto na lista.
                    resultado.Add(volta);
                }

                if (melhor == null)
                {
                    melhor = volta;
                }

                // Compara para descobrir a melhor volta da corrida.
                melhor = volta.TempoVolta <= melhor?.TempoVolta ? volta : melhor;
            }

            // Ordena as voltas pelo número da volta e pelo Horário que foi registrado a volta.
            // Garante a ordem de chegada, caso algum piloto por exemplo, esteja na segunda volta e passe perto do tempo de quem esteja fazendo a terceira.
            OrdenaVoltas(resultado);

            return resultado;
        }

        /// <summary>
        /// Método que converte uma string em uma instância de Volta.
        /// </summary>
        /// <param name="textoVolta">String com os dados que serão convertidos.</param>
        /// <returns>Dados Convertidos.</returns>
        private static Volta ConverteVolta(string textoVolta)
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
                    TempoTotal = tempoVolta,
                    VelocidadeMedia = velocidadeVolta,
                    MelhorVolta = numVolta
                };
            }

            return voltaConvertida;
        }

        /// <summary>
        /// Ordenação das voltas utilizando o conceito do algoritmo Insertion Sort. O(n²).
        /// </summary>
        /// <param name="vetor">Voltas que serão ordenadas.</param>
        private static void OrdenaVoltas(List<Volta> voltas)
        {
            int i;
            int j;
            Volta atual;

            // Percore todas as posições.
            for (i = 1; i < voltas.Count; i++)
            {
                // Pega a posição atual de volta.
                atual = voltas[i];
                j = i - 1;

                // Compara se a hora da volta alterior e maior que a atual e se a volta é menor ou igual a atual.
                while (j >= 0 && voltas[j].HoraVolta > atual.HoraVolta && voltas[j].NumVolta <= atual.NumVolta)
                {
                    // Troca as posições que tiverem os valores menores, ordenando da esquerda para a direita.
                    voltas[j + 1] = voltas[j];
                    j--;
                }
                voltas[j + 1] = atual;
            }
        }
    }
}
