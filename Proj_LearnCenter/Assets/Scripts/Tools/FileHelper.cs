using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FileHelper
{
    public byte[] ReadFile(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
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
            EnsureDirectory(path);
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

    bool EnsureDirectory(string path)
    {
        try
        {
            path.Replace("\\","/");
            if (path.Contains("."))
            {
                path = path.Substring(0, path.LastIndexOf("/") + 1);
            }
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return true;
        }
        catch(Exception e)
        {
            GLog.LogError(e.ToString());
        }
        return false;
    }
}
