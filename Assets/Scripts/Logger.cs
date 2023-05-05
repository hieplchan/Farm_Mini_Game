using System;

public class Logger
{
    private static Logger _instance;
    public event Action<string> NewLog;

    public static Logger Instance 
    {
        get
        {
            // not thread-safe, can improve with more time
            if (_instance == null)
            {
                _instance = new Logger();
            }
            return _instance;
        }
    }

    public void Log(string text)
    {
        Instance.NewLog?.Invoke(text);
    }
}
