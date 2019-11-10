using System;
using System.Text;

namespace GympassKartModel
{
    /// <summary>
    /// Classe que representa a volta de uma corrida.
    /// </summary>
    public class Volta
    {
        public TimeSpan HoraVolta { get; set; }
        public int NumeroPiloto { get; set; }
        public string NomePiloto { get; set; }
        public int NumVolta { get; set; }
        public TimeSpan TempoTotal { get; set; }
        public TimeSpan TempoVolta { get; set; }
        public float VelocidadeMedia { get; set; }
        public int MelhorVolta { get; set; }

        /// <summary>
        /// Método que retorna os dados da volta, comprando com o resultado da primeira volta.
        /// </summary>
        /// <param name="i">Posição de chegada.</param>
        /// <param name="tempoPrimeiraVolta">Tempo da primeira volta da corrida.</param>
        /// <returns>Retorna uma string com os dados da volta.</returns>
        public string ToString(int i, TimeSpan tempoPrimeiraVolta)
        {
            StringBuilder dadosVolta = new StringBuilder();
            dadosVolta.AppendLine();

            dadosVolta.Append($"Posição de chegada: {i}º \n\n");
            dadosVolta.Append($"\tCód. Piloto: {this.NumeroPiloto}\n");
            dadosVolta.Append($"\tNome Piloto: {this.NomePiloto}\n");
            dadosVolta.Append($"\tQtde Voltas: {this.NumVolta}\n");
            dadosVolta.Append($"\tTempo Total: {this.TempoTotal}\n");
            dadosVolta.Append($"\tVelocidade Media: {this.VelocidadeMedia / this.NumVolta} km/h\n");
            dadosVolta.Append($"\tMelhor Volta: {this.MelhorVolta}");

            if (i != 1)
            {
                dadosVolta.Append($"\n\tDiferença entre o primeiro colocado: {(this.HoraVolta - tempoPrimeiraVolta).TotalSeconds} segundos\n");
            }

            dadosVolta.AppendLine();

            return dadosVolta.ToString();
        }

        /// <summary>
        /// Método que escreve os dados da volta.
        /// </summary>
        /// <returns>Retorna uma string com os dados da volta.</returns>
        public override string ToString()
        {
            StringBuilder dadosVolta = new StringBuilder();
            dadosVolta.Append($"\tCód. Piloto: {this.NumeroPiloto}\n");
            dadosVolta.Append($"\tNome Piloto: {this.NomePiloto}\n");
            dadosVolta.Append($"\tTempo: {this.TempoVolta}\n");
            dadosVolta.Append($"\tVolta: {this.NumVolta}\n");
            dadosVolta.AppendLine();

            return dadosVolta.ToString();
        }
    }
}
