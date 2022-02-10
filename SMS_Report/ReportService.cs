using NLog;
using SMS_Report.Repositories;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace SMS_Report
{
    public partial class ReportService : ServiceBase
    {

        private const int MinuteInMilliseconds = 60000;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private int _intervalInMinutes;
        private Timer _timer;

        private ErrorRepository _errorRepository = new ErrorRepository();
        private SmsGenerator _smsGenerator = new SmsGenerator();
        private SmsSender _smsSender = new SmsSender();

        public ReportService()
        {
            InitializeComponent();

            SetVariables();
            CheckDatabaseConnection();
            CheckSmsSending();
        }
        private void SetVariables()
        {
            try
            {
                _intervalInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInMinutes"]);
                _timer = new Timer(_intervalInMinutes * MinuteInMilliseconds);
                Logger.Info("Variables are OK");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to set variables");
                throw new Exception("Failed to set variables");
            }

        }
        private void CheckDatabaseConnection()
        {
            var connectionString = ConnectionStringGenerator.GenerateConnectionString().ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    connection.Close();
                    Logger.Info("Database connection is OK");
                }
                catch (SqlException ex)
                {
                    Logger.Error(ex, ex.Message);
                    throw new Exception(ex.Message);
                }
            }
        }
        private void CheckSmsSending()
        {
            var isSuccess = _smsSender.Send("test_message", true);

            if (isSuccess)
                Logger.Info("GSMService connection is OK");
            else
            {
                Logger.Error("Failed to connect to GSMService");
                throw new Exception("Failed to connect to GSMService");
            }
        }


        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();
            Logger.Info("Service started...");
        }

        protected override void OnStop()
        {
            Logger.Info("Service stopped...");
        }


        private void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                SendError();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void SendError()
        {
            var errors = _errorRepository.GetNotSentErrorsFromDatabase();
            if(!errors.Any())
            {
                Logger.Info("No new errors to send");
                return;
            }

            var message = _smsGenerator.GenerateSms(errors, _intervalInMinutes);
            var isSuccess = _smsSender.Send(message, false);
            if (isSuccess)
            {
                var errorCount = errors.Count();
                Logger.Info($"Errors sent ({errorCount})");
                _errorRepository.UpdateErrorStatus(errors);
                Logger.Info($"Database records updated ({errorCount})");
            }                
        }
    }
}
