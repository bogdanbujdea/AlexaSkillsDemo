using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JetBrainsRaffle
{
    public class TypeformAPI
    {
        public static async Task<List<string>> GetNames()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "<key>");
            var json = await httpClient.GetStringAsync("https://api.typeform.com/forms/<id>/responses");
            var responses = JsonConvert.DeserializeObject<Responses>(json);
            var names = responses.Items.Where(i => i.Answers != null)
                .Select(i => i.Answers.FirstOrDefault(a => a.Field.Id == "PcnBCIsn1rmn")?.Text)
                .ToList();
            return names;
        }
    }

    public static class RandomPicker
    {
        private static Random rng = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }

    public class Responses
    {
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public Answer[] Answers { get; set; }
    }

    public class Typeform
    {
        [JsonProperty("form_response")]
        public FormResponse FormResponse { get; set; }
    }

    public class FormResponse
    {
        [JsonProperty("form_id")]
        public string FormId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("landed_at")]
        public DateTime LandedAt { get; set; }

        [JsonProperty("submitted_at")]
        public DateTime SubmittedAt { get; set; }

        [JsonProperty("definition")]
        public Definition Definition { get; set; }

        [JsonProperty("answers")]
        public Answer[] Answers { get; set; }
    }

    public class Definition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("fields")]
        public Field[] Fields { get; set; }
    }

    public class Field
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("_ref")]
        public string Ref { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }
    }

    public class Properties
    {
    }

    public class Answer
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("field")]
        public Field Field { get; set; }
    }
}