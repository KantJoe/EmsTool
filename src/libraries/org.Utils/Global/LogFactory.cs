using Serilog;
using Serilog.Sinks.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace org.Utils.Global
{
    public class LogFactory
    {
        public static ILogger DebugLogger { get; private set; }
        public static ILogger InfoLogger { get; private set; }
        public static ILogger ErrorLogger { get; private set; }

        public static void Initialize()
        {
            if (!Directory.Exists(FilePathConst.SnapShots))
            {
                Directory.Exists(FilePathConst.SnapShots);
            }

            DebugLogger = new LoggerConfiguration().WriteTo
                .File(GeneratePath("Debug"),
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {Message}{NewLine}",
                fileSizeLimitBytes: 1024 * 1024 * 100,
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true,
                retainedFileTimeLimit: TimeSpan.FromHours(24 * 7 * 30),
                retainedFileCountLimit: 24 * 7 * 30,
                hooks: new EmptyFileLifecycleHooks())
                .MinimumLevel.Debug()
                .CreateLogger();

            InfoLogger = new LoggerConfiguration().WriteTo
                .File(GeneratePath("Info"),
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {Message}{NewLine}",
                fileSizeLimitBytes: 1024 * 1024 * 100,
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true,
                retainedFileTimeLimit: TimeSpan.FromHours(24 * 7 * 30),
                retainedFileCountLimit: 24 * 7 * 30,
                hooks: new EmptyFileLifecycleHooks())
                .MinimumLevel.Information()
                .CreateLogger();

            ErrorLogger = new LoggerConfiguration().WriteTo
                .File(GeneratePath("Error"),
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {Message}{NewLine}{Exception}{NewLine}",
                fileSizeLimitBytes: 1024 * 1024 * 100,
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true,
                retainedFileTimeLimit: TimeSpan.FromHours(24 * 7 * 30),
                retainedFileCountLimit: 24 * 7 * 30,
                hooks: new EmptyFileLifecycleHooks())
                .MinimumLevel.Error()
                .CreateLogger();
        }

        private static string GeneratePath(string level, string ext = ".log")
        {
            var now = DateTime.Now;
            return Path.Combine(FilePathConst.LogsPath, level, now.ToString("yyyyMMdd") , ext);
        }

        public static void Debug(string msg, params object[] propertyValues)
        {
            DebugLogger.Debug(msg, propertyValues);
        }

        public static void Info(string msg, params object[] propertyValues)
        {
            InfoLogger.Information(msg, propertyValues);
        }

        public static void Error(string msg, params object[] propertyValues)
        {
            Error(null, msg, propertyValues);
        }

        public static void Error(Exception ex, string msg, params object[] propertyValues)
        {
            ErrorLogger.Error(ex, msg, propertyValues);
        }

        public static ILogger CreateLogger(string name, string fileExt = ".log", string textTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {Message}{NewLine}")
        {
            return new LoggerConfiguration().WriteTo
                .File(GeneratePath(name, fileExt),
                outputTemplate: textTemplate,
                fileSizeLimitBytes: 1024 * 1024 * 100,
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true,
                retainedFileTimeLimit: TimeSpan.FromHours(24 * 7 * 30),
                retainedFileCountLimit: 24 * 7 * 30,
                hooks: new EmptyFileLifecycleHooks())
                .MinimumLevel.Verbose()
                .CreateLogger();
        }

        public static ILogger CreateExtensLogger(string filePath, string textTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {Message}{NewLine}")
        {
            return new LoggerConfiguration().WriteTo
                .File(filePath,
                outputTemplate: textTemplate,
                fileSizeLimitBytes: 1024 * 1024 * 100,
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true,
                retainedFileTimeLimit: TimeSpan.FromHours(24 * 7),
                retainedFileCountLimit: 24 * 7 * 30,
                hooks: new EmptyFileLifecycleHooks())
                .MinimumLevel.Verbose()
                .CreateLogger();
        }
    }
}
