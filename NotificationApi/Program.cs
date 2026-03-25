using MassTransit;
using NotificationApi.Consumers;
using NotificationApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

// Add CORS
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyHeader()
			  .AllowAnyMethod()
			  .AllowCredentials()
			  .SetIsOriginAllowed(_ => true);
	});
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
	x.AddConsumer<NotificationConsumer>();

	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host("localhost", "/", h =>
		{
			h.Username("guest");
			h.Password("guest");
		});

		cfg.ReceiveEndpoint("notification-queue", e =>
		{
			e.ConfigureConsumer<NotificationConsumer>(context);
		});
	});
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

// Enable CORS — must be before other middleware
app.UseCors();

app.UseHttpsRedirection();
app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();