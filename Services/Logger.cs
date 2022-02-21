using System;
using System.Collections.ObjectModel;

namespace BinaryApp.Services;

public class Logger
{
    private static Logger _instance;

    public ObservableCollection<Log> LogCollection { get; } = new();

    private Logger() 
    { }

    public static Logger GetInstance()
    {
        if (_instance == null)
            _instance = new Logger();

        return _instance;
    }

    public void AddLog(string message, LogType logType = LogType.Info)
    {
        var newLog = new Log
        {
            DateTime = DateTime.Now,
            Message = message,
            Type = logType
        };

        LogCollection.Add(newLog);
    }
}