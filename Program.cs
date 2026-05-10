using TrainingCenterApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

InMemoryData.Initialize();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
