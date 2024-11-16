using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookWithUs.Application.Database;
using CookWithUs.Application.ViewModels.DataStructures;
using CookWithUs.Application.Windows;
using System.Collections.ObjectModel;

namespace CookWithUs.Application.ViewModels
{
    internal partial class MainWindowVM : ObservableObject
    {

        private readonly List<RecipeVM> _allResipes;

        public MainWindowVM()
        {
            _allResipes = new List<RecipeVM>(RecipesReader.GetAllRecipes().Select(r => new RecipeVM(r)));

            Recipes = new ObservableCollection<RecipeVM>();
            SearchKeywords = "сало";
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

        public ObservableCollection<RecipeVM> Recipes { get; }

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
    }
}
