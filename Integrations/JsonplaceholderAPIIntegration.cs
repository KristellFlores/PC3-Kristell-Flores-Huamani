using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using petclinic.DTO;
using Pc3.DTO;

namespace petclinic.Integrations
{
    public class JsonplaceholderAPIIntegration
    {
        private readonly ILogger<JsonplaceholderAPIIntegration> _logger;
        private const string API_URL="https://jsonplaceholder.typicode.com/todos/";
        private readonly HttpClient _client;

        private List<PostDTO> localPosts = new List<PostDTO>();

        public JsonplaceholderAPIIntegration(ILogger<JsonplaceholderAPIIntegration> logger){
            _logger = logger;
            _client = new HttpClient();
        }

        //public async Task<List<TodoDTO>> GetAll(){
        //    string requestUrl = $"{API_URL}";
        //    List<TodoDTO> listado = new List<TodoDTO>();
        //    try{
        //        HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            listado =  await response.Content.ReadFromJsonAsync<List<TodoDTO>>() ?? new List<TodoDTO>();
        //        }
        //    }catch(Exception ex){
        //        _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
        //    }
        //    return listado;
        //}

        public async Task<List<PostDTO>> GetAllPostsAsync()
        {
            string requestUrl = $"{API_URL}";
            List<PostDTO> listado = new List<PostDTO>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    listado = await response.Content.ReadFromJsonAsync<List<PostDTO>>() ?? new List<PostDTO>();

                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
            }

            // Combina los posts de la API con los posts locales
            return listado.Concat(localPosts).ToList();
        }

        public async Task<PostDTO> CreatePostAsync(PostDTO post)
        {
            try
            {
                /*string requestUrl = $"{API_URL}";
                StringContent content = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(requestUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PostDTO>(responseBody);*/

                post.id = localPosts.Any() ? localPosts.Max(p => p.id) + 1 : 1; // Asignamos un nuevo ID
                localPosts.Add(post);
                return post;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el post: {ex.Message}");
                return null;
            }
        }


        public async Task<PostDTO> GetPostByIdAsync(int id)
        {
            var localPost = localPosts.FirstOrDefault(p => p.id == id);
            if (localPost != null)
            {
                return localPost;
            }

            try
            {
                string requestUrl = $"{API_URL}";
                HttpResponseMessage response = await _client.GetAsync(requestUrl + id);
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PostDTO>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el post con ID {id}: {ex.Message}");
                return null;
            }
        }


        public async Task<PostDTO> UpdatePostAsync(int id, PostDTO updatedPost)
        {
                         
                var localPost = localPosts.FirstOrDefault(p => p.id == id);
                if (localPost != null)
                {
                    localPost.title = updatedPost.title;
                    localPost.Body = updatedPost.Body;
                    return localPost;
                }

                // Si el post no está en la lista local, lo añade a la lista local
                localPosts.Add(updatedPost);
                return updatedPost;                       
        }


        

    }
}