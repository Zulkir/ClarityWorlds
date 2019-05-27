using System;
using System.IO;

namespace Clarity.App.Worlds.Logging
{
    public class FileLogWriter : ILogWriter
    {
        private readonly StreamWriter writer;

        public FileLogWriter()
        {
            var dateTime = DateTime.Now;
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
            var fileName = $"Logs/log-{dateTime.Year}-{dateTime.Month:D2}-{dateTime.Day:D2}-{dateTime.Hour:D2}-{dateTime.Minute:D2}-{dateTime.Second:D2}.txt";
            var stream = File.Open(fileName, FileMode.CreateNew, FileAccess.Write);
            writer = new StreamWriter(stream);
        }

        public void Write(LogMessageType messageType, string message)
        {
            writer.WriteLine($"{messageType}: {message}");
            writer.Flush();
        }
    }
}