using System;

namespace gympass.Models
{
    public class VoltaCorrida : IVoltaCorrida
    {
        private TimeSpan _horaVolta;
        public TimeSpan HoraVolta { get => _horaVolta; set => _horaVolta = value; }

        private IPiloto _piloto;
        public IPiloto Piloto { get => _piloto; set => _piloto = value; }

        private int _numeroVolta;
        public int NumeroVolta { get => _numeroVolta; set => _numeroVolta = value; }

        private TimeSpan _tempoVolta;
        public TimeSpan TempoVolta { get => _tempoVolta; set => _tempoVolta = value; }

        private float _velocidadeMediaVolta;
        public float VelocidadeMediaVolta { get => _velocidadeMediaVolta; set => _velocidadeMediaVolta = value; }

        public VoltaCorrida(TimeSpan pHoraVolta, Piloto pPiloto, int pNumeroVolta, TimeSpan pTempoVolta, float pVelocidadeMediaVolta)
        {
            Piloto = pPiloto;
            NumeroVolta = pNumeroVolta;
            HoraVolta = pHoraVolta;
            TempoVolta = pTempoVolta;
            VelocidadeMediaVolta = pVelocidadeMediaVolta;
        }
    }
}