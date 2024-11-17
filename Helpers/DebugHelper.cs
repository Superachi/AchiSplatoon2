using log4net;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using Terraria.ID;

namespace AchiSplatoon2.Helpers
{
    enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
    }

    internal static class DebugHelper
    {
        public static void PrintStackTrace(object caller, int amount, Color? color = null)
        {
            if (color == null) color = Color.White;

            string currentClass = "";
            ChatHelper.SendChatToThisClient($"{DateTime.Now.TimeOfDay} ==========", color);

            if (currentClass != caller.GetType().Name)
            {
                currentClass = caller.GetType().Name;
                ChatHelper.SendChatToThisClient($"Class: {currentClass}", Color.Orange);
            }

            for (var i = 0; i < amount; i++)
            {
                ChatHelper.SendChatToThisClient($"{i}>{(new System.Diagnostics.StackTrace()).GetFrame(i + 2).GetMethod().Name}", Color.Yellow);
            }
        }

        public static void PrintError(string message, ILog? logger = null)
        {
            LogMessage(LogLevel.Error, message, logger);
        }

        public static void PrintWarning(string message, ILog? logger = null)
        {
            LogMessage(LogLevel.Warn, message, logger);
        }

        public static void PrintInfo(string message, ILog? logger = null)
        {
            LogMessage(LogLevel.Info, message, logger);
        }

        public static void PrintDebug(string message, ILog? logger = null)
        {
            LogMessage(LogLevel.Debug, message, logger);
        }

        public static void Ping() => PrintInfo("Ping!");

        public static void PrintError(object message, ILog? logger = null) => PrintError(JsonConvert.SerializeObject(message), logger);
        public static void PrintWarning(object message, ILog? logger = null) => PrintWarning(JsonConvert.SerializeObject(message), logger);
        public static void PrintInfo(object message, ILog? logger = null) => PrintInfo(JsonConvert.SerializeObject(message), logger);
        public static void PrintDebug(object message, ILog? logger = null) => PrintDebug(JsonConvert.SerializeObject(message), logger);

        private static void LogMessage(LogLevel logLevel, string message, ILog? logger = null)
        {
            string newMessage = FormatMessage(logLevel, message);
            Color messageColor = Color.White;

            switch (logLevel)
            {
                case LogLevel.Debug:
                    messageColor = Color.Gray;
                    logger?.Debug(newMessage);
                    break;
                case LogLevel.Info:
                    messageColor = Color.Turquoise;
                    logger?.Info(newMessage);
                    break;
                case LogLevel.Warn:
                    messageColor = Color.Orange;
                    logger?.Warn(newMessage);
                    SoundHelper.PlayAudio(SoundID.Item35);
                    break;
                case LogLevel.Error:
                    messageColor = Color.Crimson;
                    logger?.Error(newMessage);
                    SoundHelper.PlayAudio(SoundID.Item47);
                    break;
            }

            ChatHelper.SendChatToThisClient(newMessage, messageColor);
        }

        private static string GetTime()
        {
            DateTime now = DateTime.Now;
            return now.ToString("HH:mm:ss");
        }

        private static string FormatMessage(LogLevel logLevel, string message)
        {
            return $"<WoomyMod {logLevel} @ {GetTime()}> {message}";
        }

    }
}
