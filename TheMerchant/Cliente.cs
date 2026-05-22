using System.Globalization;

public class Cliente
{
    public Citta cittaAppartenenza;
    public bool Sesso { get; set; }
    public TipoClasse ClasseSociale { get; set; }
    public float Pazienza { get; set; }
    public (Prodotto Desiderato, float PrezzoMedio, int Quantita)? ProdottoDesiderato { get; set; }
    public PersonalitaCliente Personalita { get; set; }
    private Random rand = new Random();
    private int nOfferte = 0;

    public void Acquista(float prezzoVendita)
    {
        nOfferte = 0;
        if (!ProdottoDesiderato.HasValue) return;

        float prezzoMedio = ProdottoDesiderato.Value.PrezzoMedio;
        float tolleranza  = ProdottoDesiderato.Value.Desiderato.TolleranzaPrezzo;
        int quantita = ProdottoDesiderato.Value.Quantita;

        // Tutti i confronti e la trattativa lavorano su prezzi totali
        float prezzoTotaleAtteso = prezzoMedio * quantita;
        float prezzoTotaleMax = prezzoMedio * tolleranza * quantita;
        float prezzoTotaleMin = prezzoMedio * 0.5f * quantita;

        // Se il prezzo è oltre la tolleranza massima assoluta, non tratta nemmeno
        if (prezzoVendita > prezzoTotaleMax)
        {
            Console.WriteLine(prezzoVendita - prezzoTotaleMax);
            Console.WriteLine("Prezzo decisamente troppo alto! Il cliente se ne va sdegnato.");
            Pazienza = 0.0f;
            // Il cliente voleva pagare prezzoTotaleAtteso, tu hai chiesto prezzoVendita
            ModificaStima(prezzoVendita, prezzoTotaleAtteso);
            return;
        }

        if (prezzoVendita < prezzoTotaleMin)
        {
            Console.WriteLine("Hai proposto un prezzo troppo basso! Il cliente pensa che tu stia cercando di fregarlo e se ne va.");
            cittaAppartenenza.Stima -= 0.01f; // stima scende leggermente
            return;
        }

        // Il cliente valuta il prezzo esposto
        if (Contratta(prezzoVendita, prezzoTotaleAtteso))
        {
            // Il prezzo non gli sta bene, ma è in una fascia accettabile: inizia la trattativa
            Console.WriteLine("Il cliente trova il prezzo alto, ma è disposto a trattare...");
            ModificaPazienza(prezzoVendita, prezzoTotaleAtteso);
            ProponiOfferta(prezzoVendita, prezzoTotaleAtteso);
        }
        else
        {
            // Il cliente accetta direttamente il tuo prezzo senza battere ciglio
            Console.WriteLine($"Acquisto effettuato direttamente a {prezzoVendita}!");
            ModificaSoldi(prezzoVendita);
        }
    }

    private void ModificaStima(float prezzoVendita, float prezzoCliente, bool forcePositive = false)
    {
        // rapporto tra il prezzo chiesto e quello del cliente
        float rapportoSopraprezzo = prezzoVendita / prezzoCliente;

        float variazioneStima = rapportoSopraprezzo <= 1.0f || forcePositive
             // Hai venduto a prezzo giusto o sotto il prezzo del cliente: la stima sale
            ? 0.01f
            // Hai venduto a prezzo maggiorato: la stima scende
            : -((rapportoSopraprezzo - 1.0f) / 10);

        cittaAppartenenza.Stima += (float)Math.Round(variazioneStima, 2);
        cittaAppartenenza.Stima  = Math.Clamp(cittaAppartenenza.Stima, 0.1f, 2.5f);

        Console.WriteLine($"La stima della città è ora: {cittaAppartenenza.Stima:F2}");
    }

    // prezzoTotaleAtteso è passato esplicitamente per evitare il mismatch unitario/totale
    private void ModificaPazienza(float prezzoVendita, float prezzoTotaleAtteso)
    {
        // La pazienza diminuisce in base a quanto il prezzo supera il totale atteso.
        // Più offerte si fanno, più la penalità aumenta (+ nOfferte / 10)
        float penalita = (prezzoVendita / prezzoTotaleAtteso) / 10f + (nOfferte / 10f);
        Pazienza -= (float)Math.Round(penalita, 2);
        if (Pazienza < 0) Pazienza = 0.0f;
        Console.WriteLine($"La pazienza del cliente è ora: {Pazienza}");
    }

    private bool Contratta(float prezzoVendita, float prezzoCliente)
    {
        // return false = accetta subito.
        if (prezzoVendita <= prezzoCliente) return false;

        double rapportoSopraPrezzo = prezzoVendita / prezzoCliente;
        // Più la pazienza è alta e il prezzo è vicino al prezzo del cliente,
        // più è probabile che il cliente voglia trattare (return true).
        double sogliaAccettazione = rapportoSopraPrezzo * Math.Sqrt(Pazienza);

        return rand.NextDouble() * 1.2 < sogliaAccettazione;
    }

    private void ProponiOfferta(float prezzoVendita, float prezzoTotaleAtteso)
    {
        float prezzoAttuale = prezzoVendita;
        // Il cliente formula una prima offerta più vicina al prezzo esposto, influenzata da pazienza, stima e personalità
        double fattoreAvvicinamento = Math.Clamp(
            (Pazienza * cittaAppartenenza.Stima * ModificatoriPersonalita.ModificatorePrezzoOfferta[Personalita])
            + rand.NextDouble() * 0.25,
            0.5, 0.7);
        float prezzoCliente = (float)Math.Round(prezzoVendita * fattoreAvvicinamento, 2);

        while (Pazienza > 0)
        {
            nOfferte++;

            Console.WriteLine($"\n[Turno {nOfferte}] Il cliente offre: {prezzoCliente} (Pazienza: {Pazienza})");
            Console.WriteLine("Scegli come rispondere:");
            Console.WriteLine("[0] Accetta l'offerta del cliente");
            Console.WriteLine("[1] Rifiuta e chiudi la trattativa");
            Console.WriteLine("[2] Proponi una controofferta");

            int opzioni = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            if (opzioni == 0)
            {
                Console.WriteLine($"Affare fatto! Venduto a {prezzoCliente}.");
                // Hai accettato esattamente il prezzo del cliente: rapporto = 1, stima sale
                ModificaStima(prezzoCliente, prezzoCliente);
                ModificaSoldi(prezzoCliente);
                return;
            }

            if (opzioni == 1)
            {
                Console.WriteLine("Trattativa interrotta. Il cliente se ne va.");
                Pazienza = 0.0f;
                // prezzoAttuale è l'ultimo prezzo che tu hai proposto
                ModificaStima(prezzoAttuale, prezzoCliente);
                return;
            }

            // Controofferta del mercante
            Console.Write("Inserisci il tuo nuovo prezzo: ");
            prezzoAttuale = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            if (prezzoAttuale <= prezzoCliente)
            {
                Console.WriteLine($"Hai offerto meno di quanto richiesto! Il cliente accetta felice a {prezzoAttuale}.");
                // Tu hai ceduto sotto il prezzo del cliente: rapporto <= 1, stima sale
                ModificaStima(prezzoAttuale, prezzoCliente);
                ModificaSoldi(prezzoAttuale);
                return;
            }

            if (prezzoCliente * 1.2f > prezzoAttuale)
            {
                Console.WriteLine("Hai offerto un prezzo vicino al cliente! Il cliente apprezza la tua flessibilità e accetta.");
                ModificaStima(prezzoCliente, prezzoAttuale);
                ModificaSoldi(prezzoAttuale);
                return;
            }

            if (prezzoAttuale > prezzoVendita)
            {
                Console.WriteLine("Hai alzato il prezzo rispetto al turno precedente! Il cliente si sente preso in giro e se ne va.");
                Pazienza = 0.0f;
                ModificaStima(prezzoAttuale, prezzoCliente);
                return;
            }

            // Il cliente valuta la tua controofferta
            if (!Contratta(prezzoAttuale, prezzoCliente))
            {
                Console.WriteLine($"Il cliente ha accettato la tua controofferta di {prezzoAttuale}!");
                // forziamo il positivo perché la formula produce un risultato negativo perché il rapporto è > 1
                ModificaStima(prezzoAttuale, prezzoCliente, forcePositive: true);
                ModificaSoldi(prezzoAttuale);
                return;
            }

            Console.WriteLine("Il cliente rifiuta la tua controofferta...");
            ModificaPazienza(prezzoAttuale, prezzoTotaleAtteso);

            if (Pazienza <= 0.1f)
            {
                Console.WriteLine("Il cliente ha perso la pazienza ed è uscito dal negozio!");
                ModificaStima(prezzoAttuale, prezzoCliente);
                return;
            }

            // Il prezzo attuale diventa il nuovo tetto massimo per il prossimo calcolo
            prezzoVendita = prezzoAttuale;
            // Il cliente formula una nuova offerta più vicina al prezzo attuale, influenzata da pazienza, stima e personalità
            fattoreAvvicinamento = Math.Clamp(
                (Pazienza * cittaAppartenenza.Stima * ModificatoriPersonalita.ModificatorePrezzoOfferta[Personalita])
                + rand.NextDouble() * 0.25,
                0.3, 0.6);
            // Il cliente non può fare salti bruschi: avanza al massimo del 25% del suo prezzo attuale per turno
            float passoMassimo = prezzoCliente * 0.25f;
            float avanzamento  = Math.Min(prezzoVendita - prezzoCliente, passoMassimo);
            prezzoCliente      = (float)Math.Round(prezzoCliente + avanzamento * fattoreAvvicinamento, 2);
        }
    }

    private float ModificaSoldi(float prezzoVendita) => Mercante.Monete += prezzoVendita;
}