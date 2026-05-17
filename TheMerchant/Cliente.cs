public class Cliente
{
    public bool Sesso { get; set; }
    public TipoClasse ClasseSociale { get; set; }
    public float Pazienza { get; set; }
    public (Prodotto, int)? ProdottoDesiderato { get; set; }
}