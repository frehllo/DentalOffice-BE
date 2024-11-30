using System.Diagnostics;

namespace DentalOffice_BE;

public static class DatabaseBackupHandler
{
    public static void CreateBackup(string host = "localhost", string dbName = "DentalStudio", string user = "postgres", string password = "narcis_buzatu", string backupPath = "DBbackup/backup.sql")
    {
        // Crea il comando pg_dump
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\PostgreSQL\16\bin\pg_dump.exe",
                Arguments = $"-h {host} -U {user} -d {dbName} -F c -f \"{backupPath}\"",
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

        process.Start();

        string error = process.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(error))
        {
            throw new Exception($"Backup failed: {error}");
        }

        process.WaitForExit();
    }
}
