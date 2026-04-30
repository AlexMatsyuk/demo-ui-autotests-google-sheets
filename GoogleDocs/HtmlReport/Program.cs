using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using NUglify;
using NUglify.Helpers;
using NUglify.Html;

namespace HtmlReport;

public static class Program
{
    private const string XsltFile = "Trxer.xslt";
    private const string OutputFileExt = ".html";

    public static void Main(string[] args)
    {
        Console.WriteLine($"{DateTime.UtcNow:u} start generating report");
        if (!args.Any())
        {
            Console.WriteLine("Trx file path or folder should be sent as parameter");
            return;
        }
        bool skipLogsForPassedTests = args.ToList().ConvertAll(x => x.ToLowerInvariant()).Contains("skipPassedTests".ToLowerInvariant());
        string fileOrFolderPath = args[0];
        string filePath = GetTrxFilePath(fileOrFolderPath);
        Console.WriteLine("Trx File (or folder): {0}", filePath);
        Transform(filePath, PrepareXsl(), args.Length > 2 ? args[2] : null, skipLogsForPassedTests);
        Console.WriteLine($"{DateTime.UtcNow:u} end generating report");
        OpenHtml($"{filePath}.html", args.Length > 1 && args[1].Contains("openhtml", StringComparison.InvariantCultureIgnoreCase));
    }

    private static void OpenHtml(string filePath, bool openReport)
    {
        Console.WriteLine($"html report: {filePath}");
        if (!openReport)
        {
            return;
        }
        try
        {
            Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", filePath);
        }
        catch
        {
            Console.WriteLine("cannot open report in browser");
        }
    }

    private static string GetTrxFilePath(string fileOrFolderPath)
    {
        FileAttributes attr = File.GetAttributes(fileOrFolderPath);
        return attr.HasFlag(FileAttributes.Directory)
            ? Path.Combine(fileOrFolderPath, new DirectoryInfo(fileOrFolderPath).GetFiles("*.trx").OrderByDescending(f => f.LastWriteTime).First().FullName)
            : fileOrFolderPath;
    }

    private static void Transform(
        string fileName,
        XmlDocument xsl,
        string additionalParameters,
        bool skipLogsForPassedTests)
    {
        if (skipLogsForPassedTests)
        {
            RemoveLogsForPassedTests(fileName);
        }
        OverrideSpecCharactersAndAddData(fileName, additionalParameters);
        XslCompiledTransform transformer = new(true);
        transformer.Load(xsl, new XsltSettings(true, true), new XmlUrlResolver());
        Console.WriteLine("Transforming...");
        string filePath = fileName + OutputFileExt;
        transformer.Transform(fileName, filePath);
        UglifyResult minifiedHtml = Uglify.Html(File.ReadAllText(filePath), new HtmlSettings { CollapseWhitespaces = false });
        File.WriteAllText(filePath, minifiedHtml.Code);
        Console.WriteLine("Done transforming xml into html");
    }

    /// <summary>
    /// parse <param name="additionalParameters"/> from "key1=value1;key2=value2;key3=value3" format to xml
    /// and add to <param name="text">text of the file</param>
    /// </summary>
    private static string AddAdditionalParameters(string text, string additionalParameters)
    {
        if (string.IsNullOrEmpty(additionalParameters))
        {
            return text;
        }
        StringBuilder parsedParameters = new();
        parsedParameters.Append("<AdditionalData>\n");
        additionalParameters.Split(';')
            .ToList()
            .ForEach(
                keyValue =>
                {
                    string key = keyValue.Split('=', 2)[0];
                    string value = keyValue.Split('=', 2)[1].Replace("&", "&amp;");
                    if (key.Contains("json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        value = value.Replace("'", "\"");
                    }
                    parsedParameters.Append($"<key name=\"{key}\">{value}</key>\n");
                });
        parsedParameters.Append("</AdditionalData>\n");
        return text.Replace("</TestSettings>", $"{parsedParameters}\n</TestSettings>");
    }

    private static void RemoveLogsForPassedTests(string filePath)
    {
        Console.WriteLine($"{DateTime.UtcNow:u} removing passed tests from report");
        try
        {
            string trxContent = File.ReadAllText(filePath);
            string pattern = "<UnitTestResult[^>]+outcome=\"Passed\"[^>]*>.*?</UnitTestResult>";
            string updatedTrxContent = Regex.Replace(trxContent, pattern, string.Empty, RegexOptions.Singleline);
            File.WriteAllText(filePath, updatedTrxContent);
            Console.WriteLine($"{DateTime.UtcNow:u} passed tests are removed from report");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void OverrideSpecCharactersAndAddData(string fileName, string additionalParameters)
    {
        StringBuilder decodedText = new();
        File.ReadLines(fileName).ForEach(line => decodedText.AppendLine(DecodeText(line)));
        string decodedTextWithFormattedTime = FormatTime(decodedText.ToString());
        string textWithFormatterJson = AddCollapsibleJson(decodedTextWithFormattedTime);
        string textWithAdditionalData = AddAdditionalParameters(textWithFormatterJson, additionalParameters);
        File.WriteAllText(fileName, textWithAdditionalData);
    }

    private static string DecodeText(string text)
    {
        string replaceErrorTag = Regex.Replace(text, "&lt;error/&gt;", "</span><error/><span>");
        string replaceStepOpenTag = Regex.Replace(replaceErrorTag, "&lt;step (.*?)&gt;", "<step " + "$1" + ">");
        string replaceStepData = Regex.Replace(replaceStepOpenTag, "&lt;stepData (.*?)&gt;", "<stepData " + "$1" + ">");
        string replaceCategoriesTag = Regex.Replace(replaceStepData, "&lt;categories (.*?)&gt;", "<categories " + "$1" + ">");
        string replaceTestCaseTag = Regex.Replace(replaceCategoriesTag, "&lt;testCase (.*?)&gt;", "<testCase " + "$1" + ">");
        string replaceBugTag = Regex.Replace(replaceTestCaseTag, "&lt;bug (.*?)&gt;", "<bug " + "$1" + ">");
        string replaceScreenshotTag = Regex.Replace(replaceBugTag, "&lt;screenshot (.*?)&gt;", "<screenshot " + "$1" + ">");
        string decodedText = replaceScreenshotTag
            .Replace("&lt;span&gt;", "<span>")
            .Replace("&lt;/span&gt;", "</span>")
            .Replace("&lt;/step&gt;", "</step>")
            .Replace("&lt;test&gt;", "<test>")
            .Replace("&lt;/test&gt;", "</test>")
            .Replace("\u001a", string.Empty);
        return decodedText;
    }

    private static string AddCollapsibleJson(string text)
    {
        string id = Guid.NewGuid().ToString();
        return text
            .Replace("&lt;jsonData&gt;", $"</span><span id='{id}' jsonData='true'>")
            .Replace("&lt;/jsonData&gt;", "</span><span>");
    }

    private static string FormatTime(string text)
    {
        string start = new Regex("start=\"(?<x>.*?)\"").Match(text).Groups["x"].Value;
        string end = new Regex("finish=\"(?<x>.*?)\"").Match(text).Groups["x"].Value;
        string textDuration = Regex.Replace(text, "queuing=\"(.*?)\"", $"queuing=\"{GetTestsDuration(start, end)}\"");
        string textStart = Regex.Replace(
            textDuration,
            "start=\"(.*?)\"",
            $"start=\"{DateTime.Parse(start, CultureInfo.CurrentCulture):yyyy-MM-dd HH:mm:ss zz}UTC\"");
        string textEnd = Regex.Replace(
            textStart,
            "finish=\"(.*?)\"",
            $"finish=\"{DateTime.Parse(end, CultureInfo.CurrentCulture):yyyy-MM-dd HH:mm:ss zz}UTC\"");
        return textEnd;
    }

    private static string GetTestsDuration(string start, string end)
    {
        try
        {
            TimeSpan timeSpan = DateTime.Parse(end, CultureInfo.CurrentCulture) - DateTime.Parse(start, CultureInfo.CurrentCulture);
            return timeSpan.ToString("hh':'mm':'ss'.'fff", CultureInfo.CurrentCulture);
        }
        catch
        {
            return string.Empty;
        }
    }

    private static XmlDocument PrepareXsl()
    {
        XmlDocument xslDoc = new();
        Console.WriteLine("Loading xslt template...");
        xslDoc.Load(ResourceReader.StreamFromResource(XsltFile));
        MergeCss(xslDoc);
        MergeJavaScript(xslDoc);
        return xslDoc;
    }

    private static void MergeJavaScript(XmlDocument xslDoc)
    {
        Console.WriteLine("Loading javascript...");
        XmlNode scriptEl = xslDoc.GetElementsByTagName("script")[0];
        XmlAttribute scriptSrc = scriptEl.Attributes["src"];
        string script = ResourceReader.LoadTextFromResource(scriptSrc.Value);
        scriptEl.Attributes.Remove(scriptSrc);
        scriptEl.InnerText = script;
    }

    private static void MergeCss(XmlDocument xslDoc)
    {
        Console.WriteLine("Loading css...");
        XmlNode headNode = xslDoc.GetElementsByTagName("head")[0];
        XmlNodeList linkNodes = xslDoc.GetElementsByTagName("link");
        List<XmlNode> toChangeList = linkNodes.Cast<XmlNode>().ToList();
        foreach (XmlNode xmlElement in toChangeList)
        {
            XmlElement styleEl = xslDoc.CreateElement("style");
            styleEl.InnerText = ResourceReader.LoadTextFromResource(xmlElement.Attributes["href"].Value);
            headNode.ReplaceChild(styleEl, xmlElement);
        }
    }
}