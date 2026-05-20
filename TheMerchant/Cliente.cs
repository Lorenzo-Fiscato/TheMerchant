using System.Globalization;

public class Cliente
{
    public Citta cittaAppartenenza;
    public bool Sesso { get; set; }
    public TipoClasse ClasseSociale { get; set; }
    public float Pazienza { get; set; }
    public (Prodotto Desiderato, float PrezzoMedio)? ProdottoDesiderato { get; set; }
    public PersonalitaCliente Personalita { get; set; }
    private Random rand = new Random();
    private int nOfferte = 0;

    public void Acquista(float prezzoVendita)
    {
        if (!ProdottoDesiderato.HasValue) return;

        float prezzoMedio = ProdottoDesiderato.Value.PrezzoMedio;
        float tolleranza = ProdottoDesiderato.Value.Desiderato.TolleranzaPrezzo;

        // Se il prezzo è oltre la tolleranza massima assoluta, non tratta nemmeno
        if (prezzoVendita > prezzoMedio * tolleranza)
        {
            Console.WriteLine("Prezzo decisamente troppo alto! Il cliente se ne va sdegnato.");
            Pazienza = 0;
            ModificaStima(prezzoVendita, prezzoMedio);
            return;
        }

        // Il cliente valuta il prezzo esposto
        if (Contratta(prezzoVendita, prezzoMedio))
        {
            // Il cliente accetta direttamente il tuo prezzo senza battere ciglio
            Console.WriteLine($"Acquisto effettuato direttamente a {prezzoVendita}!");
            ModificaStima(prezzoVendita, prezzoMedio);
            ModificaSoldi(prezzoVendita);
        }
        else
        {
            // Il prezzo non gli sta bene, ma è in una fascia accettabile: inizia la trattativa
            Console.WriteLine("Il cliente trova il prezzo alto, ma è disposto a trattare...");
            ModificaPazienza(prezzoVendita);
            ProponiOfferta(prezzoVendita, prezzoMedio);
        }
    }

private void ModificaStima(float prezzoVendita, float prezzoCliente)
{
    // Calcoliamo il rapporto di profitto/sopraprezzo esagerato.
    float rapportoSopraprezzo = prezzoVendita / prezzoCliente;

    // Se hai venduto a meno o uguale a quanto voleva il cliente, la città ti adora
    // Altrimenti, più il rapporto è alto, più la stima scende.
    float variazioneStima;

    if (rapportoSopraprezzo <= 1.0f)
    {
        // Ottimo affare per il cliente: la stima sale
        // Più la pazienza era alta, più il cliente parlerà bene di te in giro.
        variazioneStima = 0.01f + (Pazienza / 50f);
    }
    else
    {
        // Hai venduto a prezzo maggiorato: la stima scende
        // Sottraiamo 1 per isolare solo il surplus
        // La pazienza rimasta attenua il colpo: se il cliente era felice (alta pazienza), si arrabbierà meno
        variazioneStima = -((rapportoSopraprezzo - 1.0f) / 10) + (Pazienza / 100f);
    }

    // Applichiamo la variazione alla città arrotondando a 2 decimali
    cittaAppartenenza.Stima += (float)Math.Round(variazioneStima, 2);
    
    // Evitiamo che la stima vada sotto zero o diventi infinita (es. range tra 0.0 e 2.5)
    cittaAppartenenza.Stima = Math.Clamp(cittaAppartenenza.Stima, 0.0f, 2.5f);

    Console.WriteLine($"[REP] La stima della città è ora: {cittaAppartenenza.Stima:F2}");
}

    private void ModificaPazienza(float prezzoVendita)
    {
        // La pazienza diminuisce in base a quanto il prezzo supera il prezzo medio. 
        // Più offerte si fanno, più la penalità aumenta (+ nOfferte / 10)
        float penalita = (prezzoVendita / ProdottoDesiderato.Value.PrezzoMedio) / 10f + (nOfferte / 10f);
        Pazienza -= (float)Math.Round(penalita, 2);
        Console.WriteLine($"La pazienza del cliente è ora: {Pazienza}");
    }

    private bool Contratta(float prezzoVendita, float prezzoCliente)
    {
        if (prezzoVendita <= prezzoCliente) return true; 

        double rapportoSopraPrezzo = prezzoVendita / prezzoCliente;
        // Più la pazienza è alta e il prezzo è vicino al prezzo del cliente, più è facile che questa funzione restituisca true
        double sogliaAccettazione = 0.7 / rapportoSopraPrezzo * Pazienza;

        return rand.NextDouble() * 3 < sogliaAccettazione;
    }

    private void ProponiOfferta(float prezzoVendita, float prezzoCliente)
    {
        float prezzoAttualeCliente = prezzoCliente;
        double fattoreAvvicinamento = Math.Clamp((Pazienza * cittaAppartenenza.Stima *
            ModificatoriPersonalita.ModificatorePrezzoOfferta[Personalita]) + rand.NextDouble() * 0.25, 0.5, 0.8);
        prezzoCliente = (float)Math.Round(prezzoVendita * fattoreAvvicinamento, 2);
        float prezzoAttuale = prezzoVendita;

        while (Pazienza > 0)
        {
            nOfferte++;
            
            // Il cliente formula l'offerta basandosi sulla sua pazienza attuale

            Console.WriteLine($"\n[Turno {nOfferte}] Il cliente offre: {prezzoCliente} (Pazienza: {Pazienza})");
            Console.WriteLine("Scegli come rispondere:");
            Console.WriteLine("[0] Accetta l'offerta del cliente");
            Console.WriteLine("[1] Rifiuta e chiudi la trattativa");
            Console.WriteLine("[2] Proponi una controofferta");
            
            int opzioni = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            if (opzioni == 0)
            {
                Console.WriteLine($"Affare fatto! Venduto a {prezzoCliente}.");
                ModificaStima(prezzoCliente, ProdottoDesiderato.Value.PrezzoMedio);
                ModificaSoldi(prezzoCliente);
                return;
            }
            if (opzioni == 1)
            {
                Console.WriteLine("Trattativa interrotta. Il cliente se ne va.");
                Pazienza = 0;
                ModificaStima(prezzoAttuale, prezzoAttualeCliente);
                return;
            }

            // Opzione 2: Controofferta del mercante
            Console.Write("Inserisci il tuo nuovo prezzo: ");
            prezzoAttuale = float.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

            // Verifiche sulla controofferta inserita
            if (prezzoAttuale <= prezzoCliente)
            {
                Console.WriteLine($"Hai offerto meno di quanto richiesto! Il cliente accetta felice a {prezzoAttuale}.");
                ModificaStima(prezzoAttuale, prezzoCliente);
                ModificaSoldi(prezzoAttuale);
                return;
            }

            if (prezzoAttuale > prezzoVendita)
            {
                Console.WriteLine("Hai alzato il prezzo rispetto al turno precedente! Il cliente si sente preso in giro e se ne va.");
                Pazienza = 0;
                ModificaStima(prezzoAttuale, prezzoCliente);
                return;
            }

            // Il cliente valuta la tua controofferta
            if (Contratta(prezzoAttuale, prezzoCliente))
            {
                Console.WriteLine($"Il cliente ha accettato la tua controofferta di {prezzoAttuale}!");
                ModificaStima(prezzoAttuale, prezzoCliente);
                ModificaSoldi(prezzoAttuale);
                return;
            }

            // Se non l'ha accettata, perde pazienza per il prossimo turno
            Console.WriteLine("Il cliente rifiuta la tua controofferta...");
            ModificaPazienza(prezzoAttuale);

            if (Pazienza <= 0)
            {
                Console.WriteLine("Il cliente ha perso la pazienza ed è uscito dal negozio!");
                ModificaStima(prezzoAttuale, prezzoCliente);
                return;
            }

            // Il prezzo attuale diventa il nuovo tetto massimo per il prossimo calcolo
            prezzoVendita = prezzoAttuale;
            fattoreAvvicinamento = Math.Clamp((Pazienza * cittaAppartenenza.Stima *
                ModificatoriPersonalita.ModificatorePrezzoOfferta[Personalita]) + rand.NextDouble() * 0.25, 0.3, 0.6);
            prezzoAttualeCliente = prezzoCliente = (float)Math.Round(prezzoCliente + (prezzoAttuale - prezzoCliente) * fattoreAvvicinamento, 2);
        }
    }

    private float ModificaSoldi(float prezzoVendita) => Mercante.Monete += prezzoVendita;
}