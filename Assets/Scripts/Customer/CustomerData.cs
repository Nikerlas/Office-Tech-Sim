using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Customer Data")]
public class CustomerData
    : ScriptableObject
{
    public string customerName;

    public bool isStoryCustomer = true;

    public List<CustomerJob> stages =
        new List<CustomerJob>();
}