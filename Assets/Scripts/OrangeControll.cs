using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeControll : MonoBehaviour
{
    float rotationSpeed = 70;
    
    void Start()
    {
        rotationSpeed += Random.Range(0, rotationSpeed / 4.0f);
    } //������� �������� ����������� ����� ������

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    } //�������� ����������

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            transform.parent.gameObject.SetActive(false);
        }
    } //�������� ��� ����������� ���������� �������, �.�. �� ����
} //������, �������������� �� ����������
