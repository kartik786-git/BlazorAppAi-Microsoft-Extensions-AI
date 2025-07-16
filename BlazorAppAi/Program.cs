using BlazorAppAi.Components;
using Microsoft.Extensions.AI;
using OllamaSharp;
using OpenAI;
using System;
using System.ClientModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Ollama chat client.
//IChatClient chatClient = new OllamaApiClient(new Uri("http://localhost:11434"),
//    "llama3.2");

// foundry ai chat client.
ApiKeyCredential key = new ApiKeyCredential("manager.ApiKey");
OpenAIClient openAIClient = new OpenAIClient(key, new OpenAIClientOptions 
{ 
    Endpoint = new Uri("http://localhost:5273/v1")
});

var chatClient = openAIClient.AsChatClient("deepseek-r1-distill-qwen-7b-cuda-gpu");

builder.Services.AddChatClient(chatClient).UseFunctionInvocation().UseLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
