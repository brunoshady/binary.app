using System;

namespace BinaryApp.Services;

public class Log
{
    public DateTime DateTime { get; set; }
    public string Message { get; set; }
    public LogType Type { get; set; }
}

public enum LogType
{
    Info,
    Warning,
    Error,
    Debug
}