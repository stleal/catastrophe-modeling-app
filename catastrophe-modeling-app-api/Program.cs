var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
	options.AddPolicy("LocalFrontend", policy =>
	{
		policy.WithOrigins("http://localhost:5266", "https://localhost:7032")
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});
builder.Services.AddTransient<IProcessService, ProcessService>();
var app = builder.Build();
app.UseCors("LocalFrontend");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();