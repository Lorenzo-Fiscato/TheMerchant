Prodotto pane = new Prodotto
{
    Nome = "Pane",
    ClasseSociale = TipoClasse.Bassa,
    Stackable = true,
    TolleranzaPrezzo = 1.2f,
    Tag = TagProdotto.CiboPovero
};

Prodotto grano = new Prodotto
{
    Nome = "Grano",
    ClasseSociale = TipoClasse.Bassa,
    Stackable = true,
    TolleranzaPrezzo = 1.2f,
    Tag = TagProdotto.BeneDiPrimaNecessita
};

Prodotto vino = new Prodotto
{
    Nome = "Vino",
    ClasseSociale = TipoClasse.Media,
    Stackable = true,
    TolleranzaPrezzo = 1.1f,
    Tag = TagProdotto.CiboRicco
};

Prodotto collana = new Prodotto
{
    Nome = "Collana",
    ClasseSociale = TipoClasse.Alta,
    Stackable = false,
    TolleranzaPrezzo = 1.05f,
    Tag = TagProdotto.Lusso
};

Prodotto farina = new Prodotto
{
    Nome = "Farina",
    ClasseSociale = TipoClasse.Media,
    Stackable = true,
    TolleranzaPrezzo = 1.2f,
    Tag = TagProdotto.BeneDiPrimaNecessita
};

Prodotto legno = new Prodotto
{
    Nome = "Legno",
    ClasseSociale = TipoClasse.Media,
    Stackable = true,
    TolleranzaPrezzo = 1.3f,
    Tag = TagProdotto.Materiale
};
Citta start = new Citta
{
    Stima = 1,
    AbbondanteInCitta = new Dictionary<Prodotto, float>
    {
        { pane, 5 },
        { vino, 10 },
        { grano, 2 }
    },
    Prodotti = new Dictionary<Prodotto, float>
    {
        { collana, 100 },
        { pane, 5 },
        { vino, 11 },
        { grano, 3 },
        { farina, 4 },
        { legno, 6 }
    },
};

Inventario.Prodotti.Add(pane, 10);
Inventario.Prodotti.Add(vino, 5);
Inventario.Prodotti.Add(collana, 1);

Disponibili.Prodotti.Add(pane, (5, 5));
Disponibili.Prodotti.Add(vino, (10, 11));
Disponibili.Prodotti.Add(grano, (50, 3));
Disponibili.Prodotti.Add(collana, (1, 100));
Disponibili.Prodotti.Add(farina, (20, 4));
Disponibili.Prodotti.Add(legno, (15, 6));
for (int i = 0; i < 20; i++)
{
    start.CreaCliente();
}

Console.WriteLine();
start.Stima = 2;

for (int i = 0; i < 20; i++)
{
    start.CreaCliente();
}



