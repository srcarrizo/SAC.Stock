namespace SAC.Stock.Infrastucture
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Seed.CodeTable;
    using Seed.ExceptionHandling;
    using Seed.Logging;
    using Seed.NLayer.ExceptionHandling;
    internal static class Logg
    {
        public static LogActivity BeginActivity(CodeItem operation, IDictionary<string, string> args = null)
        {
            var logg = LoggerFactory.CreateLogger();
            return logg == null ? null : logg.BeginActivity(operation.Code, args);
        }

        public static void EndActivity(LogActivity activity, IDictionary<string, string> data = null)
        {
            var logg = LoggerFactory.CreateLogger();
            if (logg == null)
            {
                return;
            }

            logg.EndActivity(activity, data);
        }

        public static LogProcess BeginProcess(CodeItem operation, IDictionary<string, string> args = null)
        {
            var logg = LoggerFactory.CreateLogger();
            return logg == null ? null : logg.BeginProcess(operation.Code, args);
        }

        public static void EndProcess(LogProcess process, IDictionary<string, string> data = null)
        {
            var logg = LoggerFactory.CreateLogger();
            if (logg == null)
            {
                return;
            }

            logg.EndProcess(process, data);
        }

        public static void Info(string message, IDictionary<string, string> data = null)
        {
            var logg = LoggerFactory.CreateLogger();
            if (logg == null)
            {
                return;
            }

            logg.Info(message, data);
        }

        public static void Verbose(string message, IDictionary<string, string> data = null)
        {
            var logg = LoggerFactory.CreateLogger();
            if (logg == null)
            {
                return;
            }

            logg.Verbose(message, data);
        }

        public static void Warning(string message, IDictionary<string, string> data = null)
        {
            var logg = LoggerFactory.CreateLogger();
            if (logg == null)
            {
                return;
            }

            logg.Warning(message, data);
        }

        public static void Error(Exception ex, IDictionary<string, string> data = null)
        {
            var logg = LoggerFactory.CreateLogger();
            if (logg == null)
            {
                return;
            }

            var message = ex.Message;

            var busEx = ex as BusinessRulesException;
            if (busEx != null)
            {
                if (data == null)
                {
                    data = new Dictionary<string, string>();
                }

                var myData =
                    data.Union(
                            busEx.Report.Select(
                                r => new KeyValuePair<string, string>(string.Format("BusinessReport-{0}", r.Code), r.Message)))
                        .ToDictionary(x => x.Key, x => x.Value);

                logg.Warning(
                    string.Format("BusinessRule-{0}: {1}", ex.GetType(), message),
                    myData);
            }
            else
            {
                var appEx = ex as AppException;
                if (appEx != null)
                {
                    logg.Error(string.Format("{0}: {1}", ex.GetType(), message), data);
                }
                else
                {
                    logg.Critical(string.Format("{0}: {1}", ex.GetType(), message), data);
                }
            }
        }

        public static void Error(Exception ex, CodeItem operation, IDictionary<string, string> data = null)
        {
            var logg = LoggerFactory.CreateLogger();
            if (logg == null)
            {
                return;
            }

            var message = ex.Message;

            var busEx = ex as BusinessRulesException;
            if (busEx != null)
            {
                if (data == null)
                {
                    data = new Dictionary<string, string>();
                }

                var myData =
                    data.Union(
                            busEx.Report.Select(
                                r => new KeyValuePair<string, string>(string.Format("BusinessReport-{0}", r.Code), r.Message)))
                        .ToDictionary(x => x.Key, x => x.Value);

                logg.Warning(
                    string.Format("BusinessRule-{0}: {1}\n[OP] {2}: {3}", ex.GetType(), message, operation.Code, operation.Name),
                    myData);
            }
            else
            {
                var appEx = ex as AppException;
                if (appEx != null)
                {
                    logg.Error(string.Format("{0}: {1}\n[OP] {2}: {3}", ex.GetType(), message, operation.Code, operation.Name), data);
                }
                else
                {
                    logg.Critical(string.Format("{0}: {1}\n[OP] {2}: {3}", ex.GetType(), message, operation.Code, operation.Name), data);
                }
            }
        }
    }
}
