using System.Collections.ObjectModel;

namespace CookWithUs.Application.ViewModels.DataStructures
{
    internal class RecipesGroupVM
    {
        public RecipesGroupVM(string title, IEnumerable<RecipeVM> recipes)
        {
            Title = title;
            Recipes = new ObservableCollection<RecipeVM>(recipes);
        }

        public string Title { get; }

        public ObservableCollection<RecipeVM> Recipes { get; }
    }
}
