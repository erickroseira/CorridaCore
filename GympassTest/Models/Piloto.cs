namespace gympass.Models
{
    public class Piloto : IPiloto
    {
        private int _codigo;
        public int Codigo { get => _codigo; set => _codigo = value; }

        private string _nomePiloto;
        public string NomePiloto { get => _nomePiloto; set => _nomePiloto = value; }

        public Piloto(int pCodigo, string pNomePilot)
        {
            Codigo = pCodigo;
            NomePiloto = pNomePilot;
        }
    }
}