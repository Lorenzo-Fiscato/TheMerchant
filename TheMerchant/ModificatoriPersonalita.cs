public static class ModificatoriPersonalita
{
    public static Dictionary<PersonalitaCliente, float> ModificatorePazienza = new Dictionary<PersonalitaCliente, float>
    {
        { PersonalitaCliente.Normale, 1f },
        { PersonalitaCliente.Frettoloso, 0.8f },
        { PersonalitaCliente.Trattatore, 1.2f }
    };

    public static Dictionary<PersonalitaCliente, float> ModificatorePrezzoOfferta = new Dictionary<PersonalitaCliente, float>
    {
        { PersonalitaCliente.Normale, 1f },
        { PersonalitaCliente.Frettoloso, 1.2f },
        { PersonalitaCliente.Trattatore, 0.9f }
    };
}