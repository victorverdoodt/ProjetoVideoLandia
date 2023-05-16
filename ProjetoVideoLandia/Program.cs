using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoVideoLandia.ActionFilter;
using ProjetoVideoLandia.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VideoLandiaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ProjetoVideoLandiaContext") ?? throw new InvalidOperationException("Connection string 'ProjetoVideoLandiaContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{

});
var key = Encoding.ASCII.GetBytes("fedaf7d8863b48e197b9287d492b708e");



builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TokenActionFilter>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // Em produção, defina isso como verdadeiro
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // Verifique se o emissor (issuer) do token é válido
        ValidateAudience = false, // Verifique se o público (audience) do token é válido
        ValidateLifetime = true, // Verifique se o tempo de vida (lifetime) do token não foi expirado
        ValidateIssuerSigningKey = true, // Verifique se a chave de assinatura do token é válida
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fedaf7d8863b48e197b9287d492b708e"))
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseMiddleware<TokenActionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Filmes}/{action=Index}/{id?}");

app.Run();
