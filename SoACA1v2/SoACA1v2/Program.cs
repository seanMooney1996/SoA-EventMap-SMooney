using GoogleMapsComponents;
using SoACA1v2.Components;
using Microsoft.Extensions.Configuration;
using SoACA1v2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazorBootstrap();

builder.Services.AddHttpClient<TicketMasterClient>(client =>
{
    client.BaseAddress = new Uri("https://app.ticketmaster.com/discovery/v2/");
});

builder.Services.AddHttpClient<GooglePlacesClient>(client =>
{
    client.BaseAddress = new Uri("https://places.googleapis.com/v1/places/");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();