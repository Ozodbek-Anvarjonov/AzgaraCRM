using AzgaraCRM.WebUI.Helpers;

namespace AzgaraCRM.WebUI.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseWebApplicationMiddleware(this WebApplication app)
    {
        app.UseHelper();
        app.UseExceptionHandler();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseCors("AllowAllOrigins");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseStaticFiles();

        app.MapControllers();

        return app;
    }

    private static WebApplication UseHelper(this WebApplication app)
    {
        HttpContextHelper.Accessor = app.Services.GetRequiredService<IHttpContextAccessor>();
        HttpContextHelper.SystemId = Convert.ToInt64(app.Configuration.GetSection("System:SystemId").Value);

        app.Services.Migration();
        //app.Services.SeedData();

        return app;
    }
}