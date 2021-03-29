using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    //Vars for getting object components
    [SerializeField] private TMP_Text displayNameText = null;
    //[SerializeField] public MeshFilter displayMeshFilter = null;
    private Renderer[] displayColorRenderer;



    //Syncs The int PCName to all other connections and hooks it to a method
    [SyncVar(hook =nameof(HandleDisplayNameUpdated))] 
    [SerializeField] public string PCName = "Missing Name";
    [SerializeField] private string Newname;


    //Syncs The int PCColor to all other connections and hooks it to a method
    [SyncVar(hook =nameof(HandleDisplayColorUpdated))]
    [SerializeField] public Color PCColor;

    //Syncs The int MeshIndex to all other connections and hooks it to a method
    [SyncVar(hook =nameof(HandleDisplayMeshUpdated))]
    [SerializeField] public int MeshIndex;

    #region server
   
    [Server]
    public void SetPCName(string newPCName)
    {
        PCName = newPCName;
    }
    [Server]
    public void SetPCColor(Color newPCColor)
    {               
        PCColor = newPCColor;
    }
    [Server]
    
    public void SetMeshIndex(int newMeshIndex)
    {      
        //newPCMesh = Meshlist[Random.Range(0, Meshlist.Count)];
        //newPCMesh = Meshlist[newPCMesh];
        //Debug.Log(newPCMesh);
        //UsedShapes.Add(newPCMesh);

        //Applies the current newMeshIndex
        MeshIndex = newMeshIndex;
    }
    
    [Command]
    private void CmdSetDisplayName(string newPCName)
    {
        //1. Checks if any of the blacklisted in the new name
        string [] blacklist = {" ","-","."};
        //2. If any name has a blacklisted character return
        foreach(string word in blacklist)
        {
            if (newPCName.Contains(word))
            return;
        }
        if (newPCName.Length > 12 | newPCName.Length < 2){ return; }

        //if (newPCName.Contains(" ")) { return; }
        
        RpcSetName(newPCName);

        SetPCName(newPCName);
    }

    #endregion

    #region client
    private void Awake()
    {
        //Sets the DisplayColorRenderer to all children Rendrers
        displayColorRenderer = GetComponentsInChildren<Renderer>(true);
    }
    private void HandleDisplayMeshUpdated(int oldIndex, int newIndex)
    {
        //displayMeshFilter.mesh = newMesh;
        //if(PCShape = Destroy(true))

        //displayColorRenderer = newMesh.GetComponent<Renderer>();
        //HandleDisplayColorUpdated(PCColor ,PCColor);
        //newMesh.transform.SetParent(transform);

        //Deativate And Activate required Mesh from index
        transform.Find("Mesh").GetChild(oldIndex).gameObject.SetActive(false);
        transform.Find("Mesh").GetChild(newIndex).gameObject.SetActive(true);
    }
    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        //displayColorRenderer.material.SetColor("_BaseColor", newColor);

        //Set Color to all Player child Renderers
        foreach (var Renderer in displayColorRenderer)
            Renderer.material.SetColor("_BaseColor", newColor);
        
    }
    private void HandleDisplayNameUpdated(string oldName, string newName)
    {
        displayNameText.SetText(newName);
    }
    [ContextMenu("Set my name")]
    private void SetMyName()
    {
        CmdSetDisplayName(Newname);
    }
    [ClientRpc]
    private void RpcSetName(string newPCName)
    {
        Debug.Log(newPCName);
    }
    #endregion
}
