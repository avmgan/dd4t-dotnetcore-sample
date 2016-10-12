using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System
{
    //public static class HtmlHelperExtensions
    //{
    //    public static IHtmlString DisplayOrActionFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, List<IRenderableViewModel>>> expression)
    //    {
    //        StringBuilder result = new StringBuilder();
    //        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
    //        foreach (var model in metadata.Model as List<IRenderableViewModel>)
    //        {
    //            result.Append(htmlHelper.DisplayOrActionFor(m => model));
    //        }

    //        return new MvcHtmlString(result.ToString());
    //    }

    //    public static IHtmlString DisplayOrActionFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IRenderableViewModel>> expression)
    //    {
    //        IHtmlString result;

    //        ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
    //        var model = metadata.Model as IRenderableViewModel;
    //        if (string.IsNullOrWhiteSpace(model.RenderData.Controller))
    //        {
    //            result = htmlHelper.DisplayFor(m => model, model.RenderData.View);
    //        }
    //        else
    //        {
    //            result = htmlHelper.Render(model);
    //        }

    //        return result;
    //    }
    //}
    public static class ArgumentNotNull
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static void ThrowIfNull<T>(this T obj, string parameterName)
                where T : class
        {
            if (obj == null) throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Throws an NullArgument exception if obj is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        /// <param name="parameterName"></param>
        public static void ThrowIfNull<T>(this T obj, string message, string parameterName)
               where T : class
        {
            if (obj == null) throw new ArgumentNullException(message, parameterName);
        }

        public static bool IsNull<T>(this T obj)
              where T : class
        {
            return (obj == null);
        }
    }

    public static class FormatExtensions
    {
        public static string FormatString(this string input, params object[] values)
        {
            if (!input.IsNullOrEmpty())
                return string.Format(input, values);

            return string.Empty;
        }
    }

    public static class URLFormatingExtensions
    {
        /// <summary>
        /// Converts a request URL into a CMS URL (for example adding default page name, and file extension)
        /// </summary>
        /// <param name="url">The request URL</param>
        /// <returns>A CMS URL</returns>
        public static string ParseUrl(this string url, string contextPath = null, bool locateIndex = false, string defaultFileName = "index.json")
        {
            if (string.IsNullOrEmpty(url))
            {
                url = defaultFileName;
            }

            url = url.TrimEnd('/');

            if (locateIndex)
            {
                url = "{0}/".FormatString(url);
            }
            if (!url.StartsWith("/") && !url.StartsWith("http"))
            {
                url = string.Format("/{0}", url);
            }
            if (!contextPath.IsNullOrEmpty() && !contextPath.Equals("/", StringComparison.CurrentCultureIgnoreCase) && !url.StartsWith("http"))
            {
                url = string.Concat(contextPath, url.Replace(contextPath, ""));
            }
            if (url.EndsWith("/"))
            {
                url = url + defaultFileName;
            }
            if (!Path.HasExtension(url))
            {
                var extension = Path.GetExtension(defaultFileName);
                url = string.Format("{0}{1}", url, extension);
            }

            return url;
        }

        /// <summary>
        /// Converts a CMS URL into a Request URL (for example removing default page name, and file extension)
        /// </summary>
        /// <param name="url">The CMS URL</param>
        /// <returns>A Request URL</returns>
        public static string CleanUrl(this string url, string defaultFileName)
        {
            //first make sure its a CMS URL
            url = url.ParseUrl(defaultFileName: defaultFileName);

            var name = Path.GetFileName(url);
            var nameNoExt = Path.GetFileNameWithoutExtension(name);
            var ext = Path.GetExtension(name);

            if (name == defaultFileName)
            {
                url = url.Replace(name, string.Empty);
            }
            else
            {
                url = url.Replace(name, nameNoExt);
            }

            if (!url.EndsWith("/"))
                url = string.Concat(url, "/");

            return url;
        }
    }
}