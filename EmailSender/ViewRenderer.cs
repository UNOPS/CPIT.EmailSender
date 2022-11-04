using System.Net.Mail;
using System.Reflection;
using RazorLight;

namespace EmailSender
{
    /// <summary>
    /// Razor template engine.
    /// </summary>
    public static class ViewRenderer
    {
        /// <summary>
        /// Renders "cshtml" email template to string.
        /// </summary>
        /// <param name="template">
        /// Name of the email template (without the file extension, e.g. - "ChangeRequestAdded").
        /// </param>
        /// <param name="model">The model to pass to the view.</param>
        public static string RenderPartialView<T>(string template, T model)
        {
            var assembly = typeof(T).Assembly;

            var templateBody = RenderPartialView(assembly,  template + ".cshtml", model).Result;

            return templateBody;
        }

        /// <summary>
        /// Renders "cshtml" email template to string.
        /// </summary>
        /// <param name="assembly">Assembly where the embedded resource resides.</param>
        /// <param name="embeddedResourceName"></param>
        /// <param name="model">The model to pass to the view.</param>
        public static async Task<string> RenderPartialView<T>(Assembly assembly, string embeddedResourceName, T model)
        {
            var modelType = typeof(T);
            var engine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(MailMessage))
                .UseMemoryCachingProvider()
                .Build();

            var source = assembly.GetEmbeddedResourceText(embeddedResourceName);
            return await engine.CompileRenderStringAsync(modelType.FullName, source, model);
        }
    }
}