using System;


namespace Tess.Common
{
    static class functions
    {
        public static string idiotMessage()
        {
            var messages = Traduzioni.Loading_message;
            string[] messageList = messages.Split('|');
            var rnd = new Random();
            var message = messageList[rnd.Next(0, messageList.Length)];

            return message;
        }
    }
}
