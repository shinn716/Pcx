using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadFileFromPath : MonoBehaviour
{
    #region Path
    public enum ReadPath
    {
        persistentDataPath,
        streamingAssetsPath,
        dataPath,
        temporaryCachePath,
    }

    public string PathSelect(ReadPath pathstatus)
    {
        switch (pathstatus)
        {
            default:
                return Application.persistentDataPath;
            case ReadPath.persistentDataPath:
                return Application.persistentDataPath;
            case ReadPath.streamingAssetsPath:
                return Application.streamingAssetsPath;
            case ReadPath.dataPath:
                return Application.dataPath;
            case ReadPath.temporaryCachePath:
                return Application.temporaryCachePath;
        }
    }
    #endregion

    #region Extenstion
    public enum ReadExtension
    {
        OBJ,
        FBX,
        PLY,

        JPG,
        PNG,

        MP3,
        MP4,

        TXT
    }

    public string ExtensionSelect(ReadExtension extensionStatus)
    {
        switch (extensionStatus)
        {
            default:
                return ".obj";

            case ReadExtension.OBJ:
                return ".obj";
            case ReadExtension.FBX:
                return ".fbx";
            case ReadExtension.PLY:
                return ".ply";

            case ReadExtension.JPG:
                return ".jpg";
            case ReadExtension.PNG:
                return ".png";

            case ReadExtension.MP3:
                return ".mp3";
            case ReadExtension.MP4:
                return ".mp4";

            case ReadExtension.TXT:
                return ".txt";
        }
    }
    #endregion

    public ReadPath readpath = ReadPath.persistentDataPath;
    public ReadExtension readextension = ReadExtension.OBJ;

    public List<string> names = new List<string>();
    public List<string> GetNames { get { return names; } }

    private List<string> contents = new List<string>();
    public List<string> GetContents { get { return contents; } }

    public int loadindex = 0;

    private void Start()
    {
        LoadFiles();
    }


    #region Debug Function

    [ContextMenu("LoadPly")]
    public void LoadPly()
    {
        if (loadindex<names.Count)
        { var go = LoadPlyMeshData.ImportToScene(names[loadindex]);
            go.transform.parent = transform;
            StartCoroutine(SetDefault(go));
        }
    }

    [ContextMenu("RemoveModel")]
    public void RemoveModel()
    {
        var findGO = GameObject.FindWithTag("ply");
        if (findGO != null)
            Destroy(findGO.gameObject);
    }

   

    [ContextMenu("RemoveAllFiles")]
    public void RemoveAllFiles()
    {
        DirectoryInfo dir = new DirectoryInfo(PathSelect(readpath));
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            File.Delete(f.ToString());
            Debug.Log("Delet files: " + f.Name);
        }

        contents.Clear();
        names.Clear();
    }
    #endregion


    #region private Function
    private void LoadFiles()
    {
        DirectoryInfo dir = new DirectoryInfo(PathSelect(readpath));
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            if (Path.GetExtension(f.Name) == ExtensionSelect(readextension))
            {
                Debug.Log("Read files name: " + f.FullName);
                names.Add(f.FullName);
                contents.Add(LoadTxt(f.FullName));
            }
        }
    }

    private IEnumerator SetDefault(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        go.tag = "ply";
    }

    private string LoadTxt(string path)
    {
        using (StreamReader r = new StreamReader(path))
        {
            string myobj = r.ReadToEnd();
            return myobj;
        }
    }
    #endregion


}