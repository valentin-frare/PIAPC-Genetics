using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Empanada
{
    public float flavor1;
    public float flavor2;
    public float flavor3;

    public bool nice;

    public Empanada(float flavor1, float flavor2, float flavor3)
    {
        this.flavor1 = flavor1;
        this.flavor2 = flavor2;
        this.flavor3 = flavor3;
    }
}

public class EmpanadaFramework : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject empanadaPrefab;

    [Header("UI")]
    [SerializeField] private Transform empanadasContent;
    
    [Header("Color de sabores")]
    [SerializeField] private Gradient gradient1;
    [SerializeField] private Gradient gradient2;
    [SerializeField] private Gradient gradient3;

    [Header("Inputs")]
    [SerializeField] private Dropdown dropdown1;
    [SerializeField] private Dropdown dropdown2;
    [SerializeField] private Dropdown dropdown3;

    [SerializeField] private (int, int, int) goal;

    [Header("Generaciones y Empanadas Generadas")]
    [SerializeField] private int generation = 0;

    [SerializeField] private List<Empanada> empanadas = new List<Empanada>();

    private IEnumerator evolutionCoroutine;

    private void Awake() 
    {
        //dropdown1.onValueChanged.AddListener(ResetEvolution);
        //dropdown2.onValueChanged.AddListener(ResetEvolution);
        //dropdown3.onValueChanged.AddListener(ResetEvolution);

        evolutionCoroutine = StartEvolution();
    }

    private void ResetEvolution(int unused)
    {
        generation = 0;
        StopCoroutine(evolutionCoroutine);

        empanadas = GenerateRandomEmpanadas();

        StartCoroutine(evolutionCoroutine);
    }

    private void Start() 
    {
        ResetEvolution(0);
    }

    void Update()
    {
        goal = (dropdown1.value, dropdown2.value, dropdown3.value);
    }

    private void RenderEmpanadas()
    {
        for (var i = 0; i < empanadasContent.childCount; i++)
        {
            Destroy(empanadasContent.GetChild(i).gameObject);
        }

        foreach (var empanada in empanadas)
        {
            var empanadaObj = Instantiate(empanadaPrefab, empanadasContent);
            empanadaObj.GetComponent<EmpanadaObject>().flavor1 = gradient1.Evaluate(empanada.flavor1/3);
            empanadaObj.GetComponent<EmpanadaObject>().flavor2 = gradient2.Evaluate(empanada.flavor2/3);
            empanadaObj.GetComponent<EmpanadaObject>().flavor3 = gradient3.Evaluate(empanada.flavor3/3);
            empanadaObj.GetComponent<EmpanadaObject>().nice = empanada.nice;
        }
    }

    private IEnumerator StartEvolution()
    {
        while (true) 
        {
            empanadas = CalculateNiceEmpanadas(empanadas);

            if (generation > 1)
            {
                if (empanadas.Count == 0)
                {
                    empanadas = GenerateRandomEmpanadas();
                }
                else 
                {
                    empanadas = GenerateMutations(empanadas, 12);
                }
            }

            RenderEmpanadas();

            yield return new WaitForSeconds(2f);
            generation++;
        }
    }

    private List<Empanada> GenerateRandomEmpanadas()
    {
        var tempList = new List<Empanada>();

        for (var i = 0; i < 12; i++)
        {
            tempList.Add(new Empanada(
                UnityEngine.Random.Range(0f, 3f),
                UnityEngine.Random.Range(0f, 3f),
                UnityEngine.Random.Range(0f, 3f)
            ));
        }

        return tempList;
    }

    private List<Empanada> CalculateNiceEmpanadas(List<Empanada> empanadas)
    {
        var tempList = new List<Empanada>();

        foreach (var empanada in empanadas)
        {
            var prom = (Mathf.Abs(goal.Item1 - empanada.flavor1) + Mathf.Abs(goal.Item2 - empanada.flavor2) + Mathf.Abs(goal.Item3 - empanada.flavor3)) / 3;

            if (Mathf.Abs(prom) < 0.5f)
            {
                tempList.Add(empanada);
                empanada.nice = true;
            }
        }

        return tempList;
    }

    private List<Empanada> GenerateMutations(List<Empanada> empanadasNice, int lenght)
    {
        var tempList = new List<Empanada>();

        for (var i = 0; i < (lenght - empanadasNice.Count); i++)
        {
            Empanada randomEmpanada = empanadasNice[UnityEngine.Random.Range(0, empanadasNice.Count)];

            tempList.Add(new Empanada(
                randomEmpanada.flavor1 + UnityEngine.Random.Range(-.5f, .5f),
                randomEmpanada.flavor2 + UnityEngine.Random.Range(-.5f, .5f),
                randomEmpanada.flavor3 + UnityEngine.Random.Range(-.5f, .5f)
            ));
        }

        foreach (var empanada in empanadasNice)
        {
            tempList.Add(empanada);
        }

        return tempList;
    }
}
