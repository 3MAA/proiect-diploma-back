using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

using ProiectDiploma;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddOpenAIClient(new Uri(Environment.GetEnvironmentVariable("OPENAI_ENDPOINT")),
        new Azure.AzureKeyCredential(Environment.GetEnvironmentVariable("OPENAI_KEY")));
});
builder.Services.AddScoped<OpenAIService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Your API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");  
    app.UseHsts();
}

app.UseCors("default");
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
