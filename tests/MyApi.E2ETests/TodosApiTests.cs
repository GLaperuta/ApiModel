using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MyApi.Application.Todos;
using Xunit;

namespace MyApi.E2ETests
{
    public class TodosApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        //private readonly HttpClient _client;
        private readonly HttpClient _client = factory.CreateClient();

        //public TodosApiTests(WebApplicationFactory<Program> factory)
        //{
        //    _client = factory.CreateClient();
        //}

        [Fact]
        public async Task Post_Then_GetAll_ShouldReturnItem()
        {
            var created = await (await _client.PostAsJsonAsync("/api/todos", new { title = "teste" }))
                .Content.ReadFromJsonAsync<dynamic>();

            Assert.NotNull(created);

            var all = await _client.GetFromJsonAsync<dynamic[]>("/api/todos");
            Assert.NotEmpty(all!);
        }

        [Fact]
        public async Task GetById_ShouldReturnTodo()
        {
            var createdResponse = await _client.PostAsJsonAsync("/api/todos", new { title = "teste" });
            createdResponse.EnsureSuccessStatusCode();

            var created = await createdResponse.Content.ReadFromJsonAsync<TodoDto>();
            Assert.NotNull(created);

            var response = await _client.GetAsync($"/api/todos/{created!.Id}");
            response.EnsureSuccessStatusCode();

            var todo = await response.Content.ReadFromJsonAsync<TodoDto>();
            Assert.NotNull(todo);
            Assert.Equal(created.Id, todo!.Id);
        }
    }
}