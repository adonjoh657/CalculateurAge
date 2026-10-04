namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    // Contient l'ÉTAT de l'écran et les ACTIONS possibles.
    // Champs privés : la vraie donnée.
    private string _nom = "";
    private DateTime _dateNaissance =
        DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private bool _resultatVisible;
    private string _statutMajorite = "";
    private string _messageAnniversaire = "";

    // Propriétés publiques : ce que le XAML voit.
    public string Nom
    {
        get => _nom;
        set
        {
            if (SetField(ref _nom, value))
                CalculerCommand.Rafraichir();
        }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set => SetField(ref _dateNaissance, value);
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    public string StatutMajorite
    {
        get => _statutMajorite;
        set => SetField(ref _statutMajorite, value);
    }

    public string MessageAnniversaire
    {
        get => _messageAnniversaire;
        set => SetField(ref _messageAnniversaire, value);
    }

    // Lié à Button.Command dans le XAML.
    public RelayCommand CalculerCommand { get; }

    // Remet tous les champs à zéro.
    public RelayCommand EffacerCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom));
        EffacerCommand = new RelayCommand(Effacer);
    }

    // Remet l'écran à zéro, sans toucher à la vue.
    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        StatutMajorite = "";
        MessageAnniversaire = "";
        ResultatVisible = false;
    }

    // Jours restants avant le prochain anniversaire.
    private static string CalculerMessageAnniversaire(DateTime naissance)
    {
        DateTime aujourdHui = DateTime.Today;
        int annee = aujourdHui.Year;

        // Cas du 29 février : on fête le 28 février les années non bissextiles.
        int jour = naissance.Day;
        int mois = naissance.Month;
        if (mois == 2 && jour == 29 && !DateTime.IsLeapYear(annee))
            jour = 28;

        DateTime prochain = new DateTime(annee, mois, jour);
        if (prochain.Date < aujourdHui.Date)
        {
            annee++;
            jour = naissance.Day;
            if (mois == 2 && jour == 29 && !DateTime.IsLeapYear(annee))
                jour = 28;
            prochain = new DateTime(annee, mois, jour);
        }

        int jours = (prochain.Date - aujourdHui.Date).Days;
        return jours == 0
            ? "Joyeux anniversaire !"
            : $"Anniversaire dans {jours} jour{(jours > 1 ? "s" : "")}";
    }

    // La logique métier : aucun contrôle d'interface ici.
    private void Calculer()
    {
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date >
            DateTime.Today.AddYears(-age)) age--;

        Resultat = $"{Nom}, vous avez {age} ans";
        StatutMajorite = age >= 18 ? "Majeur" : "Mineur";
        MessageAnniversaire = CalculerMessageAnniversaire(DateNaissance);
        ResultatVisible = true;
    }
}
