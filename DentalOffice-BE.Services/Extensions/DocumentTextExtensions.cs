using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using DentalOffice_BE.Data;
using DentalOffice_BE;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using System.Text;

namespace DentalOffice_BE.Services.Extensions
{
    public static class IDocumentTextExtensions
    {
        static readonly Assembly[] _references = [
        typeof(Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions).Assembly,

            typeof(DentalOffice_BE.Data.StageDto).Assembly
            
        ];

        public static string ReplaceAndExecuteCode<T>(this string input, T model)
        {
            string pattern = @"\{\{(.+?)\}\}";

            string inputObj = JsonConvert.DeserializeObject<object>(input)!.ToString()!;

            // Usiamo una funzione di callback per gestire la sostituzione dei blocchi {{}}
            string result = Regex.Replace(input, pattern, m =>
            {
                string expression = m.Groups[1].Value.Trim();

                // Verifica se l'espressione contiene la keyword "foreach"

                Globals<T> data = new Globals<T>()
                {
                    data = model
                };

                if (expression.StartsWith("DateTime.Now"))
                {
                    var now = DateTime.Now.Date;
                    return $"{now.Day}/{now.Month}/{now.Year}";
                }

                if (expression.StartsWith("IList"))
                {
                    // Esegui il loop sugli elementi e concatena i valori delle proprietà
                    return ExecuteForeachExpression<T>(expression, data);
                }
                else
                {
                    // Esegui il codice sostituendo il nome della variabile con il valore di 'data'
                    return ExecuteCode<T>(expression, data);
                }
            });

            return result;
        }

        public static string ExecuteCode<T>(string expression, object data)
        {
            expression = Regex.Unescape(expression);
            var assemblyReferences = _references;
            var assemblyLoader = LoadAssemblies(assemblyReferences);

            var encoding = Encoding.UTF8;

            var options = ScriptOptions.Default
            .WithReferences(assemblyReferences)
            .WithEmitDebugInformation(true)
            .WithFileEncoding(encoding);


            try
            {
                // Esegui il codice e restituisci il risultato
                var result = CSharpScript.EvaluateAsync("using System.Linq; using System.Collections.Generic; using DentalOffice_BE.Data;" + expression, options, globalsType: typeof(Globals<T>), globals: data).Result;

                if (result is DateTime)
                {
                    DateTime date = (DateTime)result;
                    var locale = date.ToLocalTime();
                    result = $"{locale.Day}/{locale.Month}/{locale.Year}";
                }

                return result?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Gestione degli errori
                return $"ERROR: {ex.Message}";
            }
        }

        public static string ExecuteForeachExpression<T>(string expression, object data)
        {
            try
            {
                var references = new[]
                {
                    typeof(object).Assembly,
                    typeof(IList<object>).Assembly
                };

                IList<object>? result = CSharpScript.EvaluateAsync("using System.Collections.Generic;" + expression, ScriptOptions.Default.WithReferences(references), globalsType: typeof(Globals<T>), globals: data).Result as IList<object>;
                
                if (result is IList<object> strings)
                {
                    var distinctStrings = strings.Distinct();
                    var total = string.Join(", ", distinctStrings);
                    return total;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return $"ERROR: {ex.Message}";
            }
        }

        public class Globals<T>
        {
            public T? data { get; set; }
        }

        static InteractiveAssemblyLoader LoadAssemblies(Assembly[] assemblies)
        {
            var assemblyResolver = new InteractiveAssemblyLoader();

            foreach (var assemblyReference in assemblies)
            {
                assemblyResolver.RegisterDependency(assemblyReference);
            }

            return assemblyResolver;
        }
    }
}
