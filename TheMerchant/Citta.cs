using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;

public class Citta
{

    public float Stima { get; set; }
    public Dictionary<Prodotto, int> Abbondante { get; set; } = new Dictionary<Prodotto, int>();
    public Dictionary<Prodotto, int> Carente { get; set; } = new Dictionary<Prodotto, int>();
    public List<Cliente> Clienti { get; set; } = new List<Cliente>();
    public Evento EventoInCorso { get; set; } = Evento.Nessuno;

    //funzione per creare un cliente con caratteristiche casuali e un prodotto desiderato basato sulla classe sociale
    public void CreaCliente()
    {
        Random rand = new Random();
        Cliente cliente = new Cliente
        {
            Sesso = rand.Next(0, 2) == 0,
            ClasseSociale = (TipoClasse)rand.Next(0, 3),
            Pazienza = 1f, //pazienza minima
            ProdottoDesiderato = null 
        };

        cliente.Pazienza = scegliPazienza(cliente);
        if(scegliSeComprare(cliente)) cliente.ProdottoDesiderato = ScegliProdotto(cliente);
         
        Clienti.Add(cliente);

        Console.WriteLine($"Nuovo cliente: Sesso: {(cliente.Sesso ? "Maschio" : "Femmina")}, Classe Sociale: {cliente.ClasseSociale}, Pazienza: {cliente.Pazienza}, Prodotto Desiderato: {(cliente.ProdottoDesiderato.HasValue ? cliente.ProdottoDesiderato.Value.Item1.Nome : "Nessuno")}");
    }

    //funzione per scegliere un prodotto desiderato in base alla classe sociale del cliente e alla disponibilità dei prodotti
    private (Prodotto, int)? ScegliProdotto(Cliente cliente)
    {
        Random rand = new Random();
        //la classe sociale bassa sceglie sicuramente un prodotto
        if(cliente.ClasseSociale == TipoClasse.Bassa)
        {
            var prodottoScelto = Disponibili.Prodotti.Where(p => p.Key.ClasseSociale == TipoClasse.Bassa).Select(p => p.Key).ToList();
            
            if (!prodottoScelto.Any()) return null;

            var tuttiProdotti = Abbondante.Keys
            .Concat(Carente.Keys)
            .Distinct();

            var opzioniDisponibili = tuttiProdotti
            .Where(p => prodottoScelto.Contains(p))
            .ToList();


            if (opzioniDisponibili.Any()) return ScegliProdottoRandom(opzioniDisponibili);

            return null;
    
        }
        //la classe sociale media sceglie un prodotto con una certa probabilità, preferendo quelli carenti
        if(cliente.ClasseSociale == TipoClasse.Media)
        {
            var prodottoScelto = Disponibili.Prodotti.Where(p => p.Key.ClasseSociale == TipoClasse.Media).Select(p => p.Key).ToList();
            
            if (!prodottoScelto.Any()) return null;

            var tuttiProdotti = Abbondante.Keys
            .Concat(Carente.Keys)
            .Distinct();

            var opzioniDisponibili = tuttiProdotti
            .Where(p => prodottoScelto.Contains(p))
            .ToList();

            if (Carente.Keys.Any(k => opzioniDisponibili.Contains(k))) return ScegliProdottoRandom(opzioniDisponibili);
            else if (Abbondante.Keys.Any(k => opzioniDisponibili.Contains(k)))
            {
                int scelta = rand.Next(0, 10);
                if (scelta < 4) return ScegliProdottoRandom(opzioniDisponibili);
            }

            return null;
        }
        //la classe sociale alta sceglie un prodotto sicuramente(essendo rari) e senza preferenze particolari
        else{

            var prodottoScelto = Disponibili.Prodotti.Where(p => p.Key.ClasseSociale == TipoClasse.Alta).Select(p => p.Key).ToList();
            
            if (!prodottoScelto.Any()) return null;

            var tuttiProdotti = Abbondante.Keys
            .Concat(Carente.Keys)
            .Distinct();

            var opzioniDisponibili = tuttiProdotti
            .Where(p => prodottoScelto.Contains(p))
            .ToList();


            if (opzioniDisponibili.Any()) return ScegliProdottoRandom(opzioniDisponibili);

            return null;
        }


    }

    private (Prodotto, int) ScegliProdottoRandom(List<Prodotto> opzioni)
    {
        Random rand = new Random();
        int indiceCasuale = rand.Next(opzioni.Count);
        Prodotto prodottoTrovato= opzioni[indiceCasuale];
        return (prodottoTrovato, 1);
    }

    private float scegliPazienza(Cliente cliente)
    {
        Random rand = new Random();
        double calcolo = cliente.Pazienza;

        if(cliente.ClasseSociale == TipoClasse.Bassa)
            calcolo += rand.NextDouble() * 1 + 0.5; //aggiunge pazienza tra 0.5 e 1.5
        else if(cliente.ClasseSociale == TipoClasse.Media)
            calcolo += rand.NextDouble() * 0.5 + 0.25; //aggiunge pazienza tra 0.25 e 0.75
        else
            calcolo += rand.NextDouble() * 0.5; //aggiunge pazienza tra 0 e 0.5
        return (float)Math.Round(calcolo, 2);
    }

    private bool scegliSeComprare(Cliente cliente)
    {
        Random rand = new Random();
        if(cliente.ClasseSociale == TipoClasse.Bassa)
        {
            int scelta = rand.Next(0, 3);
            if (scelta < 2) return true;
        }
        else
        {
            int scelta = rand.Next(0, 5);
            if (scelta < 2) return true;
        }

        return false;

    }
}
