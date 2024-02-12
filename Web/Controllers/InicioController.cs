using System.Diagnostics;
using System.Dynamic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class InicioController : Controller
{
    private readonly IHttpClientFactory _httpClient;

    public InicioController(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
    }

    public IActionResult Index()
    {
        return View();
    }


    public async Task<IActionResult> Formulario()
    {
        var random = new Random();
        var dice1 = random.Next(1, 7);
        var dice2 = random.Next(1,7);

        var httpClient = _httpClient.CreateClient();
        var task1 = httpClient.GetAsync($"https://jsonplaceholder.typicode.com/todos/{dice1}");
        var task2 = httpClient.GetAsync($"https://jsonplaceholder.typicode.com/todos/{dice2}");
        var response = await Task.WhenAll(task1, task2);

        dynamic? obj1 = await response[0].Content.ReadFromJsonAsync<ExpandoObject>();
        dynamic? obj2 = await response[1].Content.ReadFromJsonAsync<ExpandoObject>();

        var persona = new Persona { Nombre = obj1.title.GetString() ?? "", Edad = obj2.id.GetInt32()};
        return View(persona);
    }


    public IActionResult Privacidad()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
