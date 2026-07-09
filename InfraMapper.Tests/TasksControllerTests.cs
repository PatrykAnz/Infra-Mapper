using System.Net;
using System.Net.Http.Json;
using InfraMapper.Data;
using InfraMapper.DTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InfraMapper.Tests;

public class TasksControllerTests : IClassFixture<TasksControllerTests.InMemoryFactory>
{
    private readonly HttpClient _client;

    public TasksControllerTests(InMemoryFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsSeededTasks()
    {
        var response = await _client.GetAsync("/api/tasks");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskReadDto>>();
        Assert.NotNull(tasks);
    }

    [Fact]
    public async Task Create_Then_GetById_RoundTrips()
    {
        var create = new TaskCreateDto { Name = "test-task" };
        var post = await _client.PostAsJsonAsync("/api/tasks", create);
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);
        var created = await post.Content.ReadFromJsonAsync<TaskReadDto>();
        Assert.NotNull(created);
        Assert.False(created!.IsCompleted);

        var get = await _client.GetAsync($"/api/tasks/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }

    [Fact]
    public async Task GetById_MissingId_Returns404()
    {
        var response = await _client.GetAsync("/api/tasks/999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_IdMismatch_Returns400()
    {
        var dto = new TaskReadDto { Id = 2, Name = "x", IsCompleted = false };
        var response = await _client.PutAsJsonAsync("/api/tasks/1", dto);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    public class InMemoryFactory : WebApplicationFactory<Program>
    {
        private readonly string _dbName = $"infra-mapper-tests-{Guid.NewGuid()}";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<DbContextOptions<AppDbContext>>();
                services.RemoveAll<AppDbContext>();
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseInMemoryDatabase(_dbName));
            });
        }
    }
}
