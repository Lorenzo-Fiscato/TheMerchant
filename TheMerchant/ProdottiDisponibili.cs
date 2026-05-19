public static class Disponibili
{
    public static Dictionary<Prodotto, (int Quantita, float Prezzo)> Prodotti { get; set; } =
     new Dictionary<Prodotto,(int Quantita, float Prezzo)>();
}