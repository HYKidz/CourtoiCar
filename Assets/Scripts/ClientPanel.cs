using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClientPanel : MonoBehaviour
{
    [SerializeField] private string _historyPath = "history_list";
    [SerializeField] private string _clientPath = "client_list";
    [SerializeField] private string _carPath = "car_list";
    [SerializeField] private TMP_Text _nomText;
    private string _nom;
    public string Nom
    {
        get => _nom; set
        {
            if (value == _nom) return;
            _nom = value;
            _nomText.text = $"Nom:{value}";
        }
    }
    [SerializeField] private TMP_Text _serieText;
    private string _serie;
    public string Serie
    {
        get => _serie; set
        {
            if (value == _serie) return;
            _serie = value;
            _serieText.gameObject.SetActive(true);
            _serieText.text = $"Serie:{value}";
        }
    }
    [SerializeField] private TMP_Text _anneText;
    private string _anne;
    public string Anne
    {
        get => _anne; set
        {
            if (value == _anne) return;
            _anne = value;
            _anneText.gameObject.SetActive(true);
            _anneText.text = $"Anne:{value}";
        }
    }
    [SerializeField] private TMP_Text _infoText;
    private string _info;
    public string Info
    {
        get => _info; set
        {
            if (value == _info) return;
            _info = value;
            _infoText.gameObject.SetActive(true);
            _infoText.text = $"Information:{value}";
        }
    }
    [SerializeField] private TMP_Text _plaqueText;
    private string _plaque;
    public string Plaque
    {
        get => _plaque; set
        {
            if (value == _plaque) return;
            _plaque = value;
            _plaqueText.gameObject.SetActive(true);
            _plaqueText.text = $"Plaque:{value}";
        }
    }
    private string _ID;
    public string ID
    {
        set => _ID = value;
    }
    [SerializeField] private TMP_Text _dateEmpruntText;
    private string _dateEmprunt;
    public string DateEmprunt
    {
        get => _dateEmprunt; set
        {
            if (value == _dateEmprunt) return;
            _dateEmprunt = value;
            _dateEmpruntText.gameObject.SetActive(true);
            _dateEmpruntText.text = $"Debut Emprunt:{value}";
        }
    }
    [SerializeField] public GameObject _buttonRetour;
    private bool _dispo;
    public bool Dispo
    {
        get => _dispo; set
        {
            _dispo = value;
            // if(_dispo)
        }
    }
    private Timestamp _timeGet;
    public Timestamp TimeGet
    {
        set => _timeGet = value;
    }
    private string _carID;
    public string CarId
    {
        set => _carID = value;
    }
    public void Click()
    {
        // Debug.Log(_carID);
        Retour();
    }
    private async void Retour()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        Debug.Log(_ID);
        DocumentReference clientRef = db.Collection(_clientPath).Document(_ID);
        DocumentReference CarRef = db.Collection(_carPath).Document(_carID);
    
        string date = _timeGet.ToDateTime().ToString("yyy-MM-dd");
        Dictionary<string, object> updateCar = new Dictionary<string, object> 
        {
            {"Disponible", true}
        };
        Dictionary<string, object> updateClient = new Dictionary<string, object>
        {
            // {
                
            //     "Hierarchy", new Dictionary<string,object>
            //     {
            //         {$"Empreint{_timeGet.ToDateTime()} char{_plaque}",new Dictionary<String, DateTime>
            //             {
            //                 {"Empreint", _timeGet.ToDateTime()},{"Retour", DateTime.Now}
            //             }
            //         },
            //     }
            //     // "Hierarchy", ($"Empreint du char{_plaque}", (_timeGet , DateTime.Now ))
            // },
            {"TimeGet",FieldValue.Delete },
            {"CarId", FieldValue.Delete}
        };
        Dictionary<string, object> newHistory = new Dictionary<string, object>
        {
            
                
                // "Hierarchy", new Dictionary<string,object>
                // {
                //     {$"Empreint{_timeGet.ToDateTime()} char{_plaque}",new Dictionary<String, DateTime>
                //         {
                            {$"Empreint{_nom}_{date}_{_plaque}", new Dictionary<string,object>
                            {
                                {"Empreint", _timeGet.ToDateTime()},{"Retour", DateTime.Now}, {"client", _nom}, {"vehicule", _carID}
                            }}
            //             }
            //         },
            //     }
            //     // "Hierarchy", ($"Empreint du char{_plaque}", (_timeGet , DateTime.Now ))
            // },
            // {"TimeGet",FieldValue.Delete },
            // {"CarId", FieldValue.Delete}
        };

        DocumentReference AddedDocRef = await db.Collection(_historyPath).AddAsync(newHistory);
        await clientRef.UpdateAsync(updateClient).ContinueWithOnMainThread(async task =>
        {
            await CarRef.UpdateAsync(updateCar).ContinueWithOnMainThread( task =>
            {
                SceneManager.LoadScene("List");

            });

            // Debug.Log(clientRef.GetSnapshotAsync);

        });
        //gerer retour de voiture
    }

}
