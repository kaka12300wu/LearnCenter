using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FileHelper
{
    public byte[] ReadFile(string path)
    {
        if (!File.Exists(path))
            return default(byte[]);
        using (FileStream fStream = new FileStream(path, FileMode.Open))
        {
            fStream.Position = 0;
            byte[] buffer = new byte[fStream.Length];
            fStream.Read(buffer,0,buffer.Length);
            fStream.Close();
            return buffer;
        };
    }

    public bool SaveFile(string path, byte[] buffer)
    {
        try
        {
            using (FileStream fStream = new FileStream(path, FileMode.Create))
            {
                fStream.Write(buffer,0,buffer.Length);
                fStream.Close();
                return true;
            };
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }
    }
}
