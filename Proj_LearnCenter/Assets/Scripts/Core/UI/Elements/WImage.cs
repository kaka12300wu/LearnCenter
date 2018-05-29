using UnityEngine;
using UnityEngine.UI;
public class WImage : Image
{
    private string m_spName = "";
    public string spriteName
    {
        get { return m_spName; }
        set 
        {
            if (string.IsNullOrEmpty(value))
            {
                m_spName = "";
                enabled = false;
                return;
            }
            
            //sprite = AtlasFactory.instance.GetSpriteByName(value, te);
            
            m_spName = value;
        }
    }


}

