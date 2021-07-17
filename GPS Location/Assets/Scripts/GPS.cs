using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System;

public class GPS : MonoBehaviour
{
    private float Latitude;
    private float Longitude;

    private bool is_start_routine;
    private String status;

    public Text lbl_location;
    public Text lbl_updated_at;
    public Text lbl_service_status;
     public Text lbl_routine_status;

    // Start is called before the first frame update
    void Start()
    {
        Input.compass.enabled = true;
        StartCoroutine(getLocation());
    }

    IEnumerator getLocation()
    {
        is_start_routine = true;

        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            status = "location service not enabled";
            yield break;
        }
        
        // Start service before querying location
        Input.location.Start(1,1);

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            status = "Timed out";
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            status = "Unable to determine device location";
            yield break;
        }
        else
        {
            Latitude = Input.location.lastData.latitude;
            Longitude = Input.location.lastData.longitude;
            lbl_location.text = "Latitude : " + Latitude + " , Longitude : " + Longitude;
            // Access granted and location value could be retrieved
            // print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        status = "ready..";
        is_start_routine = false;

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        Latitude = Input.location.lastData.latitude;
        Longitude = Input.location.lastData.longitude;

        lbl_routine_status.text = is_start_routine.ToString();
        lbl_service_status.text = Input.location.status.ToString() + "[" + status + "]";
        lbl_location.text = "Lat : " + Latitude + " , Lng : " + Longitude;
        lbl_updated_at.text = DateTime.Now.ToString("dd/mm/yyyy HH:mm:sss");
    }
}
