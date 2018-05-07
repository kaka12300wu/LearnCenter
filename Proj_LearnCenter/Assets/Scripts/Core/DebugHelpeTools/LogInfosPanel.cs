namespace ZDebug
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class LogInfosPanel : MonoBehaviour
    {
        List<GLog.LogItem> srcLogInfos;
        List<GLog.LogItem> showingLogs;

        //public ScrollList scroll;
        public Image imgLog;
        public Image imgWarn;
        public Image imgError;
        bool showLog = true;
        bool showWarn = true;
        bool showError = true;
        Color color_on = new Color(115 / 255.0f, 91 / 255.0f, 242 / 255.0f, 255 / 255.0f);
        Color color_off = new Color(174 / 255.0f, 174 / 255.0f, 174 / 255.0f, 255 / 255.0f);
        Color color_info = Color.white;
        Color color_warn = new Color(255 / 255.0f, 228 / 255.0f, 8 / 255.0f, 255 / 255.0f);
        Color color_error = new Color(255 / 255.0f, 32 / 255.0f, 32 / 255.0f, 255 / 255.0f);

        void OnEnable()
        {
            SetImageColor(imgLog,showLog);
            SetImageColor(imgWarn, showWarn);
            SetImageColor(imgError, showError);
            GLog.OnNewLogAdd += OnNewLogInfo;
            srcLogInfos = GLog.GetLogInfosList();
            FilterLogs();
        }

        private void Awake()
        {
            //if (null != scroll)
            //    scroll.renderFunc = FillLogItem;
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
            //scroll.SetData(showingLogs.ConvertAll((src)=> { return src as object; }));
            //scroll.Refresh(showingLogs.Count - 1);
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

        void FillLogItem(object data,UIReferences refer)
        {
            GLog.LogItem item = data as GLog.LogItem;
            Text txt = refer.Get("Text") as Text;
            Image img = refer.Get(1) as Image;
            Button btn = refer.Get(2) as Button;

            txt.text = item.ToString();
            img.enabled = showingLogs.IndexOf(item) % 2 == 0;
            btn.AddClickEvent(()=> {
                
            });
            if (item.logType == LogType.Log)
                txt.color = color_info;
            else if (item.logType == LogType.Warning)
                txt.color = color_warn;
            else if (item.logType == LogType.Error)
                txt.color = color_error;
            else
                txt.color = color_info;
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