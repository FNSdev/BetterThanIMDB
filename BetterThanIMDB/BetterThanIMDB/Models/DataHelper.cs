using BetterThanIMDB.Models.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BetterThanIMDB.CustomSettings;
using System.Globalization;

namespace BetterThanIMDB.Models
{
    public static class DataHelper
    {
        public static Dictionary<Guid, List<Guid>> FilmToActorConnections = new Dictionary<Guid, List<Guid>>();
        public static Dictionary<Guid, List<Guid>> FilmToProducerConnections = new Dictionary<Guid, List<Guid>>();

        private static async Task SaveTextAsync(string fileName, string json)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
            using (var writer = File.CreateText(filePath))
            {
                await writer.WriteAsync(json);
            }
        }

        private static async Task<string> ReadTextAsync(string fileName)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
            using (var reader = File.OpenText(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static async Task SaveAppInfo()
        {
            await SaveTextAsync("Films.txt", JsonConvert.SerializeObject(FilmCollection.Instance.Films));
            await SaveTextAsync("Actors.txt", JsonConvert.SerializeObject(ActorCollection.Instance.Actors));
            await SaveTextAsync("Producers.txt", JsonConvert.SerializeObject(ProducerCollection.Instance.Producers));
            await SaveTextAsync("FilmsToActors.txt", JsonConvert.SerializeObject(FilmToActorConnections));
            await SaveTextAsync("FilmsToProducers.txt", JsonConvert.SerializeObject(FilmToProducerConnections));
        }

        public static async Task SaveAppSettings()
        {
            SettingsProvider provider = new SettingsProvider
                { Culture = Settings.Instance.Culture.Name, DownloadImages = Settings.Instance.DownloadImages };
            await SaveTextAsync("Settings.txt", JsonConvert.SerializeObject(provider));
        }

        public static async Task LoadAppInfo()
        {
            string json = await ReadTextAsync("Films.txt");
            FilmCollection.Instance.Films.AddMany(JsonConvert.DeserializeObject<CustomObservableCollection<Film>>(json));
            json = await ReadTextAsync("Actors.txt");
            ActorCollection.Instance.Actors.AddMany(JsonConvert.DeserializeObject<CustomObservableCollection<Actor>>(json));
            json = await ReadTextAsync("Producers.txt");
            ProducerCollection.Instance.Producers.AddMany(JsonConvert.DeserializeObject<CustomObservableCollection<Producer>>(json));
            json = await ReadTextAsync("FilmsToActors.txt");
            var FtoA = JsonConvert.DeserializeObject<Dictionary<Guid, List<Guid>>>(json);
            json = await ReadTextAsync("FilmsToProducers.txt");
            var FtoP = JsonConvert.DeserializeObject<Dictionary<Guid, List<Guid>>>(json);
            await Task.Run(() =>
            {
                foreach (var filmId in FtoA.Keys)
                {
                    Film film = FilmCollection.Instance.Find(filmId);
                    foreach(var actorId in FtoA[filmId])
                    {
                        film.AddActor(ActorCollection.Instance.Find(actorId));
                    }
                    foreach(var producerId in FtoP[filmId])
                    {
                        film.AddProducer(ProducerCollection.Instance.Find(producerId));
                    }
                }
            });        
        }

        public static async Task LoadAppSettings()
        {
            string json = await ReadTextAsync("Settings.txt");
            SettingsProvider provider = JsonConvert.DeserializeObject<SettingsProvider>(json);
            Settings.Instance.Culture = new CultureInfo(provider.Culture);
            Settings.Instance.DownloadImages = provider.DownloadImages;
        }
    }
}
