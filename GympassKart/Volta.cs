using System;
using System.Text;

namespace GympassKart
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
        public TimeSpan Tempo { get; set; }
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
            dadosVolta.Append($"Posição de chegada: {i}º \n");
            dadosVolta.Append($"Cód. Piloto: {this.NumeroPiloto}\n");
            dadosVolta.Append($"Nome Piloto: {this.NomePiloto}\n");
            dadosVolta.Append($"Qtde Voltas: {this.NumVolta}\n");
            dadosVolta.Append($"Tempo Total: {this.Tempo}\n");
            dadosVolta.Append($"Velocidade Media: {this.VelocidadeMedia / this.NumVolta}\n");
            dadosVolta.Append($"Melhor Volta: {this.MelhorVolta}");

            if (i != 1)
            {
                dadosVolta.Append($"\nDiferença entre o primeiro colocado: {(this.HoraVolta - tempoPrimeiraVolta).TotalSeconds}\n");
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
            dadosVolta.Append($"Cód. Piloto: {this.NumeroPiloto}\n");
            dadosVolta.Append($"Nome Piloto: {this.NomePiloto}\n");
            dadosVolta.Append($"Qtde Voltas: {this.NumVolta}\n");
            dadosVolta.Append($"Tempo Total: {this.Tempo}\n");
            dadosVolta.Append($"Velocidade Media: {this.VelocidadeMedia / this.NumVolta}\n");
            dadosVolta.AppendLine();

            return dadosVolta.ToString();
        }
    }
}
