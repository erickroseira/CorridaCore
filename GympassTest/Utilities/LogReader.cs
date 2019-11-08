using System;
using System.Text.RegularExpressions;
using gympass.Models;

namespace gympass.Utilities
{
    public class LogReader
    {

        public static IVoltaCorrida VoltaCorridaMapper(string voltaCorrida)
        {

            var elementos_volta = Regex.Split(voltaCorrida, "\\s{2,}");

            //verifica se numero de colunas é igual a 5. Caso contrário termina programa
            if (elementos_volta.Length != 5)
                throw new Exception("Arquivo de entrada com formato diferente do especificado. Lembre-se separe colunas por no mínimo 2 espaços e a informação" +
                        " de cada coluna como a identificação completa do pilto (numero - nome) por exemplo, deve estar separada por apenas 1 espaço.");


            var horaVolta = TimeSpan.Parse(elementos_volta[0]);

            var piloto = new Piloto(Convert.ToInt32(elementos_volta[1].Split(" – ")[0]), elementos_volta[1].Split(" – ")[1]);

            var numeroVolta = Convert.ToInt32(elementos_volta[2]);

            Func<string, string> FormataTimeString = (pTimeStr) =>
            {
                var HMSTempoVolta = elementos_volta[3].Split(":");

                if (HMSTempoVolta.Length > 3)
                {
                    if (HMSTempoVolta[0].Length == 1)
                        return "0" + pTimeStr;
                    else
                        return pTimeStr;
                }
                else
                {
                    if (HMSTempoVolta[0].Length == 1)
                        return "00:0" + pTimeStr;
                    else
                        return "00:" + pTimeStr;
                }
            };

            //obtem Tempo Volta. em horas, minutos, segundos

            var tempoVolta = TimeSpan.Parse(FormataTimeString(elementos_volta[3]));

            //obtem Velocidade média da volta. OBS: (linha abaixo verifica se velocidade está separada por virgula. Se sim, substitui por ponto (.)
            String velocidade = (elementos_volta[4].Contains(",")) ? elementos_volta[4].Replace(",", ".") : elementos_volta[4];
            var velMedia = float.Parse(velocidade);

            return new VoltaCorrida(horaVolta, piloto, numeroVolta, tempoVolta, velMedia);
        }
    }
}