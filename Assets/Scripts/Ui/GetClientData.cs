using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Firebase.Firestore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetClientData : MonoBehaviour
{
    [SerializeField] private string _clientPath = "client_list";
    [SerializeField] private string _carPath = "car_list";

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
                        // Debug.Log(client["CarId"]);
                        bool isTaken = client.ContainsKey("CarId");
                        if(isTaken) 
                        {
                            DocumentReference docRef = db.Collection(_carPath).Document(client["CarId"].ToString());
                            docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                            {
                                DocumentSnapshot snapshot = task.Result;
                                if(snapshot.Exists)
                                {
                                    Dictionary<string,object> car = snapshot.ToDictionary();
                                    panel.Anne = car["Anne"].ToString();
                                    panel.Info = car["Info"].ToString();
                                    panel.Serie = car["Serie"].ToString();
                                    panel.Plaque = car["Plaque"].ToString();
                                    var timestamp = (Timestamp) client["TimeGet"];
                                    panel.TimeGet = timestamp;
                                    var maDate = timestamp.ToDateTime();
                                    // DateTime result = DateTime.ParseExact(client["TimeGet"].ToString().Replace("Timest"))
                                    panel.DateEmprunt = maDate.ToString();
                                    panel._buttonRetour.SetActive(true);
                                    panel.ID = documentSnapshot.Id;
                                    panel.CarId = client["CarId"].ToString();
                                    Debug.Log(maDate);
                                    // DateTime timeGet = DateTime.TryParse(client["TimeGet"]);
                            }});
                        }
                        // GetCarInfo();
                        Debug.Log(isTaken);
                    }
            }
        });
    }
    // private DateTime ToDateTime ( DateTime value)
    // {
    //     return value.ToString("yyyyMMddHHmmssfff");
    // }
    // private async GetCarInfo()
    // {

    // }
}
