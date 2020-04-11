﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField]
    private World World;

    [SerializeField]
    private GameObject Menu;

    [SerializeField]
    private GameObject Map;
    private Map MapScript;

    private bool isShow;

    public void Awake()
    {

        MapScript = Map.GetComponent<Map>();

    }

    private void Update()
    {

        if (Input.GetKeyDown("m"))
        {

            MapButton();

        }

        if (Input.GetButtonDown("Cancel"))
        {

            Cancel();

        }

    }

    public void ShowMenu()
    {

        isShow = true;
        Menu.SetActive(true);        

    }

    public void ShowMap()
    {

        isShow = true;
        Map.SetActive(true);

    }

    public void Continue()
    {

        Menu.SetActive(false);
        Map.SetActive(false);
        isShow = false;

    }

    public void Exit()
    {

        Application.Quit();

    }

    public void Cancel()
    {

        if (isShow)
        {

            Continue();

        }
        else
        {

            ShowMenu();

        }

    }

    public void MapButton()
    {

        if (!isShow)
        {

            ShowMap();

        }

    }

    public void GenerateWorldFullRandom()
    {

        World.GenerateWorldRandomHills();

        MapScript.CreateMap();

    }

    public void GenerateWorldRandomHills()
    {        

        World.GenerateWorldRandomHills();

        MapScript.CreateMap();

    }

}
