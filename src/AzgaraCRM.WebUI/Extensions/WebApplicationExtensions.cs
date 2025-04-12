using AzgaraCRM.WebUI.Helpers;

namespace AzgaraCRM.WebUI.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseWebApplicationMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseHalper();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors("AllowAllOrigins");
        app.UseAuthentication();
        app.UseAuthorization();


        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        return app;
    }

    private static WebApplication UseHalper(this WebApplication app)
    {
        HttpContextHelper.Accessor = app.Services.GetRequiredService<IHttpContextAccessor>();
        HttpContextHelper.SystemId = Convert.ToInt64(app.Configuration.GetSection("System:SystemId").Value);

        return app;
    }
}