namespace CalculateurAge.Views;

// Relie les paramètres de l'URL aux propriétés.
// Remplies par la navigation, APRÈS le constructeur.
[QueryProperty(nameof(Nom), "nom")]
[QueryProperty(nameof(Age), "age")]
[QueryProperty(nameof(Statut), "statut")]
[QueryProperty(nameof(Anniversaire), "anniversaire")]
[QueryProperty(nameof(Signe), "signe")]
public partial class ResultatPage : ContentPage
{
    public string Nom { get; set; } = string.Empty;
    public string Age { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public string Anniversaire { get; set; } = string.Empty;
    public string Signe { get; set; } = string.Empty;

    // Construit l'arbre visuel décrit par le XAML.
    public ResultatPage() => InitializeComponent();

    // Appelée à CHAQUE affichage de la page.
    protected override void OnAppearing()
    {
        base.OnAppearing();
        lblMessage.Text = $"{Nom}, vous avez {Age} ans";
        lblStatut.Text = $"Statut : {Statut}";
        lblAnniversaire.Text = Anniversaire;
        lblSigne.Text = $"Signe : {Signe}";
    }

    // ".." = revenir à la page précédente.
    private async void OnRetourClicked(object? s, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}
