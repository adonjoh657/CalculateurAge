using System.Collections.ObjectModel;

namespace CalculateurAge.ViewModels;

// Liste partagée des calculs, gardée en mémoire.
// Les deux pages (calcul et historique) voient la même liste.
public static class HistoriqueStore
{
    public static ObservableCollection<string> Items { get; } = new();
}
