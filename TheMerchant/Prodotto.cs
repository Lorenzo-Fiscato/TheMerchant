public struct Prodotto
{
    public string Nome { get; set; }
    public TipoClasse ClasseSociale { get; set; }
    public bool Stackable { get; init;}
    public double TolleranzaPrezzo { get; set; } //indica di quanto il cliente è disposto a pagare in più 
}