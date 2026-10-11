using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace TaskManagerApp
{
    public static class AppRepository
    {
        // Calea unde se va salva fișierul pe calculator (în folderul AppData al utilizatorului)
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "TaskManagerApp_Data.json");

        public static ObservableCollection<ProjectModel> Projects { get; set; } = LoadFromFile();
        public static ProjectModel? CurrentProject { get; set; }

        // Salvarea datelor în fișier
        public static void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(Projects, options);
                File.WriteAllText(FilePath, jsonString);
            }
            catch (Exception ex)
            {
                // Gestionare simplă în caz de eroare
                Console.WriteLine("Eroare la salvare: " + ex.Message);
            }
        }

        // Încărcarea datelor din fișier la pornire
        private static ObservableCollection<ProjectModel> LoadFromFile()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string jsonString = File.ReadAllText(FilePath);
                    var loadedProjects = JsonSerializer.Deserialize<ObservableCollection<ProjectModel>>(jsonString);
                    if (loadedProjects != null)
                    {
                        return loadedProjects;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Eroare la citire: " + ex.Message);
            }

            return new ObservableCollection<ProjectModel>();
        }
    }
}