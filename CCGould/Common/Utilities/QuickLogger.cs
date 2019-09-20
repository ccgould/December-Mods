﻿namespace Common.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    //Created by PrimeSonic GitHub repo: https://github.com/PrimeSonic/PrimeSonicSubnauticaMods

    public static class QuickLogger
    {
        public static bool DebugLogsEnabled = false;

        public static void Info(string msg, bool showOnScreen = false)
        {
            string name = Assembly.GetCallingAssembly().GetName().Name;

            Console.WriteLine($"[{name}:INFO] {msg}");

            if (showOnScreen)
                ErrorMessage.AddMessage(msg);
        }

        public static void Message(string msg, bool showOnScreen = false)
        {
            string name = Assembly.GetCallingAssembly().GetName().Name;

            Console.WriteLine($"[{name}] : {msg}");

            if (showOnScreen)
                ErrorMessage.AddMessage(msg);
        }

        public static void Debug(string msg, bool showOnScreen = false)
        {
            if (!DebugLogsEnabled)
                return;

            string name = Assembly.GetCallingAssembly().GetName().Name;

            Console.WriteLine($"[{name}:DEBUG] {msg}");

            if (showOnScreen)
                ErrorMessage.AddDebug(msg);

        }

        public static void Error(string msg, bool showOnScreen = false)
        {
            string name = Assembly.GetCallingAssembly().GetName().Name;

            Console.WriteLine($"[{name}:ERROR] {msg}");

            if (showOnScreen)
                ErrorMessage.AddError(msg);
        }

        public static void Error(string msg, Exception ex)
        {
            string name = Assembly.GetCallingAssembly().GetName().Name;

            Console.WriteLine($"[{name}:ERROR] {msg}{Environment.NewLine}{ex.ToString()}");
        }

        public static void Error(Exception ex)
        {
            string name = Assembly.GetCallingAssembly().GetName().Name;

            Console.WriteLine($"[{name}:ERROR] {ex.ToString()}");
        }

        public static void Warning(string msg, bool showOnScreen = false)
        {
            string name = Assembly.GetCallingAssembly().GetName().Name;

            Console.WriteLine($"[{name}:WARN] {msg}");

            if (showOnScreen)
                ErrorMessage.AddWarning(msg);
        }

        public static string GetAssemblyVersion() => GetAssemblyVersion(Assembly.GetExecutingAssembly());

        public static string GetAssemblyVersion(Assembly assembly)
        {
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return FormatToSimpleVersion(fvi);
        }

        private static string FormatToSimpleVersion(FileVersionInfo version) => $"{version.FileMajorPart}.{version.FileMinorPart}.{version.FileBuildPart}";
    }
}