using CookWithUs.Application.Entities;
using System.Data.SQLite;
using System.IO;
using System.Windows.Media.Imaging;

namespace CookWithUs.Application.Database
{
    internal static class RecipesReader
    {
        private static string PathToDatabaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "Content", "CookWithUs.db");

        public static List<FoodRecipe> GetAllRecipes()
        {
            var recipes = new List<FoodRecipe>();

            using (var connection = new SQLiteConnection($"Data Source={PathToDatabaseFile}"))
            {
                connection.Open();
                string sql = "SELECT * FROM FoodRecipes";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ingredients = reader.GetString(reader.GetOrdinal("Ingredients"));
                            var x = reader.GetFieldValue<byte[]>(reader.GetOrdinal("Image"));
                         


                            //recipes.Add(new FoodRecipe
                            //{
                            //    Tilte = reader.GetString(reader.GetOrdinal("Title")),
                            //    Description = reader.GetString(reader.GetOrdinal("Description")),
                            //    Ingredients = ingredients,
                            //    Image = LoadImageFromBytes(bytBLOB)
                            //});
                        }
                    }
                }
            }

            return recipes;
        }


        private static BitmapImage LoadImageFromBytes(byte[] imageData)
        {
            using (var ms = new MemoryStream(imageData))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
