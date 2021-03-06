﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SignEditor
{

    public class Rootobject
    {
        public Screen[] Root { get; set; }
    }

    public class Screen
    {
        public string name { get; set; }
        public string uri { get; set; }
        public string feed { get; set; }
        public string order { get; set; }
        public string duration { get; set; }
        public string sign_text { get; set; }
        public string sign_type { get; set; }
        public string _id { get; set; }
        public int __v { get; set; }
        public DateTime modified_date { get; set; }
        public DateTime created_date { get; set; }
        public string Icon {
            get {
                if (sign_type == "web") { return "\xE774"; }
                else if (sign_type == "text") { return "\xE8E3"; }
                else if (sign_type == "image") { return "\xE8B9"; }
                else if (sign_type == "tweet") { return "\xE90A"; }
                else if (sign_type == "video") { return "\xE714"; }
                else if (sign_type == "hide") { return "\xEA14"; }
                else if (name == "New Screen") { return "\xE7C3"; }
                else { return "\xE897"; }
            }
        }
    }

    public class ScreenManager
    {

        string _baseURI = "";
        string _passcode = "";

        public ScreenManager(string uri, string passcode)
        {
            _baseURI = uri;
            _passcode = passcode;
        }

        public async Task<Screen[]> GetScreensAsync()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseURI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Screen[] sl = null;
            string items = "";


            HttpResponseMessage response = await client.GetAsync("screens?secret="+_passcode);
            if (response.IsSuccessStatusCode)
            {
                items = await response.Content.ReadAsStringAsync();
                sl = JsonConvert.DeserializeObject<Screen[]>(items);

            }

            List<Screen> nsl;
            nsl = sl.ToList<Screen>();
            nsl.Sort((x, y) => Convert.ToInt32(x.order).CompareTo(Convert.ToInt32(y.order)));

            Screen ns = new Screen();
            ns.name = "New Screen";
            nsl.Add(ns);

            return nsl.ToArray<Screen>();
        }

        public async Task<Screen> GetScreenAsync(string id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseURI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Screen s = null;
            string item = "";


            HttpResponseMessage response = await client.GetAsync("screens?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                item = await response.Content.ReadAsStringAsync();
                s = JsonConvert.DeserializeObject<Screen>(item);

            }

            return s;
        }

        public async Task UpdateScreenAsync(Screen screenToUpdate)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseURI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string content = JsonConvert.SerializeObject(screenToUpdate);
            string url = "save?secret="+_passcode+"&id=" + screenToUpdate._id;
            var o = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(content);
            o.Property("_id").Remove();
            o.Property("__v").Remove();
            o.Property("modified_date").Remove();
            o.Property("created_date").Remove();

            StringContent sc = new StringContent(o.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, sc);

            response.EnsureSuccessStatusCode();

            string r = await response.Content.ReadAsStringAsync();

        }

        public async Task CreateScreenAsync(Screen screenToUpdate)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseURI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string content = JsonConvert.SerializeObject(screenToUpdate);
            string url = "new?secret=" + _passcode;
            var o = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(content);
            o.Property("_id").Remove();
            o.Property("__v").Remove();
            o.Property("modified_date").Remove();
            o.Property("created_date").Remove();

            StringContent sc = new StringContent(o.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, sc);

            response.EnsureSuccessStatusCode();

            string r = await response.Content.ReadAsStringAsync();

        }

        public async Task DeleteScreenAsync(Screen screenToUpdate)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseURI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string content = JsonConvert.SerializeObject(screenToUpdate);
            string url = "delete?secret=" + _passcode + "&id=" + screenToUpdate._id;

            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string r = await response.Content.ReadAsStringAsync();

        }


    }
}
