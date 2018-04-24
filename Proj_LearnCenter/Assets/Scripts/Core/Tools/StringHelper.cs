using System;
using System.Collections.Generic;
using System.Text;

public static class StringHelper
{
    public static string PathFormat(this string path)
    {
        return path.Replace("\\","/");
    }

    public static string CombinePath(this string pathPre,string pathNext)
    {
        pathPre = pathPre.PathFormat();
        pathNext = pathNext.PathFormat();
        if (string.IsNullOrEmpty(pathPre))
            return pathNext;
        if (string.IsNullOrEmpty(pathNext))
            return pathPre;
        if (!pathPre.EndsWith("/") && !pathNext.StartsWith("/"))
            return string.Format("{0}/{1}",pathPre,pathNext);
        else if (pathPre.EndsWith("/") && pathNext.StartsWith("/"))
        {
            return pathPre + pathNext.Substring(1);
        }
        else
        {
            return pathPre + pathNext;
        }        
    }

    public static string Concat(this string[] paramStrs,string str)
    {
        if (paramStrs.Length == 1)
            return paramStrs[0];

        StringBuilder sbuilder = new StringBuilder();
        for(int i = 0,max = paramStrs.Length;i<max;++i)
        {
            sbuilder.Append(paramStrs[i]);
            if(i < max - 1)
            {
                sbuilder.Append(str);
            }
        }
        return sbuilder.ToString();
    }

    public static string Concat(this List<string> listStr,string str)
    {
        return listStr.ToArray().Concat(str);
    }


}
