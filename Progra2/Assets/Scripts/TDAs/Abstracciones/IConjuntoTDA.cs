namespace TDAs.Abstracciones
{ 
    public interface IConjuntoTDA
    {
        void InicializarConjunto();
        bool ConjuntoVacio();
        void Agregar(int x);
        int Elegir();
        void Sacar(int x);
        bool Pertenece(int x);
    }
}