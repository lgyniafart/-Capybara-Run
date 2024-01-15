using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SocialPlatforms.GameCenter;

public class MapGenerator : MonoBehaviour
{
    int itemSpace = 30; //���������� ����� ������������� �� ����� �������
    int itemCountInMap = 5; //���������� ����������� �� ����� �����
    enum TrackPos { Left = -1, Center = 0, Right = 1 }; //������� �������
    public float laneOffset = 9f; //���������� ����� ��������� ������
    int orangeCountInItem = 10; //���������� ���������� � �����
    float orangeHeight = 2f; //������, �� ������� ����� ����������
    int mapSize;

    enum OrangeStale { Line, Jump, PartOfLine, Ramp }; //����� ������������ ����������

    //public GameObject Ramp;
    public GameObject Bottom; //������� �����������
    public GameObject Full;
    public GameObject Left;
    public GameObject Right;
    public GameObject Middle;
    public GameObject Orange;

    public List<GameObject> maps = new List<GameObject>(); //���� ��� ���� �����������
    public List<GameObject> activeMaps = new List<GameObject>(); //���� ��� �������� ���� �����������

    static public MapGenerator instance; //������� ��������� (����������� ����������)

    struct MapItem
    {
        public void SetValues(GameObject obstacle, TrackPos trackPos, OrangeStale orangeStale)
        {
            this.obstacle = obstacle;
            this.trackPos = trackPos;
            this.orangeStale = orangeStale;
        }
        public GameObject obstacle;
        public TrackPos trackPos;
        public OrangeStale orangeStale;
    } //���������� ���� ����������� ���������

    private void Awake() //��������������� �������� ���������� ���������� ����, ���� ������ �� �������
    {
        instance = this;
        mapSize = itemCountInMap * itemSpace;
        maps.Add(MakeMap1());
        maps.Add(MakeMap2());
        maps.Add(MakeMap3());
        maps.Add(MakeMap4());
        maps.Add(MakeMap5());
        maps.Add(MakeMap6());
        maps.Add(MakeMap7());
        maps.Add(MakeMap8());
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }
    }
 
    void Update()  //��������� � �������� ���� � ��������� ������, � �.�. ����� ����� �� �����, �� ���������� � �������� �������
    {
        if (RoadGenerator.instance.speed == 0) return;
        foreach (GameObject map in activeMaps)
        {
            map.transform.position -= new Vector3(0, 0, RoadGenerator.instance.speed * Time.deltaTime);
        }
        if (activeMaps[0].transform.position.z < -mapSize)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }

    void RemoveFirstActiveMap()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    } //�������� ������ �������� �����

    public void ResetMaps()
    {
        while (activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }
        AddActiveMap();
        AddActiveMap();
    } //�������� ����� �����������

    void AddActiveMap()
    {
        int r = Random.Range(0, maps.Count);
        GameObject go = maps[r];
        go.SetActive(true);
        foreach(Transform child in go.transform)
        {
            child.gameObject.SetActive(true);
        }
        go.transform.position = activeMaps.Count > 0 ?
                                activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * mapSize :
                                new Vector3(0, 0, 10);
        maps.RemoveAt(r);
        activeMaps.Add(go);
    } //���������� �������� ����� �����������
    GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            OrangeStale orangeStyle = OrangeStale.Line;

            if (i == 2) { trackPos = TrackPos.Right; obstacle = Left; orangeStyle = OrangeStale.PartOfLine; }
            else if (i == 3) { trackPos = TrackPos.Left; obstacle = Bottom; orangeStyle = OrangeStale.Jump; }
            else if (i == 4) { trackPos = TrackPos.Center; obstacle = Full; orangeStyle = OrangeStale.Line; }

            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateOrange(orangeStyle, obstaclePos, result);
            
            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    } //������� 1

    GameObject MakeMap2()
    {
        GameObject result = new GameObject("Map2");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            OrangeStale orangeStyle = OrangeStale.Line;

            if (i == 2) { trackPos = TrackPos.Center; obstacle = Middle; orangeStyle = OrangeStale.PartOfLine; }
            else if (i == 3) { trackPos = TrackPos.Right; obstacle = Bottom; orangeStyle = OrangeStale.Jump; }
            else if (i == 4) { trackPos = TrackPos.Left; obstacle = Full; orangeStyle = OrangeStale.Line; }

            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateOrange(orangeStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    } //������� 2

    GameObject MakeMap3()
    {
        GameObject result = new GameObject("Map3");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            OrangeStale orangeStyle = OrangeStale.Line;

            if (i == 2) { trackPos = TrackPos.Right; obstacle = Left; orangeStyle = OrangeStale.PartOfLine; }
            else if (i == 3) { trackPos = TrackPos.Center; obstacle = Left; orangeStyle = OrangeStale.PartOfLine; }
            else if (i == 4) { trackPos = TrackPos.Left; obstacle = Bottom; orangeStyle = OrangeStale.Jump; }
            

            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateOrange(orangeStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    } //������� 3

    GameObject MakeMap4()
    {
        GameObject result = new GameObject("Map4");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            OrangeStale orangeStyle = OrangeStale.Line;

            if (i == 2) { trackPos = TrackPos.Right; obstacle = Bottom; orangeStyle = OrangeStale.Jump; }
            else if (i == 3) { trackPos = TrackPos.Center; obstacle = Right; orangeStyle = OrangeStale.PartOfLine; }
            else if (i == 4) { trackPos = TrackPos.Left; obstacle = Right; orangeStyle = OrangeStale.PartOfLine; }


            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateOrange(orangeStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    } //������� 4

    GameObject MakeMap5()
    {
        GameObject result = new GameObject("Map5");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            OrangeStale orangeStyle = OrangeStale.Line;

            if (i == 2) { trackPos = TrackPos.Right; obstacle = Full; orangeStyle = OrangeStale.Line; }
            else if (i == 3) { trackPos = TrackPos.Center; obstacle = Full; orangeStyle = OrangeStale.Line; }
            else if (i == 4) { trackPos = TrackPos.Left; obstacle = Full; orangeStyle = OrangeStale.Line; }


            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateOrange(orangeStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    } //������� 5

    GameObject MakeMap6()
    {
        GameObject result = new GameObject("Map6");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            OrangeStale orangeStyle = OrangeStale.Line;

            if (i == 2) { trackPos = TrackPos.Right; obstacle = Left; orangeStyle = OrangeStale.PartOfLine; }
            else if (i == 3) { trackPos = TrackPos.Left; obstacle = Right; orangeStyle = OrangeStale.PartOfLine; }
            //else if (i == 4) { trackPos = TrackPos.Left; obstacle = Full; orangeStyle = OrangeStale.Line; }


            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateOrange(orangeStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    } //������� 6

    GameObject MakeMap7()
    {
        GameObject result = new GameObject("Map7");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            OrangeStale orangeStyle = OrangeStale.Line;

            if (i == 2) { trackPos = TrackPos.Right; obstacle = Bottom; orangeStyle = OrangeStale.Jump; }
            else if (i == 3) { trackPos = TrackPos.Center; obstacle = Middle; orangeStyle = OrangeStale.PartOfLine; }
            else if (i == 4) { trackPos = TrackPos.Left; obstacle = Bottom; orangeStyle = OrangeStale.Jump; }


            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateOrange(orangeStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    } //������� 7

    GameObject MakeMap8()
    {
        GameObject result = new GameObject("Map8");
        result.transform.SetParent(transform);
        for (int i = 0; i < itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            OrangeStale orangeStyle = OrangeStale.Line;

            if (i == 2) { trackPos = TrackPos.Right; obstacle = Bottom; orangeStyle = OrangeStale.Jump; }
            else if (i == 3) { trackPos = TrackPos.Center; obstacle = Full; orangeStyle = OrangeStale.Line; }
            else if (i == 4) { trackPos = TrackPos.Left; obstacle = Right; orangeStyle = OrangeStale.PartOfLine; }
            else if (i == 5) { trackPos = TrackPos.Right; obstacle = Full; orangeStyle = OrangeStale.Line; }


            Vector3 obstaclePos = new Vector3((int)trackPos * laneOffset, 0, i * itemSpace);
            CreateOrange(orangeStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject go = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    } //������� 8
    void CreateOrange(OrangeStale style, Vector3 pos, GameObject parentObject)  //������� ������ ����������, ������� �������� ��������
    {
        Vector3 orangePos = Vector3.zero;
        if (style == OrangeStale.Line) //��� ������ ��������
        {
            for (int i = -orangeCountInItem/2; i < orangeCountInItem/2; i++)
            {
                orangePos.y = orangeHeight;
                orangePos.z = i * ((float)itemSpace / orangeCountInItem);
                GameObject go = Instantiate(Orange, orangePos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            } 
        }else if (style == OrangeStale.Jump) //��� ����������� � �������
        {
            for (int i = -orangeCountInItem / 2; i < orangeCountInItem / 2; i++)
            {
                orangePos.y = Mathf.Max (-1/2f * Mathf.Pow(i,2)+10, orangeHeight);
                orangePos.z = i * ((float)itemSpace / orangeCountInItem);
                GameObject go = Instantiate(Orange, orangePos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        } else if (style == OrangeStale.Ramp) //��� �����, ������� � ������ �� �������� � ������ �����������
        {
            for (int i = -orangeCountInItem / 2; i < orangeCountInItem / 2; i++)
            {
                orangePos.y = Mathf.Min(Mathf.Max(0.7f * (i+8), orangeHeight), 8.0f);
                orangePos.z = i * ((float)itemSpace / orangeCountInItem);
                GameObject go = Instantiate(Orange, orangePos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        } else if (style == OrangeStale.PartOfLine) //��� ����������
        {
            for (int i = -orangeCountInItem / 5; i < orangeCountInItem / 5; i++)
            {
                orangePos.y = orangeHeight;
                orangePos.z = (i * ((float)itemSpace / orangeCountInItem)) - 10;
                GameObject go = Instantiate(Orange, orangePos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
    }
} //������ ��� �������� ����������� ����������� �� ����������� ������
