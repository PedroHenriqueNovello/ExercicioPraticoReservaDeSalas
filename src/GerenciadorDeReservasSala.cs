namespace ReservaDeSalas
{
    public class GerenciadorDeReservasSala
    {
        private GerenciadorDeReservasSala() { }
        private static GerenciadorDeReservasSala instance;
        private static readonly object _trava = new object();

        public static GerenciadorDeReservasSala getInstance()
        {
            lock (_trava)
            {
                if (instance == null) instance = new GerenciadorDeReservasSala();
                return instance;
            }
        }
    }
}
