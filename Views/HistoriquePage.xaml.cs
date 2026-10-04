using CalculateurAge.ViewModels;

namespace CalculateurAge.Views;

public partial class HistoriquePage : ContentPage
{
    public HistoriquePage()
    {
        InitializeComponent();
        // Objet dans lequel tous les {Binding} de la page
        // vont chercher leurs valeurs.
        BindingContext = new HistoriqueViewModel();
    }
}
