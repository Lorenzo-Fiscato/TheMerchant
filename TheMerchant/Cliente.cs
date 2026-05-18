public class Cliente
{
    public bool Sesso { get; set; }
    public TipoClasse ClasseSociale { get; set; }
    public double Pazienza { get; set; }
    public (Prodotto, double)? ProdottoDesiderato { get; set; }
}