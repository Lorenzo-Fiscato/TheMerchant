using System.Globalization;

Prodotto pane = new Prodotto
{
    Nome = "Pane",
    ClasseSociale = TipoClasse.Bassa,
    Stackable = true,
    TolleranzaPrezzo = 1.5f,
    Tag = TagProdotto.CiboPovero
};

Prodotto grano = new Prodotto
{
    Nome = "Grano",
    ClasseSociale = TipoClasse.Bassa,
    Stackable = true,
    TolleranzaPrezzo = 1.5f,
    Tag = TagProdotto.BeneDiPrimaNecessita
};

Prodotto vino = new Prodotto
{
    Nome = "Vino",
    ClasseSociale = TipoClasse.Media,
    Stackable = true,
    TolleranzaPrezzo = 1.2f,
    Tag = TagProdotto.CiboRicco
};

Prodotto collana = new Prodotto
{
    Nome = "Collana",
    ClasseSociale = TipoClasse.Alta,
    Stackable = false,
    TolleranzaPrezzo = 1.2f,
    Tag = TagProdotto.Lusso
};

Prodotto farina = new Prodotto
{
    Nome = "Farina",
    ClasseSociale = TipoClasse.Media,
    Stackable = true,
    TolleranzaPrezzo = 1.5f,
    Tag = TagProdotto.BeneDiPrimaNecessita
};

Prodotto legno = new Prodotto
{
    Nome = "Legno",
    ClasseSociale = TipoClasse.Media,
    Stackable = true,
    TolleranzaPrezzo = 1.5f,
    Tag = TagProdotto.Materiale
};
Citta start = new Citta
{
    Stima = 1,
    AbbondanteInCitta = new Dictionary<Prodotto, float>
    {
        { pane, 5 },
        { vino, 10 },
        { grano, 3 }
    },
    Prodotti = new Dictionary<Prodotto, float>
    {
        { grano, 3 },
        {collana, 200 },
        { pane, 5 },
        { vino, 11 },
        { farina, 4.2f },
        { legno, 6.5f }
    },
};

Inventario.Prodotti.Add(pane, 10);
Inventario.Prodotti.Add(vino, 5);
Inventario.Prodotti.Add(collana, 1);

Disponibili.Prodotti.Add(pane, (5, 5));
Disponibili.Prodotti.Add(vino, (10, 11));
Disponibili.Prodotti.Add(collana, (1, 200));
Disponibili.Prodotti.Add(grano, (50, 3.1f));
Disponibili.Prodotti.Add(farina, (20, 4));
Disponibili.Prodotti.Add(legno, (15, 6));


// --- CONFIGURAZIONE DELLA GIORNATA ---
int clientiTotaliOggi = 20; // Quanti clienti totali passeranno oggi in bottega

Console.WriteLine("=================================================================");
Console.WriteLine($" APERTURA BOTTEGA - Stima iniziale della città: {start.Stima:F2}");
Console.WriteLine("=================================================================");

for (int i = 1; i <= clientiTotaliOggi; i++)
{
    Console.WriteLine($"\n==================== [ CLIENTE {i} DI {clientiTotaliOggi} ] ====================");
    
    // 1. Generiamo UN SOLO cliente alla volta. 
    // Questa funzione stamperà le sue caratteristiche (Sesso, Classe, Pazienza, ecc.)
    start.CreaCliente(); 

    // Prendiamo l'ultimo cliente appena aggiunto alla lista della città
    Cliente clienteAttuale = start.Clienti.Last();

    // 2. Controllo: Ha deciso di non comprare nulla durante la generazione?
    if (clienteAttuale.ProdottoDesiderato == null)
    {
        Console.WriteLine("\n\"Buongiorno mercante! Oggi sto solo dando un'occhiata in giro, arrivederci!\"");
        Console.WriteLine("(Il cliente esce senza comprare. Premi un tasto per il prossimo...)");
        Console.ReadKey();
        continue; // Salta direttamente al prossimo ciclo del 'for'
    }

    // Estraiamo i dati dal Tuple (ora che siamo sicuri che esiste)
    Prodotto prodottoScelto = clienteAttuale.ProdottoDesiderato.Value.Item1;
    int quantita = clienteAttuale.ProdottoDesiderato.Value.Item3;
    float prezzoDiMercatoLocale = clienteAttuale.ProdottoDesiderato.Value.Item2 * quantita;

    // 4. Avvio della trattativa vera e propria
    Console.WriteLine($"\n\"Vorrei acquistare {quantita} unità di {prodottoScelto.Nome}.\"");
    Console.WriteLine($"[INFO MERCATO] Prezzo calcolato dalla città: {prezzoDiMercatoLocale} monete.");
    
    // 5. Richiesta prezzo al giocatore
    Console.Write("\nA quale prezzo iniziale decidi di proporlo? ");
    if (!float.TryParse(Console.ReadLine(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out float prezzoProposto))
    {
        prezzoProposto = prezzoDiMercatoLocale; // Fallback di sicurezza
    }

    // 6. Parte il tuo metodo di contrattazione
    clienteAttuale.Acquista(prezzoProposto);

    // 7. Resoconto a fine turno prima del prossimo cliente
    Console.WriteLine("\n-----------------------------------------------------------------");
    Console.WriteLine($"STATO AGGIORNATO -> Nuova Stima Città: {start.Stima:F2}/5.00");
    Console.WriteLine("-----------------------------------------------------------------");
    
    Console.WriteLine("Premi un tasto per far entrare il prossimo cliente in bottega...");
    Console.ReadKey();
}

// --- FINE DELLA GIORNATA ---
Console.WriteLine("\n=================================================================");
Console.WriteLine(" CHIUSURA BOTTEGA - LA GIORNATA È TERMINATA!");
Console.WriteLine($" Stima finale registrata nella città: {start.Stima:F2}");
Console.WriteLine("=================================================================");