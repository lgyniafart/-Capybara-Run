using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeManager : MonoBehaviour
{
    Vector2 startTouch;
    bool touchMoved;
    Vector2 swipeDelta;
    const float SWIPE_THRESHOLD = 50;

    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;

    public delegate void ClickDelegate(Vector2 pos);
    public ClickDelegate ClickEvent;

    public enum Direction { Left, Right, Up, Down};
    bool[] swipe = new bool[4];

    Vector2 TouchPosition() { return (Vector2)Input.mousePosition; } //возвращает позицию касания
    bool TouchBegan() { return Input.GetMouseButtonDown(0); }
    bool TouchEnded() { return Input.GetMouseButtonUp(0); }
    bool GetTouch() { return Input.GetMouseButton(0); } //состояние "произведено сейчас касание или нет"

    public static SwipeManager instance;
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (TouchBegan())
        {
            startTouch = TouchPosition(); //определение начала касания и запоминание его в переменную
            touchMoved = true; //переводим параметр в правду, что мы начали движение
        }
        else if (TouchEnded() && touchMoved == true) //определяем конец касания и что свайп завершён
        {
            SendSwipe();
            touchMoved = false;
        }

        swipeDelta = Vector2.zero; //просчёт длины свайпа
        if (touchMoved && GetTouch())
        {
            swipeDelta = TouchPosition() - startTouch;
        }

        if (swipeDelta.magnitude > SWIPE_THRESHOLD) //проверка "это был свайп?"
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) //свайп право/лево
            {
                swipe[(int)Direction.Left] = swipeDelta.x < 0;
                swipe[(int)Direction.Right] = swipeDelta.x > 0;
            }
            else // свайп верх/низ
            {
                swipe[(int)Direction.Up] = swipeDelta.y < 0;
                swipe[(int)Direction.Down] = swipeDelta.y > 0;
            }
            SendSwipe();
        }
    }

    void SendSwipe()
    {
        if (swipe[0] || swipe[1] || swipe[2] || swipe[3])
        {
            Debug.Log("MMMMMM");
            //Debug.Log(swipe[0] + '|' + swipe[1] + '|' + swipe[2] + '|' + swipe[3]);
            MoveEvent?.Invoke(swipe); //проверка на нулёвость (ты просто нулёвый чел)
        }
        else
        {
            Debug.Log("Click");
            ClickEvent?.Invoke(TouchPosition());
        }
        Reset();
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        touchMoved = false;
        for (int i = 0; i < 4; i++) { swipe[i] = false; }
    }
} //вся эта байда для регистрации свайпов при игре на сенсорном экране, но по итогу этот скрипт мы не стали никуда подключать
