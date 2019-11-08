using System;
using gympass.Models;

namespace gympass.Utilities
{
    public class TimeSpanUtils
    {
        // função auxiliar para somar TimeSpans
        public static TimeSpan OperarTimeSpans(TimeSpan pDt1, TimeSpan pDt2, ETimeSpanOperacao pOperacao)
        {
            if (pOperacao == ETimeSpanOperacao.Adicao)
                return pDt1.Add(pDt2);
            else
                return pDt1.Subtract(pDt2);

            // TimeSpan dateTime = pDt1.Add(pSinal * pDt2.Hour); //soma horas
            // dateTime = dateTime.AddMinutes(pSinal * pDt2.Minute); //soma minutos
            // dateTime = dateTime.AddSeconds(pSinal * pDt2.Second); //soma segundos
            // dateTime = dateTime.AddMilliseconds(pSinal * pDt2.Millisecond); //soma milisegundos

            // return dateTime;
        }
    }
}