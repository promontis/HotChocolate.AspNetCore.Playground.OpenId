using System;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace HotChocolate.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        private const string _resourcesNamespace =
            "HotChocolate.AspNetCore.Playground.OpenId.Resources";

        public static IApplicationBuilder UsePlayground(
            this IApplicationBuilder applicationBuilder,
            OpenIdConfiguration openIdConfiguration)
        {
            return applicationBuilder.UsePlayground(new PlaygroundOptions(openIdConfiguration));
        }

        public static IApplicationBuilder UsePlayground(
            this IApplicationBuilder applicationBuilder,
            OpenIdConfiguration openIdConfiguration,
            PathString queryPath)
        {
            return applicationBuilder.UsePlayground(new PlaygroundOptions(openIdConfiguration)
            {
                QueryPath = queryPath,
                Path = queryPath + new PathString("/playground")
            });
        }

        public static IApplicationBuilder UsePlayground(
            this IApplicationBuilder applicationBuilder,
            OpenIdConfiguration openIdConfiguration,
            PathString queryPath,
            PathString uiPath)
        {
            return applicationBuilder.UsePlayground(new PlaygroundOptions(openIdConfiguration)
            {
                QueryPath = queryPath,
                Path = uiPath
            });
        }

        public static IApplicationBuilder UsePlayground(
            this IApplicationBuilder applicationBuilder,
            PlaygroundOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var callbackPath = options.Path.Add("/callback");

            return applicationBuilder
                .UsePlaygroundSettingsMiddleware(options.Path, options)
                .UsePlaygroundSettingsMiddleware(callbackPath, options)
                .UsePlaygroundFileServer(options.Path)                
                .UsePlaygroundFileServer(callbackPath);
        }

        private static IApplicationBuilder UsePlaygroundSettingsMiddleware(
           this IApplicationBuilder applicationBuilder,
           PathString path,
           PlaygroundOptions options)
        {
            return applicationBuilder.Map(
                path.Add(new PathString("/settings.js")),
                app => app.UseMiddleware<SettingsMiddleware>(options));
        }

        private static IApplicationBuilder UsePlaygroundFileServer(
            this IApplicationBuilder applicationBuilder,
            string path)
        {
            var fileServerOptions = new FileServerOptions
            {
                RequestPath = path,
                FileProvider = CreateFileProvider(),
                EnableDefaultFiles = true,
                StaticFileOptions =
                {
                    ContentTypeProvider =
                        new FileExtensionContentTypeProvider()
                }
            };

            return applicationBuilder.UseFileServer(fileServerOptions);
        }

        private static IFileProvider CreateFileProvider()
        {
            Type type = typeof(ApplicationBuilderExtensions);

            return new EmbeddedFileProvider(
                type.Assembly,
                _resourcesNamespace);
        }
    }
}