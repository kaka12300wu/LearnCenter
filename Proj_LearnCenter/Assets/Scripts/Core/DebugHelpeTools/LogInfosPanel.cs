namespace ZDebug
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    public class LogInfosPanel : MonoBehaviour
    {
        List<GLog.LogItem> srcLogInfos;
        List<GLog.LogItem> showingLogs;
        
        public Image imgLog;
        public Image imgWarn;
        public Image imgError;
        bool showLog = true;
        bool showWarn = true;
        bool showError = true;
        Color color_on = new Color(115 / 255.0f, 91 / 255.0f, 242 / 255.0f, 255 / 255.0f);
        Color color_off = new Color(174 / 255.0f, 174 / 255.0f, 174 / 255.0f, 255 / 255.0f);

        void OnEnable()
        {
            SetImageColor(imgLog,showLog);
            SetImageColor(imgWarn, showWarn);
            SetImageColor(imgError, showError);
            GLog.OnNewLogAdd += OnNewLogInfo;
            srcLogInfos = GLog.GetLogInfosList();
            FilterLogs();
        }

        void FilterLogs()
        {
            if(null == showingLogs)
                showingLogs = new List<GLog.LogItem>();
            else
                showingLogs.Clear();
            for(int i = 0,max = srcLogInfos.Count;i<max;++i)
            {
                GLog.LogItem item = srcLogInfos[i];
                if (item.logType == LogType.Log && showLog)
                    showingLogs.Add(item);
                else if (item.logType == LogType.Warning && showWarn)
                    showingLogs.Add(item);
                else if (item.logType == LogType.Error && showError)
                    showingLogs.Add(item);
            }

        }

        void SetImageColor(Image img,bool on)
        {
            if (null != img)
                img.color = on ? color_on : color_off;
        }

        void OnDisable()
        {
            GLog.OnNewLogAdd -= OnNewLogInfo;
        }

        void OnNewLogInfo(GLog.LogItem logItem)
        {

        }

        public void OnSelectLogType(int index)
        {
            if(index == 1)
            {
                showLog = !showLog;
                SetImageColor(imgLog, showLog);
            }
            else if(index == 2)
            {
                showWarn = !showWarn;
                SetImageColor(imgWarn, showWarn);
            }
            else if(index == 3)
            {
                showError = !showError;
                SetImageColor(imgError, showError);
            }
            FilterLogs();
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}