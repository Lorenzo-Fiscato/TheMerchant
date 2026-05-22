public struct Prodotto
{
    public string Nome { get; set; }
    public TipoClasse ClasseSociale { get; set; }
    public bool Stackable { get; init;}
    public float TolleranzaPrezzo { get; set; } //indica di quanto il cliente è disposto a pagare in più 
    public TagProdotto Tag { get; set; } //tag per identificare il tipo di prodotto, usato per eventi e modificatori
}