using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookWithUs.Application.Database;
using CookWithUs.Application.ViewModels.DataStructures;
using CookWithUs.Application.Windows;
using System.Collections.ObjectModel;

namespace CookWithUs.Application.ViewModels
{
    internal partial class MainWindowVM : ObservableRecipient
    {

        private readonly List<RecipeVM> _allResipes;

        public MainWindowVM()
        {
            _allResipes = new List<RecipeVM>(RecipesReader.GetAllRecipes().Select(r => new RecipeVM(RequestDisplayStarChanged, r)));

            Recipes = new ObservableCollection<RecipeVM>();
            SearchKeywords = "цибуля";
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
        private string _searchKeywords;

        partial void OnSearchKeywordsChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Recipes.Clear();
            }
        }

        [ObservableProperty]
        private ObservableCollection<RecipeVM> _recipes;

        [RelayCommand(CanExecute = nameof(IsSearchEnabled))]
        private void Search()
        {
            Recipes.Clear();

            foreach (RecipeVM recipe in _allResipes)
            {
                if (recipe.CanBeDisplayed(SearchKeywords))
                {
                    Recipes.Add(recipe);
                }
            }
        }

        private bool IsSearchEnabled()
        {
            return SearchKeywords?.Length > 0;
        }

        [RelayCommand]
        private void DisplayAll()
        {
            IReadOnlyCollection<RecipesGroupVM> enumerable = _allResipes.GroupBy(r => r.FoodRecipe.Type).
                Select(g => new RecipesGroupVM(g.Key, g)).ToArray();

            new AllRecipesWindow
            {
                DataContext = enumerable,
            }.ShowDialog();
        }

        [RelayCommand(CanExecute = nameof(IsDisplayStarEnabled))]
        private void DisplayStarRecipes()
        {
            try
            {
                _allResipes.ForEach(r => r.CanStarBeChanged = false);
                IReadOnlyCollection<RecipesGroupVM> enumerable = _allResipes.Where(r => r.IsStar).GroupBy(r => r.FoodRecipe.Type).
                    Select(g => new RecipesGroupVM(g.Key, g)).ToArray();

                new AllRecipesWindow
                {
                    DataContext = enumerable,
                }.ShowDialog();
            }
            finally
            {
                _allResipes.ForEach(r => r.CanStarBeChanged = true);
            }
        }

        private bool IsDisplayStarEnabled()
        {
            return _allResipes.Any(r => r.IsStar);
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DisplayStarRecipesCommand))]
        private bool _requestStar;

        private void RequestDisplayStarChanged()
        {
            RequestStar = !RequestStar;
        }
    }
}
