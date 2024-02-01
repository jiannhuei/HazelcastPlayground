using Hazelcast;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<HazelcastOptions>(o =>
{
    o.Networking.Addresses.Add("127.0.0.1:5701");
    o.Authentication.ConfigureUsernamePasswordCredentials("admin", "Pbb1223@");
    o.ClusterName = "pbbtrain";
});
builder.Services.AddTransient<HazelcastOptions>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
