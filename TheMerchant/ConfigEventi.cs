public static class ConfigurazioneEventi
{
    // Mappa ogni evento al suo moltiplicatore dei prezzi locali
    public static Dictionary<Evento, (double Prezzo, double Pazienza, double ProbComprare)> Moltiplicatori =
     new Dictionary<Evento, (double Prezzo, double Pazienza, double ProbComprare)>()
    {
        { Evento.Nessuno, (1.0, 0.0, 1.0) }, // Nessun evento, prezzi normali, pazienza normale, probabilità normale di comprare
        { Evento.Festa, (1.1, 0.2, 1.2) }, // I prezzi aumentano del 10%, i clienti sono più pazienti e più propensi a comprare
        { Evento.Carestia, (2.5, -0.5, 0.5) }, // I prezzi raddoppiano, i clienti sono meno pazienti e meno propensi a comprare
        { Evento.Epidemia, (1.8, -0.8, 0.8) }, // I prezzi aumentano del 80%, i clienti sono molto meno pazienti e meno propensi a comprare
        { Evento.Guerra, (3.0, -0.5, 0.5) } // I prezzi triplicano, i clienti sono meno pazienti e meno propensi a comprare
    };
}