namespace ReservaDeSalas
{
    public class GerenciadorDeReservasSala
    {
        private static GerenciadorDeReservasSala instance;
        private GerenciadorDeReservasSala() { }

        public static GerenciadorDeReservasSala getInstance()
        {
            if (instance == null) instance = new GerenciadorDeReservasSala();
            return instance;
        }
    }
}