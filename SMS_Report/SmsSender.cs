using Cipher;
using SMS_Report.GSMServiceAPI;


namespace SMS_Report
{
    internal class SmsSender
    {
        private string _login = StringCipher.DecryptData("GSMServiceLogin");
        private string _password = StringCipher.DecryptData("GSMServicePassword");
        private string _recipientNumber = StringCipher.DecryptData("GSMServoceRecipientNumber");
        private string _senderName = StringCipher.DecryptData("GSMServiceSenderName");

        public bool Send(string message, bool isTest)
        {
            GSMServicePortTypeClient webService = new GSMServicePortTypeClient();

            Account account = new Account()
            {
                login = _login,
                pass = _password
            };

            Message[] messages = new Message[1];
            messages[0] = new Message()
            {
                recipients = new string[] { _recipientNumber },
                message = message,
                sender = _senderName,
                msgType = 1,
                unicode = true,
                sandbox = isTest
            };

            SendSMSReturn[] sendResult = webService.SendSMS(account, messages);

            return sendResult[0].status == "OK";
        }
    }
}
