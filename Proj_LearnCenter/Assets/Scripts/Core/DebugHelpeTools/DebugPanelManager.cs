namespace ZDebug
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class DebugPanelManager : MonoBehaviour
    {

        #region UnityLife calls
        private void Awake()
        {
            InitLayoutCorners();
        }
        #endregion

        #region 处理切换菜单显示位置相关
        List<TextAnchor> canLayoutCorners;
        int nowIndex = 0;
        public LayoutGroup layout;
        void InitLayoutCorners()
        {
            canLayoutCorners = new List<TextAnchor>();
            canLayoutCorners.Add(TextAnchor.UpperLeft);
            canLayoutCorners.Add(TextAnchor.UpperRight);
            canLayoutCorners.Add(TextAnchor.LowerLeft);
            canLayoutCorners.Add(TextAnchor.LowerRight);
        }

        public void SwitchPosition()
        {
            ++nowIndex;
            if (nowIndex >= canLayoutCorners.Count)
                nowIndex = 0;
            layout.childAlignment = canLayoutCorners[nowIndex];
        }
        #endregion

        #region 处理Log点击相关
        public void WriteLogToFile()
        {
            SingletonObject.getInstance<GLog>().WriteLogToFile();
        }

        public LogInfosPanel logInfosPanel;
        public void ShowLogInfos()
        {
            if(null != logInfosPanel)
            {
                logInfosPanel.gameObject.SetActive(true);          
            }
        }


        #endregion

    }
}