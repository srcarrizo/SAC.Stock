namespace SAC.Stock.Front.InitializeDatabase.Infrastructure
{
    using SAC.Seed.Logging;
    using System;
    using System.Diagnostics;

    internal abstract class ImportProcess
    {
        private ILogger logger;


        private Stopwatch watch;


        private string scope;

        public void Run(params object[] args)
        {
            this.logger = LoggerFactory.CreateLogger();
            this.watch = new Stopwatch();
            this.scope = this.GetType().Name;

            try
            {
                this.Log(@">>>>>Init process...");
                this.watch.Start();

                this.RunCommand(args);

                this.watch.Stop();
                this.Lap(@"<<<<<End process...");
            }
            catch (Exception ex)
            {
                this.watch.Stop();
                this.Log(string.Format("///// ERROR [{0}] [{1}]", this.watch.Elapsed, ex.Message));
                throw;
            }
        }


        protected abstract void RunCommand(object[] args);

        protected void Log(string text)
        {
            this.logger.Info(string.Format(@"[{0}] {1}", this.scope, text));
        }

        protected void Lap(string text)
        {
            var elapsed = this.watch.Elapsed;
            this.Log(string.Format("++Global Lap: {0} [{1}]", text, elapsed));
        }

        protected void Lap(string text, TimeSpan mark)
        {
            var elapsed = this.watch.Elapsed.Subtract(mark);
            this.Lap(text);
            this.Log(string.Format("++Custom Lap: {0} [{1}] {2}", text, elapsed, elapsed.Seconds > 1 ? "!!!!!" : string.Empty));
        }

        protected TimeSpan GetLapMark()
        {
            return this.watch.Elapsed;
        }
    }
}
