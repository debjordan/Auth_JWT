using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/consume", async (IHttpClientFactory clientFactory) =>
{
    // Gerar JWT
    var claims = new[]
    {
        new Claim("user_id", "123")
    };
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("minha_chave_secreta_super_segura_1234567890"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(15),
        signingCredentials: creds
    );
    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    // Chamar API 1
    var client = clientFactory.CreateClient();
    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenString);
    var response = await client.GetAsync("http://localhost:5000/protected");

    if (response.IsSuccessStatusCode)
    {
        var data = await response.Content.ReadFromJsonAsync<object>();
        return Results.Ok(new { DataFromApi1 = data });
    }
    else
    {
        var error = await response.Content.ReadAsStringAsync();
        return Results.Problem($"Falha ao consumir API 1: {error}", statusCode: (int)response.StatusCode);
    }
});

app.Run();
