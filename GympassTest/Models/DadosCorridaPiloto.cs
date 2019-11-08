using System;

namespace gympass.Models
{
    public class DadosCorridaPiloto
    {
        #region Declaração Propriedades

        private IPiloto _piloto;
        public IPiloto Piloto
        {
            get { return _piloto; }
            set { _piloto = value; }
        }

        private int _posicaoFinal;
        public int PosicaoFinal
        {
            get { return _posicaoFinal; }
            set { _posicaoFinal = value; }
        }

        private TimeSpan _horaUltimaVolta;
        public TimeSpan HoraUltimaVolta
        {
            get
            {
                return _horaUltimaVolta;
            }
            set
            {
                _horaUltimaVolta = value;
            }
        }

        private int _numeroDeVoltasTotais;
        public int NumeroDeVoltasTotais
        {
            get
            {
                return _numeroDeVoltasTotais;
            }
            set
            {
                _numeroDeVoltasTotais = value;
            }
        }

        private TimeSpan _tempoTotalCorrida;
        public TimeSpan TempoTotalCorrida
        {
            get
            {
                return _tempoTotalCorrida;
            }
            set
            {
                _tempoTotalCorrida = value;
            }
        }

        private float _velocidadeTotalCorrida;
        public float VelocidadeTotalCorrida
        {
            get
            {
                return _velocidadeTotalCorrida;
            }
            set
            {
                _velocidadeTotalCorrida = value;
            }
        }

        private TimeSpan _melhorVolta;
        public TimeSpan MelhorVolta
        {
            get
            {
                return _melhorVolta;
            }
            set
            {
                _melhorVolta = value;
            }
        }

        private float _velocidadeMediaCorrida;
        public float VelocidadeMediaCorrida
        {
            get
            {
                return _velocidadeMediaCorrida;
            }
            set
            {
                _velocidadeMediaCorrida = value;
            }
        }

        #endregion

        public DadosCorridaPiloto(TimeSpan pHoraUltimaVolta, int pNumeroDeVoltasTotais, TimeSpan pTempoTotalCorrida, float pVelocidadeTotalCorrida, IPiloto pPiloto)
        {
            HoraUltimaVolta = pHoraUltimaVolta;
            NumeroDeVoltasTotais = pNumeroDeVoltasTotais;
            TempoTotalCorrida = pTempoTotalCorrida;
            VelocidadeTotalCorrida = pVelocidadeTotalCorrida;

            //assume que a melhor volta é a primeira. Depois esta informação é verificado
            // posteriormente com os dados das voltas posteriores de cada piloto
            MelhorVolta = pTempoTotalCorrida;

            Piloto = pPiloto;
        }
    }
}