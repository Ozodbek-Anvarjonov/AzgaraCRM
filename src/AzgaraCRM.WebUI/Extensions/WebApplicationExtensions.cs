namespace AzgaraCRM.WebUI.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseWebApplicationMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler();

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
}