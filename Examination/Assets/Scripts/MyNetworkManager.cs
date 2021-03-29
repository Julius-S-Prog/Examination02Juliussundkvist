using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    //[SerializeField] public List<Mesh> MeshList = new List<Mesh>();
    [SerializeField] public List<int> IndexList = new List<int> {0, 1, 2, 3};
    //[SerializeField] public List<Mesh> UsedShapes = new List<Mesh>();
    //[SerializeField] public Mesh PCShape;


    //[SerializeField] private List<GameObject> ShapeList = new List<GameObject>();
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        MyNetworkPlayer player = conn.identity.GetComponent<MyNetworkPlayer>();

        
        player.SetPCName($"Player {numPlayers}");
        player.SetPCColor(Random.ColorHSV());
        //player.SetPCShape(ShapeData(conn));
        player.SetMeshIndex(GetIndex());
        
  
        Debug.Log($"{player.PCName} Connected to server | {numPlayers} Current Player(s)");
    }
    
    GameObject ShapeData(NetworkConnection conn)
    {
        //1. Shape Left check
        int max = spawnPrefabs.Count;
        if (max == 0) return null;

        //2. Limit Player Shape cap to 1
        var i = spawnPrefabs[Random.Range(0, spawnPrefabs.Count)];
        if (i) spawnPrefabs.Remove(i);

        //3. Spawn GameObject for both host and client
        GameObject obj = Instantiate(i);
        NetworkServer.Spawn(obj, conn);

        return obj;


    }    
    int GetIndex()
    {
        //1. SETS MAX AMOUNT OF SHAPES
        int MaxShapes = IndexList.Count;
        if (MaxShapes == 0) { Debug.Log(MaxShapes); }

        //2. GET RANDOM INT FROM 0 TO LIST LENGTH
        int IndexReq = IndexList[Random.Range(0, MaxShapes)];

        //3. ON LIST REQUEST REMOVE REQUESTED
        IndexList.Remove(IndexReq);
        return IndexReq;
    }
}
