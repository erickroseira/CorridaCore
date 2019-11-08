using System.Collections.Generic;
using System.Text;
using gympass.Models;
using gympass.Utilities;
using System.Linq;
using System;
using System.Globalization;
using gympass.Models.Interfaces;

namespace gympass.Services
{
    public class CorridaService : ICorridaService
    {

        //String builder que conterá relatorio final
        public StringBuilder RelatorioFinal { get; set; } = new StringBuilder();

        public CultureInfo Culture { get; set; } = CultureInfo.CreateSpecificCulture("pt-BR");

        /*
         * Função que computa posição final (classficação final) dos pilotos
          */
        public Dictionary<int, DadosCorridaPiloto> ComputarPosicaoFinalDosPilotos(Dictionary<int, DadosCorridaPiloto> pPrePosicaoPilotos)
        {

            var dResultado = new Dictionary<int, DadosCorridaPiloto>();

            //ordena a lista primeiramente pela numero de voltas de cada piloto.
            // Havendo pilotos terminando a corrida com o mesmo numero de voltas verifica quem chegou primeiro
            var posicao = 0;
            pPrePosicaoPilotos.ToList().OrderByDescending(x => x.Value.NumeroDeVoltasTotais)
                                .ThenBy(x => x.Value.HoraUltimaVolta).ToList()
                                .ForEach(x =>
                                {
                                    x.Value.PosicaoFinal = ++posicao;
                                    dResultado.Add(x.Key, x.Value);
                                });

            return dResultado;
        }


        // função que calcula e faz chamadas para métodos que ajudarão a construir o relatório final
        public string CalcularResultadoCorrida(List<IVoltaCorrida> pVoltas)
        {

            // Dicionário que conterá dados relevantes de cada piloto ao final da corrida
            var dDadosPilotosCorrida = new Dictionary<int, DadosCorridaPiloto>();

            // agrupa voltas por cada piloto
            var voltasGroupedByPiloto = pVoltas.GroupBy(x => x.Piloto.Codigo);

            // para cada piloto, itere sobre suas voltas

            foreach (var voltaG in voltasGroupedByPiloto)
            {
                var voltasPiloto = voltaG.Select(volta => volta);

                var somadorVelocidades = 0.0f;

                var ultimaVolta = voltasPiloto.Last();

                //para cada volta do piloto atual
                foreach (var volta in voltasPiloto)
                {
                    if (!dDadosPilotosCorrida.ContainsKey(voltaG.Key))
                    {
                        somadorVelocidades += volta.VelocidadeMediaVolta;

                        var dadosCorrida = new DadosCorridaPiloto(volta.HoraVolta, volta.NumeroVolta, volta.TempoVolta, volta.VelocidadeMediaVolta, volta.Piloto);

                        dDadosPilotosCorrida.Add(voltaG.Key, dadosCorrida);
                    }
                    else
                    {
                        //se o piloto já esta no hashmap dados_pilotos_final_da_corrida, atualize seus dados//
                        DadosCorridaPiloto dadosPiloto = dDadosPilotosCorrida[voltaG.Key];

                        //atualiza hora da última volta
                        dadosPiloto.HoraUltimaVolta = volta.HoraVolta;
                        //atualiza número da volta
                        dadosPiloto.NumeroDeVoltasTotais = volta.NumeroVolta;
                        //atualiza velocidadeTotal
                        dadosPiloto.VelocidadeTotalCorrida = dadosPiloto.VelocidadeTotalCorrida + volta.VelocidadeMediaVolta;
                        //atualiza tempo total durante a corrida
                        dadosPiloto.TempoTotalCorrida = TimeSpanUtils.OperarTimeSpans(dadosPiloto.TempoTotalCorrida, volta.TempoVolta, ETimeSpanOperacao.Adicao);

                        //se tempo da volta atual for melhor (menor) que o anterior, atualize este valor
                        if (volta.TempoVolta < dadosPiloto.MelhorVolta)
                            dadosPiloto.MelhorVolta = volta.TempoVolta;

                        // variável que armazena a soma das velocidades de cada volta do piloto atual
                        somadorVelocidades += volta.VelocidadeMediaVolta;

                        // se for a última volta do piloto atual calcule sua Velocidade Média
                        if (volta.Equals(ultimaVolta))
                        {
                            float velocidadeMedia = somadorVelocidades / voltasPiloto.Count();
                            dadosPiloto.VelocidadeMediaCorrida = velocidadeMedia;

                            somadorVelocidades = 0.0F; // reseta somadorVelocidades
                        }

                        //atualiza dados do piloto atual no dicionário dDadosPilotosCorrida
                        dDadosPilotosCorrida[voltaG.Key] = dadosPiloto;
                    }
                }
            }

            //Dicionário com classificação final dos pilotos
            Dictionary<int, DadosCorridaPiloto> posicaoFinal = ComputarPosicaoFinalDosPilotos(dDadosPilotosCorrida);

            //gera relatório da classificação final da corrida
            CalcularClassificacaoFinalPilotos(posicaoFinal);

            //gera relatório da melhor volta de cada piloto na corrida
            CalcularMelhorVoltaDeCadaPiloto(dDadosPilotosCorrida);

            //gera relatório da melhor volta da corrida
            CalcularMelhorVoltaDaCorrida(dDadosPilotosCorrida);

            //gera relatório da velocidade média de cada piloto na corrida
            CalcularVelocidadeMediaDeCadaPiloto(dDadosPilotosCorrida);

            return RelatorioFinal.ToString();

        }

        // função que desenha "-----------..." em cima e em baixo dos nomes das colunas do relatorio final
        // apenas formatação
        public string CriarLinhaFormatadoraColunasTabelas(StringBuilder stringbuilder)
        {
            return string.Join("", new String('-', stringbuilder.ToString().Length));
        }

        public void CalcularMelhorVoltaDeCadaPiloto(Dictionary<int, DadosCorridaPiloto> pDadosVoltaPilotos)
        {

            var melhorVoltaByPilotoRelatorio = new StringBuilder();

            var formatString = "|{0,-35}|{1,-35}|{2,-35}|";
            melhorVoltaByPilotoRelatorio.AppendLine()
                                        .AppendFormat(Culture, formatString, "Código Piloto", "Nome Piloto", "Melhor Volta")
                                        .AppendLine();

            string primeiraLinha = CriarLinhaFormatadoraColunasTabelas(melhorVoltaByPilotoRelatorio);
            melhorVoltaByPilotoRelatorio.Insert(0, primeiraLinha);
            melhorVoltaByPilotoRelatorio.Append(primeiraLinha).AppendLine();

            // constrói  relatório da melhor Volta De Cada Piloto
            pDadosVoltaPilotos.ToList().ForEach(elem =>
            {

                int codigoPiloto = elem.Key;
                String nomePiloto = elem.Value.Piloto.NomePiloto;
                TimeSpan melhorVolta = elem.Value.MelhorVolta;

                melhorVoltaByPilotoRelatorio.AppendFormat(Culture, formatString, codigoPiloto.ToString("D4"), nomePiloto, melhorVolta.ToString(@"hh\:mm\:ss\:fff")).AppendLine();

            });

            //acrescenta no relatório Final título como: [ MELHOR VOLTA POR PILOTO ]
            RelatorioFinal.AppendLine().AppendLine().Append("-> [ MELHOR VOLTA POR PILOTO ]").AppendLine().AppendLine();
            //append no relatório Final dados sobre MELHOR VOLTA POR PILOTO
            RelatorioFinal.Append(melhorVoltaByPilotoRelatorio.ToString());
        }

        public void CalcularMelhorVoltaDaCorrida(Dictionary<int, DadosCorridaPiloto> pDadosVoltaPilotos)
        {

            StringBuilder melhorVoltaDaCorrida = new StringBuilder();

            var formatString = "|{0,-35}|{1,-35}|{2,-35}|";
            melhorVoltaDaCorrida.AppendLine()
                                .AppendFormat(Culture, formatString, "Código Piloto", "Nome Piloto", "Tempo da Volta")
                                .AppendLine();

            String primeiraLinha = CriarLinhaFormatadoraColunasTabelas(melhorVoltaDaCorrida);
            melhorVoltaDaCorrida.Insert(0, primeiraLinha);
            melhorVoltaDaCorrida.Append(primeiraLinha).AppendLine();


            //hashmap que sera retornado. OBS é um LinkedHashMap pois ele sempre preserva a ordem de insercão
            //pilotos ordenados pelo menor tempo de volta
            var pilotosOrdenadosByMenorTempoVolta = new Dictionary<int, DadosCorridaPiloto>();

            //ordena o dicionário pelo menor tempo de volta de cada piloto
            pDadosVoltaPilotos.ToList().OrderBy(x => x.Value.MelhorVolta).ToList()
                                .ForEach(x => pilotosOrdenadosByMenorTempoVolta.Add(x.Key, x.Value));

            DadosCorridaPiloto dadosMelhorPiloto = pilotosOrdenadosByMenorTempoVolta.FirstOrDefault().Value;

            int codigoPiloto = dadosMelhorPiloto.Piloto.Codigo;
            String nomePiloto = dadosMelhorPiloto.Piloto.NomePiloto;
            TimeSpan melhorVolta = dadosMelhorPiloto.MelhorVolta;

            melhorVoltaDaCorrida.AppendFormat(Culture, formatString, codigoPiloto.ToString("D3"), nomePiloto, melhorVolta.ToString(@"hh\:mm\:ss\:fff")).AppendLine();

            //acrescenta no relatorio Final título seção MELHOR VOLTA POR PILOTO
            RelatorioFinal.AppendLine().AppendLine().Append("-> [ MELHOR VOLTA DA CORRIDA ]").AppendLine().AppendLine();
            //append no relatorio Final dados sobre MELHOR VOLTA POR PILOTO
            RelatorioFinal.Append(melhorVoltaDaCorrida.ToString());

        }

        public void CalcularVelocidadeMediaDeCadaPiloto(Dictionary<int, DadosCorridaPiloto> pDadosVoltaPilotos)
        {

            StringBuilder velocidadeMediaByPiloto = new StringBuilder();

            var formatString = "|{0,-35}|{1,-35}|{2,-35}|";
            velocidadeMediaByPiloto.AppendLine()
                                    .AppendFormat(Culture, formatString, "Código Piloto", "Nome Piloto", "Velocidade Média")
                                    .AppendLine();

            String primeiraLinha = CriarLinhaFormatadoraColunasTabelas(velocidadeMediaByPiloto);
            velocidadeMediaByPiloto.Insert(0, primeiraLinha);
            velocidadeMediaByPiloto.Append(primeiraLinha).AppendLine();

            // constroi  relatorio da velocidade Média de Cada Piloto
            pDadosVoltaPilotos.ToList().ForEach(x =>
            {
                velocidadeMediaByPiloto.AppendFormat(Culture, formatString, x.Value.Piloto.Codigo.ToString("D3"), x.Value.Piloto.NomePiloto, x.Value.VelocidadeMediaCorrida.ToString()).AppendLine();
            });

            //acrescenta no relatorio Final título seção Velcidade Média por Piloto
            RelatorioFinal.Append(Environment.NewLine).AppendLine().Append("-> [ VELOCIDADE MÉDIA POR PILOTO ]").AppendLine().AppendLine();
            //append no relatorio Final dados sobre MELHOR VOLTA POR PILOTO
            RelatorioFinal.Append(velocidadeMediaByPiloto.ToString());
        }

        public void CalcularClassificacaoFinalPilotos(Dictionary<int, DadosCorridaPiloto> pPosicaoFinal)
        {

            StringBuilder classificacaoFinal = new StringBuilder();

            var formatString = "|{0,-30}|{1,-30}|{2,-30}|{3,-30}|{4,-30}|{5,-30}|";
            classificacaoFinal.AppendLine()
                                .AppendFormat(Culture, formatString, "Posição Chegada", "Código Piloto", "Nome Piloto", "Qtde Voltas Completadas", "Tempo Total de Prova", "Tempo de Chegada Após Vencedor")
                                .AppendLine();

            String primeiraLinha = CriarLinhaFormatadoraColunasTabelas(classificacaoFinal);
            classificacaoFinal.Insert(0, primeiraLinha);
            classificacaoFinal.Append(primeiraLinha).AppendLine();

            TimeSpan tempoChegadaVencedor = pPosicaoFinal.FirstOrDefault().Value.HoraUltimaVolta;
            int count = 1;

            // constrói  relatorio da classificação final de cada piloto
            pPosicaoFinal.ToList().ForEach(x =>
            {
                int codigoPiloto = x.Key;
                string nomePiloto = x.Value.Piloto.NomePiloto;
                int qtdVoltas = x.Value.NumeroDeVoltasTotais;
                string tempoTotal = x.Value.TempoTotalCorrida.ToString(@"hh\:mm\:ss\:fff");
                TimeSpan horaChegada = x.Value.HoraUltimaVolta;

                string diferencaToVencedor = (tempoChegadaVencedor.CompareTo(horaChegada) == 0) ? "(Não se Aplica)" : TimeSpanUtils.OperarTimeSpans(horaChegada, tempoChegadaVencedor, ETimeSpanOperacao.Subtracao).ToString(@"hh\:mm\:ss\:fff");

                classificacaoFinal.AppendFormat(Culture, formatString, count++, codigoPiloto.ToString("D3"), nomePiloto, qtdVoltas, tempoTotal, diferencaToVencedor).AppendLine();
            });


            RelatorioFinal.Append("[ RESULTADO FINAL DA CORRIDA (CLASSIFICAÇÃO) E DIFERENÇA DE TEMPO DE CHEGADA DE CADA PILOTO PARA O VENCEDOR ]").AppendLine().AppendLine();
            RelatorioFinal.Append(classificacaoFinal.ToString());
        }
    }
}