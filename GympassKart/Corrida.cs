using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GympassKart
{
    /// <summary>
    /// Classe com métodos para realizar um resultado de uma corrida.
    /// </summary>
    public class Corrida
    {
        #region CONSTANTES
        private const string REGEX_BUSCA_VOLTA = @"^(\d{2}:\d{2}:\d{2}.\d{3})|(\d{3} – [A-Z|.]*)|(\d{1})[\t| ]|[\t| ](\d{1,2}:\d{2}.\d{3})|(\d{2,3}\,\d{2,3})";
        private const string LOG_CORRIDA = @"Hora                               Piloto             Nº Volta   Tempo Volta       Velocidade média da volta
23:49:08.277      038 – F.MASSA                           1		1:02.852                        44,275
23:49:10.858      033 – R.BARRICHELLO                     1		1:04.352                        43,243
23:49:11.075      002 – K.RAIKKONEN                       1             1:04.108                        43,408
23:49:12.667      023 – M.WEBBER                          1		1:04.414                        43,202
23:49:30.976      015 – F.ALONSO                          1		1:18.456			35,47
23:50:11.447      038 – F.MASSA                           2		1:03.170                        44,053
23:50:14.860      033 – R.BARRICHELLO                     2		1:04.002                        43,48
23:50:15.057      002 – K.RAIKKONEN                       2             1:03.982                        43,493
23:50:17.472      023 – M.WEBBER                          2		1:04.805                        42,941
23:50:37.987      015 – F.ALONSO                          2		1:07.011			41,528
23:51:14.216      038 – F.MASSA                           3		1:02.769                        44,334
23:51:18.576      033 – R.BARRICHELLO		          3		1:03.716                        43,675
23:51:19.044      002 – K.RAIKKONEN                       3		1:03.987                        43,49
23:51:21.759      023 – M.WEBBER                          3		1:04.287                        43,287
23:51:46.691      015 – F.ALONSO                          3		1:08.704			40,504
23:52:01.796      011 – S.VETTEL                          1		3:31.315			13,169
23:52:17.003      038 – F.MASS                            4		1:02.787                        44,321
23:52:22.586      033 – R.BARRICHELLO		          4		1:04.010                        43,474
23:52:22.120      002 – K.RAIKKONEN                       4		1:03.076                        44,118
23:52:25.975      023 – M.WEBBER                          4		1:04.216                        43,335
23:53:06.741      015 – F.ALONSO                          4		1:20.050			34,763
23:53:39.660      011 – S.VETTEL                          2		1:37.864			28,435
23:54:57.757      011 – S.VETTEL                          3		1:18.097			35,633";
        #endregion

        /// <summary>
        /// Método que realiza a leitura de um log de corrida, para assim escrever o resultado.
        /// </summary>
        public static List<Volta> ResultadoCorrida(out Volta melhor)
        {
            List<Volta> resultado = new List<Volta>();
            melhor = null;
            // Separa as linhas pela quebra de linha.
            string[] logs = LOG_CORRIDA.Split("\n");

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
                    existente.NumVolta = volta.NumVolta;
                    existente.HoraVolta = volta.HoraVolta;
                    existente.VelocidadeMedia += volta.VelocidadeMedia;
                    existente.Tempo += volta.Tempo;
                    existente.MelhorVolta = existente.Tempo >= volta.Tempo ? volta.NumVolta : existente.NumVolta;
                }
                else
                {
                    // Se não existir, adiciona o piloto na lista.
                    resultado.Add(volta);
                }

                // Compara para descobrir a melhor volta da corrida.
                melhor = melhor?.Tempo == null || volta.Tempo >= melhor?.Tempo ? volta : melhor;
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
                    Tempo = tempoVolta,
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
