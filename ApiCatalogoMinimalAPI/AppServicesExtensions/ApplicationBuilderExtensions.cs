namespace ApiCatalogoMinimalAPI.AppServicesExtensions
{
    public static class ApplicationBuilderExtensions
    {
        //metodo de extensão
        public static IApplicationBuilder UserExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            return app;
        }

        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors(p => {
                p.AllowAnyOrigin();
                p.WithMethods("GET");
                p.AllowAnyHeader();
            });
            return app;
        }

        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { 
            
            });

            return app;
        }
    }
}
