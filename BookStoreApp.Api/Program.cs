using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Logging using nuGet packagae SeriLog
builder.Host.UseSerilog((context, loggingConfiguration) =>
    loggingConfiguration.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

// CORS - Cross-Origin Resource Sharing Policyfor. Basically who are allowed to access this API ?
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll",
        b => b
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Tell the App to use the CORS Policy, and providing the policy we created above
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();