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
for (int i = 0; i < 20; i++)
{
    start.CreaCliente();
}

Console.WriteLine();

Cliente cliente = start.Clienti.Last();
if (cliente.ProdottoDesiderato.HasValue) cliente.Acquista(Disponibili.Prodotti.First(p =>  p.Key
                                                        .Equals(cliente.ProdottoDesiderato.Value.Item1)).Value.Item2);




