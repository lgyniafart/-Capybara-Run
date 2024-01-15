using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject RoadPrefab; //объ€вление префаба дороги (сцена с дорожками и деревь€ми)
    private List<GameObject> roads = new List<GameObject>(); //лист дл€ дорог
    public float maxSpeed = 30; //скорость смены дорог, по факту это то, с какой скоростью движетс€ капибара (но на самом деле она стоит на месте)
    public float speed = 0; //скорость капибары на этапе менюшки
    public int maxRoadCount = 7; //максимальное количество отображаемых дорог в сцене

    public static RoadGenerator instance; //задание синглтона (одинакового оформлени€)
    void Awake() 
    { 
        instance = this; 
    }

    void Start()
    {
        ResetLevel();
        //StartLevel();
    }

    void Update()
    {
        if (speed == 0) return;

        foreach (GameObject road in roads)
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
        if (roads[0].transform.position.z < - 60)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);

            CreateNextRoad();
        }
    } 
    private void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero;
        if (roads.Count > 0) { pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 60
            ); }
        GameObject go =  Instantiate(RoadPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        roads.Add(go);
    } //метод создани€/стыковки дорог

    public void ResetLevel()
    {
        speed = 0;
        while (roads.Count > 0)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
        }
        for (int i = 0; i < maxRoadCount; i++)
        {
            CreateNextRoad();
        }
        SwipeManager.instance.enabled = false;
        MapGenerator.instance.ResetMaps();
    } //метод перезапуска игры при поражении или нажатии кнопки стоп
    public void StartLevel()
    {
        speed = maxSpeed;
        SwipeManager.instance.enabled = true;
    } //метод запуска игры при нажатии кнопки старт
} //скрипт дл€ создани€ бесконечных дорог
