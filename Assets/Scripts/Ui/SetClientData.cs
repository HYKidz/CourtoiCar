using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetClientData : MonoBehaviour
{
    [SerializeField] private string _clientPath = "client_list";
    [SerializeField] private TMP_InputField _nomField;
    List<string> _option = new List<string>();
    [SerializeField] private TMP_Dropdown _listClient;
    private FirebaseFirestore db;
    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        Query clientQuery = db.Collection(_clientPath);
        clientQuery.GetSnapshotAsync().ContinueWithOnMainThread(task=>
        {
            QuerySnapshot allClientSnapshot = task.Result;
            foreach (DocumentSnapshot allClient in allClientSnapshot.Documents)
            {
                Debug.Log(String.Format("Document Data for {0} document:",allClient.Id));
                Dictionary<string,object> client = allClient.ToDictionary();
                // _listClient.AddOptions(client["Nom"]); 
                // Debug.Log(client["Nom"].ToString());
                _option.Add(client["Nom"].ToString());   
            }
            ActivateOption();
        });
        // Debug.Log(_option);
    }
    private void ActivateOption()
    {

        _listClient.AddOptions(_option);
        // foreach (string item in _option)
        // {
        // // _listClient.AddOptions(_option);
        //     Debug.Log(item);
        // }

    }

    public async void Click()
    {
        // Dictionary<string, object> client = new Dictionary<string, object>
        // {
        //     {"Nom", _nomField.text},
        //     //Ajouter quel vehicule a ete attribuer
        // };
        // DocumentReference AddDocRef = await db.Collection(_clientPath).AddAsync(client);
        // SceneManager.LoadScene("List");

        ///Test///
        foreach (string item in _option)
        {
        // _listClient.AddOptions(_option);
            Debug.Log(item);
        }
    }
}
