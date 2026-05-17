public static class Disponibili
{
    public static Dictionary<Prodotto, (int Quantita, int Prezzo)> Prodotti { get; set; } = new Dictionary<Prodotto, (int, int)>();
}