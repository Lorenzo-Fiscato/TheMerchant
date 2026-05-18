using System.Diagnostics.Tracing;
using System.Net;
using System.Runtime.CompilerServices;

public class Citta
{

    public float Stima { get; set; }
    public Dictionary<Prodotto, double> AbbondanteInCitta { get; set; } = new Dictionary<Prodotto, double>();
    public Dictionary<Prodotto, double> Prodotti { get; set; } = new Dictionary<Prodotto, double>();
    public List<Cliente> Clienti { get; set; } = new List<Cliente>();
    public Evento EventoInCorso { get; set; } = Evento.Nessuno;

    private Random rand = new Random();

    //funzione per creare un cliente con caratteristiche casuali e un prodotto desiderato basato sulla classe sociale
    public void CreaCliente()
    {
        Cliente cliente = new Cliente
        {
            Sesso = rand.Next(0, 2) == 0,
            ClasseSociale = scegliClasseSociale,
            Pazienza = 1, //pazienza minima
        };

        cliente.Pazienza = scegliPazienza(cliente);
        
        if((cliente.ProdottoDesiderato = ScegliProdotto(cliente)) != null)
            if(!scegliSeComprare(cliente)) cliente.ProdottoDesiderato = null;

        Clienti.Add(cliente);

        Console.WriteLine($"Nuovo cliente: Sesso: {(cliente.Sesso ? "Maschio" : "Femmina")}, Classe Sociale: {cliente.ClasseSociale}, Pazienza: {cliente.Pazienza}, Prodotto Desiderato: {(cliente.ProdottoDesiderato.HasValue ? cliente.ProdottoDesiderato.Value.Item1.Nome : "Nessuno")}");
    }

    //funzione per scegliere un prodotto desiderato in base alla classe sociale del cliente e alla disponibilità dei prodotti
    private (Prodotto, double)? ScegliProdotto(Cliente cliente)
    {

    //ottiene una lista dei prodotti che il cliente può comprare tra quelli disponibili
    List<Prodotto> prodottiPossibili = prodottiScelti(cliente.ClasseSociale);

    if(!prodottiPossibili.Any()) return null;

    //sceglie un prodotto random tra quelli disponibili
    var prodottoRand = ScegliProdottoRandom(prodottiPossibili);


    //la classe sociale bassa o ricca sceglie sicuramente un prodotto
    if(cliente.ClasseSociale == TipoClasse.Bassa ||  cliente.ClasseSociale == TipoClasse.Alta) return prodottoRand;

    //la classe sociale media sceglie un prodotto con una certa probabilità, preferendo quelli che scarseggianoo in città
    else
    {
        //se sono presenti prodotti che scarseggiano in città compra sicuramente
        if (!AbbondanteInCitta.Keys.Any(k => prodottiPossibili.Contains(k))) return ScegliProdottoRandom(prodottiPossibili);
        //altrimenti compra con una probabilità che dipende dalla stima della città
        else
        {
            double scelta = rand.NextDouble() * 4;
            if (scelta < 1 * Stima * ConfigurazioneEventi.Moltiplicatori[EventoInCorso].ProbComprare) return prodottoRand;
        }

    }

    return null;
}

    //Func che ritorna prodotti corrispondenti alla classe tra i prodotti disponibili
    private Func<TipoClasse, List<Prodotto>> prodottiScelti = c => Disponibili.Prodotti.Where(p => p.Key.ClasseSociale == c).Select(p => p.Key).ToList();


    //ritorna un elemento random tra quelli possibili
    private (Prodotto, double) ScegliProdottoRandom(List<Prodotto> opzioni)
    {
        int indiceCasuale = rand.Next(opzioni.Count);
        Prodotto prodottoTrovato = opzioni[indiceCasuale];
        
        return (prodottoTrovato, Prodotti[prodottoTrovato] * ConfigurazioneEventi.Moltiplicatori[EventoInCorso].Prezzo);
    }

    //funzione per scegliere la pazienza del cliente in base alla classe sociale, con un po' di casualità
    private double scegliPazienza(Cliente cliente)
    {
        double calcolo = cliente.Pazienza;

        if(cliente.ClasseSociale == TipoClasse.Bassa)
            calcolo += rand.NextDouble() * 1 + 0.5; //aggiunge pazienza tra 0.5 e 1.5
        else if(cliente.ClasseSociale == TipoClasse.Media)
            calcolo += rand.NextDouble() * 0.5 + 0.25; //aggiunge pazienza tra 0.25 e 0.75
        else
            calcolo += rand.NextDouble() * 0.5; //aggiunge pazienza tra 0 e 0.5
        return Math.Round(calcolo + ConfigurazioneEventi.Moltiplicatori[EventoInCorso].Pazienza, 2);
    }

    //funzione per decidere se il cliente compra un prodotto
    private bool scegliSeComprare(Cliente cliente)
    {
        if(cliente.ClasseSociale == TipoClasse.Bassa)
        {
            double scelta = rand.NextDouble() * 3;
            if (scelta < 1.5 * Stima * ConfigurazioneEventi.Moltiplicatori[EventoInCorso].ProbComprare) return true;
        }
        else
        {
            double scelta = rand.NextDouble() * 5;
            if (scelta < 2 * Stima * ConfigurazioneEventi.Moltiplicatori[EventoInCorso].ProbComprare) return true;
        }

        return false;

    }

    //funzione per scegliere la classe sociale del cliente con una certa distribuzione
    private TipoClasse scegliClasseSociale
    {
        get
        {
            int scelta = rand.Next(0, 100);
            if(scelta < 20) return TipoClasse.Bassa; //20% di probabilità
            else if(scelta < 80) return TipoClasse.Media; //60% di probabilità
            else return TipoClasse.Alta; //20% di probabilità
        }
    }
}
