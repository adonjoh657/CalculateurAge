using System.Collections.ObjectModel;
using CalculateurAge.Views;

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
    private string _signeAstrologique = "";
    private readonly ObservableCollection<string> _historique = new();

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

    public string SigneAstrologique
    {
        get => _signeAstrologique;
        set => SetField(ref _signeAstrologique, value);
    }

    // Liste des derniers calculs, du plus récent au plus ancien.
    public ObservableCollection<string> Historique => _historique;

    // Lié à Button.Command dans le XAML.
    public RelayCommand CalculerCommand { get; }

    // Remet tous les champs à zéro.
    public RelayCommand EffacerCommand { get; }

    // Vide la liste des derniers calculs.
    public RelayCommand EffacerHistoriqueCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom));
        EffacerCommand = new RelayCommand(Effacer);
        EffacerHistoriqueCommand = new RelayCommand(
            () => Historique.Clear(),
            () => Historique.Count > 0);
        Historique.CollectionChanged += (_, __) => EffacerHistoriqueCommand.Rafraichir();
    }

    // Remet l'écran à zéro, sans toucher à la vue.
    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        StatutMajorite = "";
        MessageAnniversaire = "";
        SigneAstrologique = "";
        ResultatVisible = false;
    }

    // Donne le signe astrologique d'une date de naissance.
    private static string GetSigneAstrologique(DateTime naissance)
    {
        int jour = naissance.Day;
        int mois = naissance.Month;
        return (mois, jour) switch
        {
            (1, >= 20) or (2, <= 18) => "Verseau",
            (2, >= 19) or (3, <= 20) => "Poissons",
            (3, >= 21) or (4, <= 19) => "Bélier",
            (4, >= 20) or (5, <= 20) => "Taureau",
            (5, >= 21) or (6, <= 20) => "Gémeaux",
            (6, >= 21) or (7, <= 22) => "Cancer",
            (7, >= 23) or (8, <= 22) => "Lion",
            (8, >= 23) or (9, <= 22) => "Vierge",
            (9, >= 23) or (10, <= 22) => "Balance",
            (10, >= 23) or (11, <= 21) => "Scorpion",
            (11, >= 22) or (12, <= 21) => "Sagittaire",
            _ => "Capricorne",
        };
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
    // Calcule tout, garde une trace, puis ouvre la page résultat.
    private async void Calculer()
    {
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date >
            DateTime.Today.AddYears(-age)) age--;

        Resultat = $"{Nom}, vous avez {age} ans";
        StatutMajorite = age >= 22 ? "Majeur" : "Mineur";
        MessageAnniversaire = CalculerMessageAnniversaire(DateNaissance);
        SigneAstrologique = GetSigneAstrologique(DateNaissance);
        Historique.Insert(0, $"{Nom} — {age} ans ({DateNaissance:dd/MM/yyyy})");
        ResultatVisible = true;

        // Ouvre la page résultat depuis le ViewModel (MVVM : rien dans le code-behind).
        await Shell.Current.GoToAsync(
            $"{nameof(ResultatPage)}" +
            $"?nom={Uri.EscapeDataString(Nom)}" +
            $"&age={age}" +
            $"&statut={Uri.EscapeDataString(StatutMajorite)}" +
            $"&anniversaire={Uri.EscapeDataString(MessageAnniversaire)}" +
            $"&signe={Uri.EscapeDataString(SigneAstrologique)}");
    }
}
