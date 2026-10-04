using System.Collections.ObjectModel;

namespace CalculateurAge.ViewModels;

// Données et actions de la page historique.
// Aucun contrôle d'interface ici, que du binding.
public class HistoriqueViewModel : BaseViewModel
{
    // La même liste que celle remplie par la page de calcul.
    public ObservableCollection<string> Items => HistoriqueStore.Items;

    // Vide la liste. Le bouton se grise quand la liste est vide.
    public RelayCommand EffacerHistoriqueCommand { get; }

    // Revient à la page précédente.
    public RelayCommand RetourCommand { get; }

    public HistoriqueViewModel()
    {
        EffacerHistoriqueCommand = new RelayCommand(
            () => HistoriqueStore.Items.Clear(),
            () => HistoriqueStore.Items.Count > 0);
        RetourCommand = new RelayCommand(
            async () => await Shell.Current.GoToAsync(".."));

        // Repose la question "peut-on effacer ?" à chaque ajout/suppression.
        HistoriqueStore.Items.CollectionChanged += (_, __) =>
            EffacerHistoriqueCommand.Rafraichir();
    }
}
