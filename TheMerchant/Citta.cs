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
            Personalita = scegliPersonalita
        };

        cliente.Pazienza = scegliPazienza(cliente);
        
        if((cliente.ProdottoDesiderato = scegliProdotto(cliente)) != null)
            if(!scegliSeComprare(cliente)) cliente.ProdottoDesiderato = null;

        Clienti.Add(cliente);

        Console.WriteLine($"Nuovo cliente: Sesso: {(cliente.Sesso ? "Maschio" : "Femmina")}, Classe Sociale: {cliente.ClasseSociale}, Pazienza: {cliente.Pazienza}, Prodotto Desiderato: {(cliente.ProdottoDesiderato.HasValue ? cliente.ProdottoDesiderato.Value.Item1.Nome : "Nessuno")}");
    }

    //funzione per scegliere un prodotto desiderato in base alla classe sociale del cliente e alla disponibilità dei prodotti
    private (Prodotto, float, int)? scegliProdotto(Cliente cliente)
    {

    //ottiene una lista dei prodotti che il cliente può comprare tra quelli disponibili
    List<Prodotto> prodottiPossibili = GetProdottiPerClasse(cliente.ClasseSociale);

    if(!prodottiPossibili.Any()) return null;

    //sceglie un prodotto random tra quelli disponibili
    var prodottoRand = scegliProdottoRandom(prodottiPossibili);
    
    int quantita = 1; //di default è 1 per i prodotti non stackable

    if (prodottoRand.Item1.Stackable)
    {
        int maxQuantita = ConfigurazioneEventi.ModificatoreQuantita[EventoInCorso][prodottoRand.Item1.Tag] >
        Disponibili.Prodotti[prodottoRand.Item1].Item1 ?
            Disponibili.Prodotti[prodottoRand.Item1].Item1 
            : 
            ConfigurazioneEventi.ModificatoreQuantita[EventoInCorso][prodottoRand.Item1.Tag];

        quantita = rand.Next(1, maxQuantita + 1);    
    }
    
    //la classe sociale bassa o ricca sceglie sicuramente un prodotto
    if(cliente.ClasseSociale == TipoClasse.Bassa ||  cliente.ClasseSociale == TipoClasse.Alta) 
        return (prodottoRand.Item1, prodottoRand.Item2, quantita); //sceglie una quantità random tra 1 e la quantità massima per quel prodotto

    //la classe sociale media sceglie un prodotto con una certa probabilità, preferendo quelli che scarseggiano in città
    else
    {
        // Separiamo i prodotti scarsi da quelli abbondanti presenti in questa selezione
        var opzioniScarse = prodottiPossibili.Where(p => !AbbondanteInCitta.ContainsKey(p)).ToList();

        // Se ci sono opzioni scarse in vetrina, la classe media preferisce quelle e compra sicuro!
        if (opzioniScarse.Any())
            {
                prodottoRand = scegliProdottoRandom(opzioniScarse);
                return (prodottoRand.Item1, prodottoRand.Item2, quantita);
            }

        else
        {
        // Se in vetrina ci sono SOLO oggetti abbondanti, allora tentiamo l'acquisto con la probabilità
        double scelta = rand.NextDouble() * 4;
        if (scelta < 1.5 * Stima * ConfigurazioneEventi.ModificatoreProbComprare[EventoInCorso][prodottoRand.Item1.Tag])
            return (prodottoRand.Item1, prodottoRand.Item2, quantita);
        }
    }

    return null;
}

    //ritorna prodotti corrispondenti alla classe tra i prodotti disponibili
    private List<Prodotto> GetProdottiPerClasse(TipoClasse classe) => 
    Disponibili.Prodotti.Where(p => p.Key.ClasseSociale == classe).Select(p => p.Key).ToList();


    //ritorna un elemento random tra quelli possibili
    private (Prodotto, float) scegliProdottoRandom(List<Prodotto> opzioni)
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
            calcolo += rand.NextDouble() * 0.5 * ModificatoriPersonalita.ModificatorePazienza[cliente.Personalita] + 0.25; //aggiunge pazienza tra 0.25 e 0.75
        else if(cliente.ClasseSociale == TipoClasse.Media)
            calcolo += rand.NextDouble() * 0.25 * ModificatoriPersonalita.ModificatorePazienza[cliente.Personalita] + 0.125; //aggiunge pazienza tra 0.125 e 0.375
        else
            calcolo += rand.NextDouble() * 0.25 * ModificatoriPersonalita.ModificatorePazienza[cliente.Personalita]; //aggiunge pazienza tra 0 e 0.25
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

    private PersonalitaCliente scegliPersonalita
    {
        get
        {
            int scelta = rand.Next(0, 100);
            if(scelta < 60) return PersonalitaCliente.Normale; //60% di probabilità
            else if(scelta < 80) return PersonalitaCliente.Frettoloso; //20% di probabilità
            else return PersonalitaCliente.Trattatore; //20% di probabilità
        }
    }
}
