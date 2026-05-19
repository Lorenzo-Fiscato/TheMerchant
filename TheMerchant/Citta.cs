using System.Diagnostics.Tracing;
using System.Net;
using System.Runtime.CompilerServices;

public class Citta
{

    public float Stima { get; set; }
    public Dictionary<Prodotto, float> AbbondanteInCitta { get; set; } = new Dictionary<Prodotto, float>();
    public Dictionary<Prodotto, float> Prodotti { get; set; } = new Dictionary<Prodotto, float>();
    public List<Cliente> Clienti { get; set; } = new List<Cliente>();
    public Evento EventoInCorso { get; set; } = Evento.Nessuno;

    private Random rand = new Random();

    //funzione per creare un cliente con caratteristiche casuali e un prodotto desiderato basato sulla classe sociale
    public void CreaCliente()
    {
        Cliente cliente = new Cliente
        {
            cittaAppartenenza = this,
            Sesso = rand.Next(0, 2) == 0,
            ClasseSociale = scegliClasseSociale,
            Pazienza = 0.5f, //pazienza minima
        };

        cliente.Pazienza = scegliPazienza(cliente);
        
        if((cliente.ProdottoDesiderato = ScegliProdotto(cliente)) != null)
            if(!scegliSeComprare(cliente)) cliente.ProdottoDesiderato = null;
            cliente.ProdottoDesiderato = (Prodotti.First().Key, Prodotti[Prodotti.First().Key]);

        Clienti.Add(cliente);

        Console.WriteLine($"Nuovo cliente: Sesso: {(cliente.Sesso ? "Maschio" : "Femmina")}, Classe Sociale: {cliente.ClasseSociale}, Pazienza: {cliente.Pazienza}, Prodotto Desiderato: {(cliente.ProdottoDesiderato.HasValue ? cliente.ProdottoDesiderato.Value.Item1.Nome : "Nessuno")}");
    }

    //funzione per scegliere un prodotto desiderato in base alla classe sociale del cliente e alla disponibilità dei prodotti
    private (Prodotto, float)? ScegliProdotto(Cliente cliente)
    {

    //ottiene una lista dei prodotti che il cliente può comprare tra quelli disponibili
    List<Prodotto> prodottiPossibili = prodottiScelti(cliente.ClasseSociale);

    if(!prodottiPossibili.Any()) return null;

    //sceglie un prodotto random tra quelli disponibili
    var prodottoRand = ScegliProdottoRandom(prodottiPossibili);


    //la classe sociale bassa o ricca sceglie sicuramente un prodotto
    if(cliente.ClasseSociale == TipoClasse.Bassa ||  cliente.ClasseSociale == TipoClasse.Alta) return prodottoRand;

    //la classe sociale media sceglie un prodotto con una certa probabilità, preferendo quelli che scarseggiano in città
    else
    {
        // Separiamo i prodotti scarsi da quelli abbondanti presenti in questa selezione
        var opzioniScarse = prodottiPossibili.Where(p => !AbbondanteInCitta.ContainsKey(p)).ToList();

        if (opzioniScarse.Any())
            // Se ci sono opzioni scarse in vetrina, la classe media preferisce quelle e compra sicuro!
            return ScegliProdottoRandom(opzioniScarse);
        else
        {
        // Se in vetrina ci sono SOLO oggetti abbondanti, allora tentiamo l'acquisto con la probabilità
        double scelta = rand.NextDouble() * 4;
        if (scelta < 1.5 * Stima * ConfigurazioneEventi.ModificatoreProbComprare[EventoInCorso][prodottoRand.Item1.Tag])
            return prodottoRand;
        }
    }

    return null;
}

    //Func che ritorna prodotti corrispondenti alla classe tra i prodotti disponibili
    private Func<TipoClasse, List<Prodotto>> prodottiScelti = c => Disponibili.Prodotti.Where(p => p.Key.ClasseSociale == c).Select(p => p.Key).ToList();


    //ritorna un elemento random tra quelli possibili
    private (Prodotto, float) ScegliProdottoRandom(List<Prodotto> opzioni)
    {
        int indiceCasuale = rand.Next(opzioni.Count);
        Prodotto prodottoTrovato = opzioni[indiceCasuale];
    
        // Se il prodotto è abbondante in città, prendiamo il suo prezzo scontato,
        // altrimenti prendiamo il prezzo standard dal dizionario globale dei prodotti
        float prezzoBaseLocale = AbbondanteInCitta.ContainsKey(prodottoTrovato) 
            ? AbbondanteInCitta[prodottoTrovato] 
            : Prodotti[prodottoTrovato];
        
        // Applichiamo il modificatore dell'evento basato sul Tag
        float prezzoFinale = prezzoBaseLocale * ConfigurazioneEventi.ModificatorePrezzo[EventoInCorso][prodottoTrovato.Tag];
    
        return (prodottoTrovato, (float)Math.Round(prezzoFinale, 2));
    }

    //funzione per scegliere la pazienza del cliente in base alla classe sociale, con un po' di casualità
    private float scegliPazienza(Cliente cliente)
    {
        double calcolo = cliente.Pazienza;

        if(cliente.ClasseSociale == TipoClasse.Bassa)
            calcolo += rand.NextDouble() * 0.5 + 0.25; //aggiunge pazienza tra 0.25 e 0.75
        else if(cliente.ClasseSociale == TipoClasse.Media)
            calcolo += rand.NextDouble() * 0.25 + 0.125; //aggiunge pazienza tra 0.125 e 0.375
        else
            calcolo += rand.NextDouble() * 0.25; //aggiunge pazienza tra 0 e 0.25
        return (float)Math.Round(calcolo * ConfigurazioneEventi.ModificatorePazienza[EventoInCorso], 2);
    }

    //funzione per decidere se il cliente compra un prodotto
    private bool scegliSeComprare(Cliente cliente)
    {
        double scelta = rand.NextDouble() * (cliente.ClasseSociale == TipoClasse.Bassa ? 3 : 4);

        if (scelta < 1.5 * Stima * ConfigurazioneEventi.ModificatoreProbComprare[EventoInCorso][cliente.ProdottoDesiderato.Value.Item1.Tag]) return true;

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
