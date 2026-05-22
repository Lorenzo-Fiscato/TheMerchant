public static class ConfigurazioneEventi
{
    // Mappa ogni evento al suo moltiplicatore dei prezzi locali
    public static Dictionary<Evento, Dictionary<TagProdotto, float>> ModificatorePrezzo { get; } = new Dictionary<Evento, Dictionary<TagProdotto, float>>()
    {
        {Evento.Nessuno, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 1.0f },
                { TagProdotto.CiboPovero, 1.0f },
                { TagProdotto.CiboRicco, 1.0f },
                { TagProdotto.Materiale, 1.0f },
                { TagProdotto.Armamento, 1.0f },
                { TagProdotto.Lusso, 1.0f }
            })
        },
        { Evento.Festa, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 0.9f },
                { TagProdotto.CiboPovero, 0.8f },
                { TagProdotto.CiboRicco, 0.95f },
                { TagProdotto.Materiale, 1.0f },
                { TagProdotto.Armamento, 1.0f },
                { TagProdotto.Lusso, 1.2f }
            })
        },
        { Evento.Carestia, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 1.5f },
                { TagProdotto.CiboPovero, 1.4f },
                { TagProdotto.CiboRicco, 1.3f },
                { TagProdotto.Materiale, 1.0f },
                { TagProdotto.Armamento, 1.0f },
                { TagProdotto.Lusso, 0.9f }
            })
        },
        { Evento.Epidemia, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 1.2f },
                { TagProdotto.CiboPovero, 1.1f },
                { TagProdotto.CiboRicco, 1.0f },
                { TagProdotto.Materiale, 0.9f },
                { TagProdotto.Armamento, 0.9f },
                { TagProdotto.Lusso, 0.8f }
            })
        },
        { Evento.Guerra, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 1.3f },
                { TagProdotto.CiboPovero, 1.2f },
                { TagProdotto.CiboRicco, 1.1f },
                { TagProdotto.Materiale, 1.5f },
                { TagProdotto.Armamento, 2.0f },
                { TagProdotto.Lusso, 0.8f }
            })  
        }
    };

    public static Dictionary<Evento, float> ModificatorePazienza { get; } = new Dictionary<Evento, float>()
    {
        {Evento.Nessuno, 1.0f },
        { Evento.Festa, 1.2f },
        { Evento.Carestia, 0.8f },
        { Evento.Epidemia, 0.9f },
        { Evento.Guerra, 0.7f }
    };

        public static Dictionary<Evento, Dictionary<TagProdotto, float>> ModificatoreProbComprare { get; } = new Dictionary<Evento, Dictionary<TagProdotto, float>>()
    {
        {Evento.Nessuno, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 1.0f },
                { TagProdotto.CiboPovero, 1.0f },
                { TagProdotto.CiboRicco, 1.0f },
                { TagProdotto.Materiale, 1.0f },
                { TagProdotto.Armamento, 1.0f },
                { TagProdotto.Lusso, 1.0f }
            })
        },
        { Evento.Festa, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 1.0f },
                { TagProdotto.CiboPovero, 1.0f },
                { TagProdotto.CiboRicco, 1.2f },
                { TagProdotto.Materiale, 1.0f },
                { TagProdotto.Armamento, 1.0f },
                { TagProdotto.Lusso, 1.2f }
            })
        },
        { Evento.Carestia, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 1.5f },
                { TagProdotto.CiboPovero, 1.5f },
                { TagProdotto.CiboRicco, 1.2f },
                { TagProdotto.Materiale, 1.0f },
                { TagProdotto.Armamento, 0.8f },
                { TagProdotto.Lusso, 0.8f }
            })
        },
        { Evento.Epidemia, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 1.3f },
                { TagProdotto.CiboPovero, 1.2f },
                { TagProdotto.CiboRicco, 1.0f },
                { TagProdotto.Materiale, 1.0f },
                { TagProdotto.Armamento, 0.7f },
                { TagProdotto.Lusso, 0.8f }
            })
        },
        { Evento.Guerra, (new Dictionary<TagProdotto, float>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 1.3f },
                { TagProdotto.CiboPovero, 1.2f },
                { TagProdotto.CiboRicco, 1.1f },
                { TagProdotto.Materiale, 1.2f },
                { TagProdotto.Armamento, 2.0f },
                { TagProdotto.Lusso, 0.8f }
            })  
        }
    };

    public static Dictionary<Evento, Dictionary<TagProdotto, int>> ModificatoreQuantita { get; } = new Dictionary<Evento, Dictionary<TagProdotto, int>>()
    {
        {Evento.Nessuno, (new Dictionary<TagProdotto, int>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 20 },
                { TagProdotto.CiboPovero, 10 },
                { TagProdotto.CiboRicco, 5 },
                { TagProdotto.Materiale, 20 },
                { TagProdotto.Armamento, 2 },
                { TagProdotto.Lusso, 2 }
            })
        },
        { Evento.Festa, (new Dictionary<TagProdotto, int>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 15 },
                { TagProdotto.CiboPovero, 8 },
                { TagProdotto.CiboRicco, 7 },
                { TagProdotto.Materiale, 20 },
                { TagProdotto.Armamento, 1 },
                { TagProdotto.Lusso, 4 }
            })
        },
        { Evento.Carestia, (new Dictionary<TagProdotto, int>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 25 },
                { TagProdotto.CiboPovero, 15 },
                { TagProdotto.CiboRicco, 6 },
                { TagProdotto.Materiale, 20 },
                { TagProdotto.Armamento, 1 },
                { TagProdotto.Lusso, 1 }
            })
        },
        { Evento.Epidemia, (new Dictionary<TagProdotto, int>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 15 },
                { TagProdotto.CiboPovero, 8 },
                { TagProdotto.CiboRicco, 4 },
                { TagProdotto.Materiale, 20 },
                { TagProdotto.Armamento, 2 },
                { TagProdotto.Lusso, 1 }
            })
        },
        { Evento.Guerra, (new Dictionary<TagProdotto, int>()
            {
                { TagProdotto.BeneDiPrimaNecessita, 25 },
                { TagProdotto.CiboPovero, 15 },
                { TagProdotto.CiboRicco, 6 },
                { TagProdotto.Materiale, 20 },
                { TagProdotto.Armamento, 5 },
                { TagProdotto.Lusso, 1 }
            })
        }
    };
}
