using UnityEngine;

[CreateAssetMenu(fileName = "RockStatus", menuName = "ScriptableObjects/RockStatus", order = 1)]
public class RockStatus : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string stoneName;
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float damage;
    [SerializeField] private float weight;
    [SerializeField] private float cooldown;
    [SerializeField] private string tempString;

    public RockStatus(RockStatus rockStatus)
    {
        id = rockStatus.Id;
        stoneName = rockStatus.stoneName;
        health = rockStatus.health;
        speed = rockStatus.speed;
        acceleration = rockStatus.acceleration;
        damage = rockStatus.damage;
        weight = rockStatus.weight;
        cooldown = rockStatus.cooldown;
        tempString = rockStatus.tempString;
    }

    public int Id { get => id; set => id = value; }
    public string StoneName { get => stoneName; set => stoneName = value; }
    public float Health { get => health; set => health = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Acceleration { get => acceleration; set => acceleration = value; }
    public float Damage { get => damage; set => damage = value; }
    public float Weight { get => weight; set => weight = value; }
    public float Cooldown { get => cooldown; set => cooldown = value; }
    public string TempString { get => tempString; set => tempString = value; }
}