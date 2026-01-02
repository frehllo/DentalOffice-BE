using DentalOffice_BE;
using DentalOffice_BE.Data;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DentalOffice_BE.Services.Extensions
{
    public static class IDocumentTextExtensions
    {
        // Usiamo una cache che tiene conto anche del tipo T per evitare conflitti
        private static readonly ConcurrentDictionary<string, Script<object>> _scriptCache = new();

        public static string ReplaceAndExecuteCode<T>(this string input, T model)
        {
            var pattern = @"\{\{(.+?)\}\}";
            var globals = new Globals<T> { data = model };

            // Prepariamo le opzioni con i riferimenti necessari
            var options = ScriptOptions.Default
                .WithReferences(typeof(T).Assembly, typeof(Enumerable).Assembly, typeof(DentalOffice_BE.Data.StageDto).Assembly)
                .WithImports("System", "System.Linq", "System.Collections.Generic", "DentalOffice_BE.Data");

            return Regex.Replace(input, pattern, m =>
            {
                string expression = m.Groups[1].Value.Replace("\\\"", "\"").Trim();

                // Gestione rapida date
                if (expression == "DateTime.Now") return DateTime.Now.ToString("dd/MM/yyyy");

                try
                {
                    // La chiave della cache deve includere il nome del tipo T 
                    // perché lo script compilato per Globals<Paziente> è diverso da Globals<Fattura>
                    string cacheKey = $"{typeof(T).FullName}_{expression}";

                    var script = _scriptCache.GetOrAdd(cacheKey, _ => {
                        return CSharpScript.Create<object>(
                            expression,
                            options,
                            globalsType: typeof(Globals<T>)
                        );
                    });

                    // Esecuzione dello script pre-compilato
                    var result = script.RunAsync(globals).GetAwaiter().GetResult().ReturnValue;

                    return FormatResult(result);
                }
                catch (Exception ex)
                {
                    return $"[[ERR: {ex.Message}]]";
                }
            });
        }

        private static string FormatResult(object? result)
        {
            if (result == null) return string.Empty;

            if (result is DateTime dt)
                return dt.ToString("dd/MM/yyyy");

            if (result is System.Collections.IEnumerable list && !(result is string))
            {
                var parts = list.Cast<object>()
                    .Where(x => x != null)
                    .Select(x => x.ToString())
                    .Distinct();
                return string.Join(", ", parts);
            }

            return result.ToString() ?? string.Empty;
        }
    }

    public class Globals<T>
    {
        public T data { get; set; }
    }
}
