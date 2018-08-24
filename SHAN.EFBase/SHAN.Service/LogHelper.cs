using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDb.Common
{

    /// <summary>
    /// 应用日志帮助类
    /// </summary>
    public static class LogHelper
    {
        static LogHelper()
        {
        }


        #region Error
        public static void Error<T>(Exception exception, string message, params object[] args)
        {
            Error(typeof(T), exception, message, args);
        }

        public static void Error(Type callingType, Exception exception, string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            LogWriter.Instance.Enqueue(new LogEntity
            {
                CallType = callingType.Name,
                Level = LogLevel.Exception,
                Message = string.Concat(exception.ToString(), message),
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Time = DateTime.Now
            });
        }

        #endregion

        #region Warn
        public static void Warn(Type callingType, string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            LogWriter.Instance.Enqueue(new LogEntity
            {
                CallType = callingType.Name,
                Level = LogLevel.Warn,
                Message = message,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Time = DateTime.Now
            });
        }

        public static void Warn<T>(string message, params object[] args)
        {
            Warn(typeof(T), message, args);
        }
        #endregion

        #region Info
        public static void Info<T>(string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }
            Info(typeof(T), message);
        }

        public static void Info(Type callingType, string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            LogWriter.Instance.Enqueue(new LogEntity
            {
                CallType = callingType.Name,
                Level = LogLevel.Info,
                Message = message,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Time = DateTime.Now
            });
        }

        #endregion

        #region Debug
        public static void Debug<T>(string message, params object[] args)
        {
            Debug(typeof(T), message, args);
        }

        public static void Debug(Type callingType, string message, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }

            LogWriter.Instance.Enqueue(new LogEntity
            {
                CallType = callingType.Name,
                Level = LogLevel.Debug,
                Message = message,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                Time = DateTime.Now
            });
        }
        #endregion

    }

    internal class LogWriter
    {
        private static readonly LogWriter instance = new LogWriter();

        /// <summary>
        /// 单日志文件最大 5M
        /// </summary>
        private static long singleFilemaxLength = 5 * 1024 * 1024;

        private Queue<LogEntity> queue = null;

        private string baseDir = null;

        private string lastFileName = null;

        public static LogWriter Instance
        {
            get
            {
                return instance;
            }
        }

        private LogWriter()
        {
            baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            queue = new Queue<LogEntity>();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (queue.Count == 0)
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    Flush();
                }

            });
        }

        public void Enqueue(LogEntity log)
        {
            lock (((ICollection)queue).SyncRoot)
            {
                queue.Enqueue(log);
            }

            if (queue.Count > 5000)
            {
                Flush();
            }
        }

        private void Flush()
        {
            if (queue.Count == 0)
            {
                return;
            }

            StringBuilder builder = new StringBuilder();

            lock (((ICollection)queue).SyncRoot)
            {
                foreach (var log in queue)
                {
                    builder.AppendLine(log.ToString());
                }

                queue = new Queue<LogEntity>();
            }

            string filePath = getLogFilePath();
            lastFileName = Path.GetFileName(filePath);

            File.AppendAllText(filePath, builder.ToString(), Encoding.UTF8);
        }

        private string getLogFilePath()
        {
            string filePath = null;
            int fileIndex = 1;


            if (string.IsNullOrEmpty(lastFileName))
            {
                //为空  ，程序首次启动
                filePath = Path.Combine(baseDir, string.Format("log-{0:yyyyMMdd}-{1}.txt", DateTime.Now, fileIndex));

                while (File.Exists(filePath))
                {
                    var fi = new FileInfo(filePath);
                    if (fi.Length < singleFilemaxLength)
                    {
                        break;
                    }
                    fileIndex++;
                    filePath = Path.Combine(baseDir, string.Format("log-{0:yyyyMMdd}-{1}.txt", DateTime.Now, fileIndex));
                }
                return filePath;
            }
            else if (lastFileName.StartsWith(string.Format("log-{0:yyyyMMdd}-", DateTime.Now)) == false)
            {
                //日期已变更
                filePath = Path.Combine(baseDir, string.Format("log-{0:yyyyMMdd}-{1}.txt", DateTime.Now, fileIndex));
                return filePath;
            }
            else
            {
                fileIndex = Convert.ToInt32(lastFileName.Split('-', '.')[2]);
                filePath = Path.Combine(baseDir, string.Format("log-{0:yyyyMMdd}-{1}.txt", DateTime.Now, fileIndex));
                while (File.Exists(filePath))
                {
                    var fi = new FileInfo(filePath);
                    if (fi.Length < singleFilemaxLength)
                    {
                        break;
                    }
                    fileIndex++;
                    filePath = Path.Combine(baseDir, string.Format("log-{0:yyyyMMdd}-{1}.txt", DateTime.Now, fileIndex));
                }
                return filePath;
            }
        }
    }

    internal class LogEntity
    {
        public DateTime Time { get; set; }

        public string CallType { get; set; }

        public int ThreadId { get; set; }

        public LogLevel Level { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss:fff} {1} {2} [Thread{3}] {4}", this.Time, this.Level, this.CallType, this.ThreadId, this.Message);
        }
    }

    internal enum LogLevel
    {
        None = 0,

        Trace,

        Info,

        Debug,

        Warn,

        Exception,

        Fatal
    }
}
