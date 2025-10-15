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
    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; SoACA1v2/1.0)");
});

builder.Services.AddHttpClient<GooglePlacesClient>(client =>
{
    client.BaseAddress = new Uri("https://places.googleapis.com/v1/places/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; SoACA1v2/1.0)");
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