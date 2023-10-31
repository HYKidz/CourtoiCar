using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetClientData : MonoBehaviour
{
    [SerializeField] private string _clientPath = "client_list";
    [SerializeField] private string _carPath = "car_list";
    [SerializeField] private TMP_InputField _nomField;
    List<string> _option = new List<string>();
    List<string> _idClient = new List<string>();
    [SerializeField] private TMP_Dropdown _listClient;
    private FirebaseFirestore db;
    private string _idCar;
    public string IdCar { get => _idCar; set => _idCar = value; }
    // Start is called before the first frame update
    void Start()
    {
        // _nomField.onValueChanged.AddListener();
        db = FirebaseFirestore.DefaultInstance;
        Query clientQuery = db.Collection(_clientPath);
        clientQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot allClientSnapshot = task.Result;
            foreach (DocumentSnapshot allClient in allClientSnapshot.Documents)
            {
                Debug.Log(String.Format("Document Data for {0} document:", allClient.Id));
                Dictionary<string, object> client = allClient.ToDictionary();
                // _listClient.AddOptions(client["Nom"]); 
                // Debug.Log(client["Nom"].ToString());
                _idClient.Add(String.Format("{0}", allClient.Id));
                _option.Add(client["Nom"].ToString());
            }
            ActivateOption();
        });
    }
    public void HandleOptionActive()
    {
        if(_nomField.text !="") _listClient.gameObject.SetActive(false);
        else _listClient.gameObject.SetActive(true);
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
        //work for now, but potentially change car Id to include a bigger object containing timestamp to get the time in and out 
        if (_listClient.value == 0 && _nomField.text!="")
        {

            Dictionary<string, object> client = new Dictionary<string, object>
        {
            {"Nom", _nomField.text},
            {"CarId", _idCar}
        };
            DocumentReference AddDocRef = await db.Collection(_clientPath).AddAsync(client);
            ChangeCarActive();
        }
        else if(_listClient.value!=0)
        {
            Debug.Log($"{_listClient.options[_listClient.value].text}  {_idClient[_listClient.value-1]}");
            DocumentReference clientRef = db.Collection(_clientPath).Document(_idClient[_listClient.value-1]);
            Dictionary<string, object> updateClient = new Dictionary<string, object>
            {
                {"CarId", _idCar}
            };
            await clientRef.UpdateAsync(updateClient).ContinueWithOnMainThread(task=>
            {
                ChangeCarActive();
            });
        }
        else
        {
            Debug.LogWarning("thats not supposed to happen");
        }
        //Ajouter Le char choisi au client existant
        // else if(_)
        ///Test///
        // _listClient.value;
        // Debug.Log(_listClient.options[_listClient.value].text);
        // Debug.Log(_nomField.text);
        // foreach (string item in _option)
        // {
        // // _listClient.AddOptions(_option);

        // }
    }

    private async void ChangeCarActive()
    {
        DocumentReference carRef = db.Collection(_carPath).Document(_idCar);
        Dictionary<string, object> updateCar = new Dictionary<string, object>
        {
            {"Disponible", false}
        };
         await carRef.UpdateAsync(updateCar).ContinueWithOnMainThread(task =>
        {
            Debug.Log("Car is unavailable");
            SceneManager.LoadScene("List");
        });
    }
}
