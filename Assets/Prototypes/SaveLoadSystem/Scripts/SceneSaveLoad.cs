using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SceneSaveLoad : MonoBehaviour
{
    public static SceneSaveLoad Instance;

    [SerializeField] private int spawnCount = 10;

    string savePath;

    private void Awake()
    {
        Instance = this;
        savePath = Path.Combine(Application.persistentDataPath + "/Saveables.sab");
    }


    public void CreateRandom() 
    {
        for (int i  = 0; i < spawnCount; i++) 
        {
            CreatePrimitiveSaveable(null);
        }
    }



    private void CreatePrimitiveSaveable(GenericObjectData data) 
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = data == null ? RGHelper.RandomPositionInSphere(Vector3.zero, 3.5f, 1f) : data.transformData[0].toVector3();
        obj.transform.rotation = data == null ? UnityEngine.Random.rotation : Quaternion.Euler(data.transformData[1].toVector3());
        obj.transform.SetParent(this.transform, true);
        Saveable svScript = obj.AddComponent<SaveableCube>();
        var renderer = obj.GetComponent<MeshRenderer>();
        renderer.sharedMaterial = GameObject.Instantiate(renderer.sharedMaterial);
        renderer.sharedMaterial.color = data == null ? Random.ColorHSV() : new Color(data.materialColor[0], data.materialColor[1], data.materialColor[2], data.materialColor[3]);
        SavedObjectScript objScript = obj.AddComponent<SavedObjectScript>();

        if (data != null && data.GetType() == typeof(SpiningCubeData))
            objScript.spin = data == null ? Random.value * 2f : ((SpiningCubeData)data).spin;
        else
            objScript.spin = Random.value * 2f;

    }


    public void WipeAll() 
    {
        for (int i = transform.childCount -1; i >= 0; i--) 
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void ClearSaveFile() 
    {
        File.Delete(savePath);
    }

    public void SaveSaveable(Saveable saveableObject) 
    {
        using (var w = new StreamWriter(savePath, true)) 
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, saveableObject.data);
            string str = System.Convert.ToBase64String(memoryStream.ToArray());

            w.WriteLine(str);
        }

    }

    public void LoadSaveable() 
    {
        using (var w = new StreamReader(savePath, true))
        {
            while (!w.EndOfStream) 
            {
                string objectStream = w.ReadLine();
                byte[] memorydata = System.Convert.FromBase64String(objectStream);
                using (MemoryStream rs = new MemoryStream(memorydata)) 
                {
                    BinaryFormatter sf = new BinaryFormatter();
                    GenericObjectData saveableObject = (GenericObjectData)sf.Deserialize(rs);

                    CreatePrimitiveSaveable(saveableObject);
                }
            }
        }
    }

}
