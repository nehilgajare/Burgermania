using Burgermania.Data;
using Burgermania.Models;
using Burgermania.Options;
using Burgermania.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Burgermania
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            // Add services to the container.
            builder.Services.AddDbContext<BurgerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http://127.0.0.1:5500")
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            builder.Services.AddControllers();


            JwtSettings jwtSettings = new JwtSettings();
            //to bind properties of JwtSettings
            builder.Configuration.Bind("JwtSettings", jwtSettings);

            //register the 
            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddScoped<ITokenService, TokenService>();
            // JWT Authentication
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer, // Ensure this matches the token's issuer
                    ValidAudience = jwtSettings.Audience, // Ensure this matches the token's audience
                    IssuerSigningKey =key
                    
                };
                //x.Events = new JwtBearerEvents
                //{
                //    OnAuthenticationFailed = context =>
                //    {
                //        Console.WriteLine("Authentication failed: " + context.Exception.Message);
                //        return Task.CompletedTask;
                //    },
                //    OnTokenValidated = context =>
                //    {
                //        Console.WriteLine("Token validated successfully.");
                //        return Task.CompletedTask;
                //    }
                //};
            });

            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Burgermania API", Version = "v1" });

                // Add security definition for JWT Bearer
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowSpecificOrigins");

            app.UseAuthentication(); // Enable authentication
            app.UseAuthorization();  // Enable authorization

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BurgerDbContext>();
                AddBurgerData(dbContext);
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BurgerDbContext>();
                SeedData(dbContext); // Ensure this is awaited
            }

            app.Run();
        }

        public static async Task SeedData(BurgerDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    MobileNumber = "8421554421",
                    Password = "admin", // Store hashed passwords in production
                    Role = "Admin"
                };

                var normalUser = new User
                {
                    MobileNumber = "1234567890",
                    Password = "userpass", // Store hashed passwords in production
                    Role = "User"
                };

                context.Users.AddRange(adminUser, normalUser);
                await context.SaveChangesAsync();
            }
        }
        static void AddBurgerData(BurgerDbContext dbContext)
        {
            if (!dbContext.Burgers.Any())
            {
                var burgers = new List<Burger>
                {
                    new Burger
                    {
                        Name = "Crispy Supreme",
                        Category = "Veg",
                        Price = 100,
                        ImageLink = "./images/veg-burger-delite.jpg"
                    },
                    new Burger
                    {
                        Name = "Chilli Cheese",
                        Category = "Veg",
                        Price = 100,
                        ImageLink = "./images/veg-burger-delite.jpg"
                    },
                    new Burger
                    {
                        Name = "Surprise Veg",
                        Category = "Veg",
                        Price = 100,
                        ImageLink = "./images/veg-burger-delite.jpg"
                    },
                    new Burger
                    {
                        Name = "Surprise Non-Veg",
                        Category = "Non-Veg",
                        Price = 200,
                        ImageLink = "./images/non-veg-burger.webp"
                    },
                    new Burger
                    {
                        Name = "WHOPPER Non-Veg",
                        Category = "Non-Veg",
                        Price = 200,
                        ImageLink = "./images/non-veg-burger.webp"
                    },
                    new Burger
                    {
                        Name = "Tandoor Grill Non-Veg",
                        Category = "Non-Veg",
                        Price = 200,
                        ImageLink = "./images/non-veg-burger.webp"
                    },
                    new Burger
                    {
                        Name = "Crispy Supreme Egg",
                        Category = "Eggs",
                        Price = 150,
                        ImageLink = "./images/egg-burger.jpg"
                    },
                    new Burger
                    {
                        Name = "WHOPPER Egg",
                        Category = "Eggs",
                        Price = 150,
                        ImageLink = "./images/egg-burger.jpg"
                    },
                    new Burger
                    {
                        Name = "Surprise Egg",
                        Category = "Eggs",
                        Price = 150,
                        ImageLink = "./images/egg-burger.jpg"
                    },
                };

                foreach (var burger in burgers)
                {
                    if (!dbContext.Burgers.Any(b => b.Name == burger.Name))
                    {
                        dbContext.Burgers.Add(burger);
                    }
                }

                dbContext.SaveChanges();
            }
        }
    }
}
