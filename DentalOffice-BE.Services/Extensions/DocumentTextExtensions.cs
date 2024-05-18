using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;

namespace DentalOffice_BE.Services.Extensions
{
    public static class IDocumentTextExtensions
    {
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
            try
            {
                // Esegui il codice e restituisci il risultato
                var result = CSharpScript.EvaluateAsync(expression, ScriptOptions.Default.WithReferences(typeof(object).Assembly), globalsType: typeof(Globals<T>), globals: data).Result;

                if (result is DateTime)
                {
                    DateTime date = (DateTime)result;
                    result = $"{date.Day}/{date.Month}/{date.Year}";
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
                    var total = string.Join(", ", strings);
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
    }
}
