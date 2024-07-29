using log4net;
using Newtonsoft.Json;
using System;
using Terraria;

namespace AchiSplatoon2.Helpers
{
    enum LogMessageType
    {
        Info,
        Warn,
        Error,
    }
    internal static class DebugHelper
    {
        private static void LogMessage(string message, ILog logger, LogMessageType type)
        {
            switch (type)
            {
                case LogMessageType.Info:
                    logger.Info(message);
                    break;
                case LogMessageType.Warn:
                    logger.Info(message);
                    break;
                case LogMessageType.Error:
                    logger.Info(message);
                    break;
            }
        }

        private static DateTime GetTime()
        {
            DateTime now = DateTime.Now;
            return now.AddTicks(-(now.Ticks % TimeSpan.TicksPerSecond));
        }

        public static void PrintError(string message, ILog logger = null)
        {
            Main.NewText($"<Error - {GetTime()}> {message}", ColorHelper.GetInkColor(InkColor.Red));
            if (logger != null) LogMessage(message, logger, LogMessageType.Error);
        }

        public static void PrintWarning(string message, ILog logger = null)
        {
            Main.NewText($"<Warning - {GetTime()}> {message}", ColorHelper.GetInkColor(InkColor.Yellow));
            if (logger != null) LogMessage(message, logger, LogMessageType.Warn);
        }

        public static void PrintInfo(string message, ILog logger = null)
        {
            Main.NewText($"<Info - {GetTime()}> {message}", ColorHelper.GetInkColor(InkColor.Aqua));
            if (logger != null) LogMessage(message, logger, LogMessageType.Info);
        }

        public static void PrintError(object message, ILog logger = null) => PrintError(JsonConvert.SerializeObject(message), logger);
        public static void PrintWarning(object message, ILog logger = null) => PrintWarning(JsonConvert.SerializeObject(message), logger);
        public static void PrintInfo(object message, ILog logger = null) => PrintInfo(JsonConvert.SerializeObject(message), logger);

    }
}
