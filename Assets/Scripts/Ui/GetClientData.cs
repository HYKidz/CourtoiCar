using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.UI;

public class GetClientData : MonoBehaviour
{
    [SerializeField] private string _clientPath = "client_list";

    [SerializeField] private GameObject _clientPanel;

    [SerializeField] private GridLayoutGroup _grid;

    private Dictionary<string, GameObject> _clientActive = new Dictionary<string, GameObject>();

    [SerializeField] private float _minYCellSize;
    [SerializeField] private float _maxYCellSize;
    [SerializeField] private float _vitesseAnim;

    // [SelectionBase] private 

    private float _actualYCellSize;

    void Start()
    {
        _actualYCellSize = _grid.cellSize.y;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Query clientQuery = db.Collection(_clientPath);
        clientQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allClientSnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in allClientSnapshot.Documents)
            {
                    Debug.Log(String.Format("Document Data for {0} document: ", documentSnapshot.Id));
                    Dictionary<string, object> client = documentSnapshot.ToDictionary();
                    if(_clientPanel != null)
                    {
                        GameObject GOClient = Instantiate(_clientPanel, gameObject.transform.position, Quaternion.identity);
                        _clientActive.Add(documentSnapshot.Id, GOClient);
                        GOClient.transform.SetParent(gameObject.transform);
                        GOClient.transform.localScale = Vector3.one;
                        ClientPanel panel = GOClient.GetComponent<ClientPanel>();
                        panel.Nom = client["Nom"].ToString();
                        Debug.Log(client["CarId"]);
                        bool isTaken = (client["CarId"]!= null);
                        Debug.Log(isTaken);
                    }
            }
        });
    }
}
