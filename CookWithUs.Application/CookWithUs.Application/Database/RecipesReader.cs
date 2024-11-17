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

                            recipes.Add(new FoodRecipe
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Tilte = reader.GetString(reader.GetOrdinal("Title")),
                                Type = reader.GetString(reader.GetOrdinal("Type")),
                                DescriptionSteps = GetDescriptionSteps(reader),
                                Ingredients = GetIngredients(reader),
                                Image = LoadImageFromBytes(reader.GetFieldValue<byte[]>(reader.GetOrdinal("Image"))),
                                IsStar = reader.GetBoolean(reader.GetOrdinal("IsStar"))
                            });
                        }
                    }
                }
            }

            return recipes;
        }

        public static void SetStar(FoodRecipe foodRecipe)
        {
            int isStar = foodRecipe.IsStar ? 1 : 0;
            using (var connection = new SQLiteConnection($"Data Source={PathToDatabaseFile}"))
            {
                connection.Open();
                string sql = $"UPDATE FoodRecipes SET IsStar = @IsStar WHERE Id = @Id";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IsStar", isStar);
                    command.Parameters.AddWithValue("@Id", foodRecipe.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static string[] GetIngredients(SQLiteDataReader reader)
        {
            return reader.GetString(
                reader.GetOrdinal("Ingredients")).
                Split(Environment.NewLine).
                Where(r => !string.IsNullOrEmpty(r)).
                ToArray();
        }

        private static List<DescriptionStep> GetDescriptionSteps(SQLiteDataReader reader)
        {
            var result = new List<DescriptionStep>();

            string description = reader.GetString(reader.GetOrdinal("Description"));
            string[] steps = description.Split(Environment.NewLine);

            string stepTitle = string.Empty;
            string stepDescription = string.Empty;

            foreach (string step in steps)
            {
                if (step.Contains("Крок"))
                {
                    if (!string.IsNullOrEmpty(stepTitle))
                    {
                        result.Add(new DescriptionStep(stepTitle, stepDescription));

                        stepTitle = string.Empty;
                        stepDescription = string.Empty;
                    }

                    stepTitle += step;
                }
                else
                {
                    stepDescription += step;
                }
            }

            return result;
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
