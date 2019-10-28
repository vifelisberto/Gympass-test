using System;
using System.IO;
using System.Linq;

namespace Gympass
{
    public class Corrida
    {
        private readonly string NOME_ARQUIVO_RESULTADO = @"CorridaKart.txt";
        public void ResultadoCorrida()
        {
            string[] logs = File.ReadAllLines($"/{NOME_ARQUIVO_RESULTADO}");



        }
    }
}
