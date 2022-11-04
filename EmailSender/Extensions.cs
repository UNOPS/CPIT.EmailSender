using System.Reflection;

namespace EmailSender;

public static class Extensions
{
    public static string GetEmbeddedResourceText(this Assembly assembly, string embeddedResourceName)
    {
        using (var stream = assembly.GetManifestResourceStream(embeddedResourceName))
        {
            if (stream == null)
            {
                var message = $"Embedded resource '{embeddedResourceName}' cannot be found in assembly '{assembly.FullName}'.";

                throw new ArgumentException(message);
            }

            using (var ms = new StreamReader(stream))
            {
                return ms.ReadToEnd();
            }
        }
    }
}