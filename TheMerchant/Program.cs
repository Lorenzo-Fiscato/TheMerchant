Prodotto pane = new Prodotto
{
    Nome = "Pane",
    ClasseSociale = TipoClasse.Bassa,
    Stackable = true
};

Prodotto grano = new Prodotto
{
    Nome = "Grano",
    ClasseSociale = TipoClasse.Bassa,
    Stackable = true
};

Prodotto vino = new Prodotto
{
    Nome = "Vino",
    ClasseSociale = TipoClasse.Media,
    Stackable = true
};

Prodotto collana = new Prodotto
{
    Nome = "Collana",
    ClasseSociale = TipoClasse.Alta,
    Stackable = false
};
Citta start = new Citta
{
    Stima = 0,
    Abbondante = new Dictionary<Prodotto, int>
    {
        { pane, 5 },
        { vino, 10 },
        { grano, 2 }
    },
    Carente = new Dictionary<Prodotto, int>
    {
        { collana, 100 }
    },
};

Inventario.Prodotti.Add(pane, 10);
Inventario.Prodotti.Add(vino, 5);
Inventario.Prodotti.Add(collana, 1);

Disponibili.Prodotti.Add(pane, (5, 5));
Disponibili.Prodotti.Add(vino, (10, 11));
Disponibili.Prodotti.Add(grano, (50, 3));
Disponibili.Prodotti.Add(collana, (1, 100));
for (int i = 0; i < 20; i++)
{
    start.CreaCliente();
}



