using System.Diagnostics;

namespace DentalOffice_BE;

public static class DatabaseBackupHandler
{
    public static void CreateBackup(string host = "localhost", string dbName = "DentalStudio", string user = "postgres", string password = "narcis_buzatu", string backupPath = "DBbackup/backup.dump")
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\PostgreSQL\16\bin\pg_dump.exe", // Percorso a pg_dump
                Arguments = $"-h {host} -U {user} -d {dbName} -F c -f \"{backupPath}\"", // Formato personalizzato
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                EnvironmentVariables =
                {
                    ["PGPASSWORD"] = password // Passa la password in modo sicuro
                }
            }
        };

        try
        {
            process.Start();
            process.WaitForExit(); // Aspetta che il backup sia completato

            string error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception($"Backup failed: {error}");
            }

            Console.WriteLine("Backup completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating backup: {ex.Message}");
        }
    }
}
