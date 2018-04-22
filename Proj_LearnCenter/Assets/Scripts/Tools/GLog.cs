//#define PC_CODE
#define DEBUG_VERSION
#define LOG_ON
#if !UNITY_EDITOR || PC_CODE
#if UNITY_IOS || UNITY_ANDROID || PC_CODE
#define UNITY_MOBILE
#define LOG_TO_FILE
#endif
#endif

using UnityEngine;
using System;
using System.Collections.Generic;
#if LOG_TO_FILE
using System.IO;
using System.Text;
#endif

public class GLog : MonoBehaviour
{
#if LOG_ON 
    public class LogItem
    {
        public string msg;
        public string stackTrace;
        public LogType logType;
        public DateTime tmLog;
        
        string GetMark(bool hasColor = true)
        {
            string mark = "[I]";
            if (logType == LogType.Warning)
                mark = hasColor ? "<color=yellow>[W]</color>" : "[W]";
            else if (logType == LogType.Error)
                mark = hasColor ? "<color=red>[E]</color>" : "[E]";
            return mark;
        }

        //public string ToGUIString()
        //{
        //    return string.Format("{0}-{1}:{2}", tmLog.ToString("HH:mm:ss"), GetMark(), msg);
        //}

        public string ToFileString()
        {
            return string.Format("{0}-{1}:{2}\r\n{3}\r\n", tmLog.ToString("HH:mm:ss"),GetMark(false),msg,stackTrace);
        }
    }

    static List<LogItem> logList;
    static bool b_init = false;
    static string logFilePath;

    void Awake()
    {
        SingletonObject.getInstance<GLog>(this);
#if UNITY_MOBILE
        Application.logMessageReceived += (string condition, string stackTrace, LogType type) =>
        {
            string msg = string.Format("{0}\n{1}", condition, stackTrace);
            if (type == LogType.Log)
                Log(msg);
            else if (type == LogType.Warning)
                LogWarning(msg);
            else if (type == LogType.Error)
                LogError(msg);
        };
#endif
    }
    
    public static void Init()
    {
        if (b_init)
            return;
#if DEBUG_VERSION
        GameObject o = Resources.Load<GameObject>("DebugHelper");
        GameObject.Instantiate<GameObject>(o).name = o.name;
#endif
        logList = new List<LogItem>();
#if LOG_TO_FILE
        string dirLog = Path.Combine(Application.persistentDataPath,"Log");
        if (!Directory.Exists(dirLog))
            Directory.CreateDirectory(dirLog);
        logFilePath = Path.Combine(dirLog, "log.txt");
        FileStream f_stream = File.Open(logFilePath,FileMode.Create);
        f_stream.Close();
#endif
        b_init = true;
    }

    static string GetStackTraceModelName()
    {
        //当前堆栈信息
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
        System.Diagnostics.StackFrame[] sfs = st.GetFrames();
        //过滤的方法名称,以下方法将不会出现在返回的方法调用列表中
        string _fullName = string.Empty;
        string _fileName = string.Empty;
        string _methodName = string.Empty;
        string _fileInfo = string.Empty;
        int line = 0;
        for (int i = sfs.Length - 1; i > 1; --i)
        {
            //非用户代码,系统方法及后面的都是系统调用，不获取用户代码调用结束
            if (System.Diagnostics.StackFrame.OFFSET_UNKNOWN == sfs[i].GetILOffset()) break;
            _fileName = sfs[i].GetFileName().Replace("\\","/").Replace(Application.dataPath,"Assets");
            _methodName = sfs[i].GetMethod().ToString();//方法名称
            line = sfs[i].GetFileLineNumber();
            _fileInfo = string.Format("{0} {1} line:{2}\r\n",_fileName,_methodName,line);
            _fullName = _fileInfo + _fullName;
        }
        _fullName = string.Format("(StackTrace: At\t{0})", _fullName.TrimEnd('\n','\r'));
        st = null;
        sfs = null;
        return _fullName;
    }

    static void AddLogItem(LogItem item)
    {
        logList.Add(item);
        if (logList.Count >= 1000)
            logList.RemoveAt(0);
    }

    public static void Log(string msg)
    {
#if UNITY_MOBILE
        LogItem item = new LogItem();
        item.msg = msg;
        item.tmLog = DateTime.Now;
        item.logType = LogType.Log;
        item.stackTrace = GetStackTraceModelName();
        AddLogItem(item);
#else
        Debug.Log(msg);
#endif
    }

    public static void LogWarning(string msg)
    {
#if UNITY_MOBILE
        LogItem item = new LogItem();
        item.msg = msg;
        item.tmLog = DateTime.Now;
        item.logType = LogType.Warning;
        item.stackTrace = GetStackTraceModelName();
        AddLogItem(item);
#else
        Debug.LogWarning(msg);
#endif
    }

    public static void LogError(string msg)
    {
#if UNITY_MOBILE
        LogItem item = new LogItem();
        item.msg = msg;
        item.tmLog = DateTime.Now;
        item.logType = LogType.Error;
        item.stackTrace = GetStackTraceModelName();
        AddLogItem(item);
#else
        Debug.LogError(msg);
#endif
    }

    public void WriteLogToFile()
    {
#if UNITY_MOBILE && DEBUG_VERSION
        StringBuilder sbuilder = new StringBuilder();
        for(int i = 0,max = logList.Count;i<max;++i)
        {
            sbuilder.Append(logList[i].ToFileString() + "\r\n");
        }
        File.AppendAllText(logFilePath,sbuilder.ToString(),Encoding.UTF8);
#else
        Debug.Log("只有移动端可以写入日志到文件！");
#endif
    }

#endif
}
