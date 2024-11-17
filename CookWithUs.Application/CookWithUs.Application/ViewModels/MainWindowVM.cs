using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookWithUs.Application.ViewModels.DataStructures;
using CookWithUs.Application.Windows;
using System.Windows.Controls;

namespace CookWithUs.Application.ViewModels
{
    internal partial class MainWindowVM : ObservableObject
    {
        private readonly MainControl _mainControl;
        private readonly Stack<UserControl> _displayedControls;

        public MainWindowVM()
        {
            _mainControl = new MainControl()
            {
                DataContext = new MainControlVM(this)
            };

            _displayedControls = new Stack<UserControl>();
            _displayedControls.Push(_mainControl);
            Refresh();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BackCommand))]
        private UserControl _displayedControl;

        [ObservableProperty]
        private bool _isBackEnabled;

        [RelayCommand()]
        private void Back()
        {
            _displayedControls.Pop();
            Refresh();
        }

        public void DisplayAll(IReadOnlyCollection<RecipesGroupVM> recipes)
        {
            _displayedControls.Push(new AllRecipesControl
            {
                DataContext = recipes,
            });
            Refresh();
        }

        public void DisplayStar(IReadOnlyCollection<RecipesGroupVM> recipes)
        {
            _displayedControls.Push(new AllRecipesControl
            {
                DataContext = recipes,
            });
            Refresh();
        }

        public void OpenDetails(RecipeVM recipeVM)
        {
            _displayedControls.Push(new DetailedControl
            {
                DataContext = recipeVM,
            });
            Refresh();
        }

        private void Refresh()
        {
            DisplayedControl = _displayedControls.Peek();
            IsBackEnabled = _displayedControls.Count > 1;
        }
    }
}
