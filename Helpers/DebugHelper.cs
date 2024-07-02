using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static void PrintError(string message, ILog logger = null)
        {
            Main.NewText($"<Error> {message}", ColorHelper.GetInkColor(InkColor.Red));
            if (logger != null) LogMessage(message, logger, LogMessageType.Error);
        }

        public static void PrintWarning(string message, ILog logger = null)
        {
            Main.NewText($"<Warning> {message}", ColorHelper.GetInkColor(InkColor.Yellow));
            if (logger != null) LogMessage(message, logger, LogMessageType.Warn);
        }

        public static void PrintInfo(string message, ILog logger = null)
        {
            Main.NewText($"<Info> {message}", ColorHelper.GetInkColor(InkColor.Aqua));
            if (logger != null) LogMessage(message, logger, LogMessageType.Info);
        }
    }
}
