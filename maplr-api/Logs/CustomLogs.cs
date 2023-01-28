namespace maplr_api.Logs
{
    public class CustomLogs
    {
        public static void SaveLog(string method, string message, string stackTrace)
        {
            var now = DateTime.Now.ToString("G"); // Format dd/MM/yyyy hh:mm:ss
            Console.WriteLine($"{now} - {method} - {message} - {stackTrace}");
        }
    }
}
