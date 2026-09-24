using AktieHandelRepositoryLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .WithMethods("GET", "POST", "PUT")
                  .AllowAnyHeader();
        });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<IAktieHandelRepositoryAsync,AktieHandelRepositoryDB>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
   
//}

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
