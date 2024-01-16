using GoogleMobileAds.Ump.Api;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserConsentManager : MonoBehaviour
{
    public static UserConsentManager Instance;
    private ConsentForm _consentForm;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        LateSceneLoad();
        //Invoke(nameof(LateSceneLoad), 1f);
        CheckConsent();
    }

    public void LateSceneLoad()
    {
        SceneManager.LoadScene("Home");
    }

    public void CheckConsent()
    {
        Debug.Log("starting");
        // Set tag for under age of consent.
        // Here false means users are not under age.
        ConsentRequestParameters request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = new ConsentDebugSettings() 
            {
                DebugGeography = DebugGeography.Disabled
            }
        };

        // Check the current consent information status.
        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    private void Start1()
    {
        ConsentInformation.Reset();
    }

    void Start2()
    {
        Debug.Log("starting");
        var debugSettings = new ConsentDebugSettings
        {
            // Geography appears as in EEA for debug devices.
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
        {
            "cd27ac21-5570-4eec-99dc-2f1e3d7322d7"
        }
        };

        // Set tag for under age of consent.
        // Here false means users are not under age.
        ConsentRequestParameters request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = debugSettings,
        };

        // Check the current consent information status.
        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            Debug.LogError(error);
            return;
        }    // If the error is null, the consent information state was updated.
             // You are now ready to check if a form is available.
        if (ConsentInformation.IsConsentFormAvailable())
        {
            Debug.Log("yes it is");
            LoadConsentForm();
        }
        else
        {
            Debug.Log("not available");
        }
    }

    void LoadConsentForm()
    {
        // Loads a consent form.
        ConsentForm.Load(OnLoadConsentForm);
    }

    void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            Debug.LogError(error);
            return;
        }

        // The consent form was loaded.
        // Save the consent form for future requests.
        _consentForm = consentForm;

        // You are now ready to show the form.
        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            Debug.Log("form shown");
            _consentForm.Show(OnShowForm);
        }
        else
        {
            Debug.Log("no form shown");
        }
    }

    void OnShowForm(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            Debug.LogError(error);
            return;
        }

        // Handle dismissal by reloading form.
        LoadConsentForm();
        //cd27ac2155704eec99dc2f1e3d7322d7
    }
}