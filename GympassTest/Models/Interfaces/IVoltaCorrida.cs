using System;

namespace gympass.Models
{
    public interface IVoltaCorrida
    {
        TimeSpan HoraVolta { get; set; }

        IPiloto Piloto { get; set; }

        int NumeroVolta { get; set; }

        TimeSpan TempoVolta { get; set; }

        float VelocidadeMediaVolta { get; set; }

    }
}