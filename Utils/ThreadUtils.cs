using System;
using System.Windows;

namespace BinaryApp.Services;

public static class ThreadUtils
{
    private static bool IsRunningOnUiThread => Application.Current?.Dispatcher?.CheckAccess() ?? true;

    public static void RunOnUiThread(Action action)
    {
        if (IsRunningOnUiThread)
            action();
        else
            Application.Current.Dispatcher.BeginInvoke(action, null);
    }

    public static void RunOnUiThread<T>(Action<T> action, T argument)
    {
        if (IsRunningOnUiThread)
            action(argument);
        else
            Application.Current.Dispatcher.BeginInvoke(action, argument);
    }
}