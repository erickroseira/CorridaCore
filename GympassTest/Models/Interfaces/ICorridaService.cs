using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace gympass.Models.Interfaces
{
    public interface ICorridaService
    {
        //String builder que conterá relatorio final
        StringBuilder RelatorioFinal { get; set; }

        CultureInfo Culture { get; set; }

        ///

        Dictionary<int, DadosCorridaPiloto> ComputarPosicaoFinalDosPilotos(Dictionary<int, DadosCorridaPiloto> pPrePosicaoPilotos);

        // função que calcula e faz chamadas para métodos que ajudarão a construir o relatório final
        string CalcularResultadoCorrida(List<IVoltaCorrida> pVoltas);

        // função que desenha "-----------..." em cima e em baixo dos nomes das colunas do relatorio final
        // apenas formatação
        string CriarLinhaFormatadoraColunasTabelas(StringBuilder stringbuilder);

        void CalcularMelhorVoltaDeCadaPiloto(Dictionary<int, DadosCorridaPiloto> pDadosVoltaPilotos);

        void CalcularMelhorVoltaDaCorrida(Dictionary<int, DadosCorridaPiloto> pDadosVoltaPilotos);

        void CalcularVelocidadeMediaDeCadaPiloto(Dictionary<int, DadosCorridaPiloto> pDadosVoltaPilotos);

        void CalcularClassificacaoFinalPilotos(Dictionary<int, DadosCorridaPiloto> pPosicaoFinal);
    }
}