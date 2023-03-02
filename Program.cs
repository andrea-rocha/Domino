using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Domino.Models;
using Domino.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiMinimaConJwt", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Autorizacion",
        Description = "Ingrese el JWT con Bearer en este campo",
        Type = SecuritySchemeType.Http
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IFichaService, FichaService>();
builder.Services.AddSingleton<IUsuarioService, UsuarioService>();


var app = builder.Build();

app.UseAuthorization();
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiMinimaConJwt v1"));


IResult Login(login user, IUsuarioService service)
{
    if (!string.IsNullOrEmpty(user.NombreUsuario) &&
        !string.IsNullOrEmpty(user.Contraseña))
    {
        var loggedInUser = service.Get(user);
        if (loggedInUser is null) return Results.NotFound("Usuario no encontrado");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.NombreUsuario),
            new Claim(ClaimTypes.Email, loggedInUser.Correo),
            new Claim(ClaimTypes.GivenName, loggedInUser.Nombre),
            new Claim(ClaimTypes.Surname, loggedInUser.Apellido),
            new Claim(ClaimTypes.Role, loggedInUser.Rol)
        };

        var token = new JwtSecurityToken
        (
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(60),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Ok(tokenString);
    }
    return Results.BadRequest("Credenciales de usuario inválidas");
}

IResult Create(Ficha ficha, IFichaService service)
{
    var result = service.Create(ficha);
    return Results.Ok(result);
}


IResult Get(string Nombre, IFichaService service)
{
    var ficha = service.Get(Nombre);

    if (ficha is null) return Results.NotFound("Ficha no encontrada");

    return Results.Ok(ficha);
}

IResult List(IFichaService service)
{
    var Listafichas = service.List();

    return Results.Ok(Listafichas);
}

IResult Delete(string nombre, IFichaService service)
{
    var result = service.Delete(nombre);

    if (!result) Results.BadRequest("Algo salió mal");

    return Results.Ok(result);
}


IResult Update(Ficha fichaActualizada, IFichaService service)
{
    var updatedFicha = service.Update(fichaActualizada);

    if (updatedFicha is null) Results.NotFound("Ficha no encontrada");

    return Results.Ok(updatedFicha);
}
 IResult OrdenarFichas(List<Ficha> fichas, IFichaService service)
{
    if (fichas.Count < 2)
    {
        var error = new { mensaje = "El conjunto de fichas debe tener al menos dos elementos." };
        return Results.BadRequest(error);
    }

    if (fichas.Count > 6)
    {
        var error = new { mensaje = "El conjunto de fichas debe tener como máximo seis elementos." };
        return Results.BadRequest(error);
    }

    try
    {
        var fichasOrdenadas = service.OrdenarFichas(fichas);
        return Results.Ok(fichasOrdenadas);
    }
    catch (ArgumentException ex)
    {
        var error = new { mensaje = ex.Message };
        return Results.BadRequest(error);
    }
}





app.MapGet("/", () => "Hello World!")
    .ExcludeFromDescription();

app.MapGet("/list", (IFichaService service) => List(service))
    .Produces<List<Ficha>>(statusCode: 200, contentType: "application/json");

app.MapGet("/get",
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Administrator")]
(string Nombre, IFichaService service) => Get(Nombre, service))
    .Produces<Ficha>();

app.MapPost("/login",(login user,IUsuarioService service) => Login(user, service))
    .Accepts<login>("application/json")
    .Produces<string>();

app.MapPut("/update",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Ficha fichaActualizada, IFichaService service) => Update(fichaActualizada, service))
    .Accepts<Ficha>("application/json")
    .Produces<Ficha>(statusCode: 200, contentType: "application/json");

app.MapPost("/OrdenarFichas", ([FromBody] List<Ficha> fichas, IFichaService service) => OrdenarFichas(fichas, service))
    .Accepts<Ficha>("application/json")
    .Produces<List<Ficha>>(statusCode: 200, contentType: "application/json");

app.MapPost("/create",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Ficha ficha, IFichaService service) => Create(ficha, service))
    .Accepts<Ficha>("application/json")
    .Produces<Ficha>(statusCode: 200, contentType: "application/json");

app.MapDelete("/delete",
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(string nombre, IFichaService service) => Delete(nombre, service));



app.Run();

