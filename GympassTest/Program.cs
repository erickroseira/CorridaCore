using System;
using System.Collections.Generic;
using System.IO;
using gympass.Models;
using gympass.Services;
using gympass.Utilities;

namespace gympass
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                    throw new Exception("É necessário informar caminho do arquivo de entrada (log da corrida) e o caminho onde salvar o relatório final.");

                var corridaLogFile = args[0];
                var relatorioFinalPath = args[1];

                var listaVoltasCorrida = new List<IVoltaCorrida>();

                foreach (string voltaInf in File.ReadLines(corridaLogFile))
                {
                    if (voltaInf.Contains("Hora"))
                        continue;
                    listaVoltasCorrida.Add(LogReader.VoltaCorridaMapper(voltaInf));
                }

                // instancia classe responsavel por calcular resultado da corrida
                CorridaService resultado = new CorridaService();

                // inicia o processo de cálculo do resultado final da corrida
                string relatorio = resultado.CalcularResultadoCorrida(listaVoltasCorrida);

                File.WriteAllText(relatorioFinalPath, relatorio);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
