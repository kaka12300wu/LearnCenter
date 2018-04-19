using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataBase : ISingletonInit
{
    public PlayerData curPlayer;

    List<PlayerData> playerList;
    string pathData;
    readonly string dataFileExt = ".dno";

    public enum DataType
    {
        Player,
        Item,      //ŒÔ∆∑
        Max
    }

    float markTm;
    static readonly float saveFrequency = 5.0f;
    bool waitSave;

    void MarkSave()
    {
        waitSave = true;
        markTm = Time.realtimeSinceStartup;
    }

    void SaveToFile()
    {
        waitSave = false;
        //SingletonObject.getInstance<FileHelper>().SaveFile();
    }

    void LoadAllGuidPlayers()
    {
        playerList = new List<PlayerData>();
        string[] dirs = Directory.GetDirectories(pathData, "", SearchOption.TopDirectoryOnly);
        string playerFileName = DataType.Player.ToString() + dataFileExt;
        byte[] buffer;
        FileHelper fileHelper = SingletonObject.getInstance<FileHelper>();
        for (int i = 0, max = dirs.Length; i < max; ++i)
        {
            string filePlayer = Path.Combine(pathData, string.Format("{0}/{1}",dirs[i],playerFileName));
            if(File.Exists(filePlayer))
            {
                buffer = fileHelper.ReadFile(filePlayer);
                PlayerData player = ProtoSerializer.ProtoDeSerialize<PlayerData>(buffer);
                if(null != player)
                {
                    playerList.Add(player);
                }
            }
        }
        if(playerList.Count > 1)
        {
            playerList.Sort((left, right) => { return (int)(left.lastLogin - right.lastLogin); });
        }
        if(playerList.Count > 0)
            curPlayer = playerList[playerList.Count - 1];
    }

    void InitAllDataBase(Guid guid)
    {

    }

    public void AddPlayer(PlayerData player)
    {
        InitAllDataBase(player.guid);
    }

    public void SetCurPlayer(PlayerData player)
    {
        if (null != player && null != playerList.Find((elem) => elem.guid == player.guid))
        {
            curPlayer = player;
        }
    }

    public void Update()
    {
        if (waitSave && Time.realtimeSinceStartup - markTm > saveFrequency)
        {
            SaveToFile();
        }
    }

    public List<PlayerData> GetPlayList()
    {
        return new List<PlayerData>(playerList.ToArray());
    }

    public void Init()
    {
        pathData = Application.persistentDataPath + "/Data";
        LoadAllGuidPlayers();
        waitSave = false;
    }

    public void Dispose()
    {
        SaveToFile();
    }
}