using System;
using System.Linq;
using System.Collections.Generic;

abstract class Character
{
    public string Name;
    public int Health;
    public int Damage;
    public bool IsAlive => Health > 0;
    
    public Character(string name, int health, int damage)
    {
        Name = name;
        Health = health;
        Damage = damage;
    }
    
    public abstract void Attack(Character target);
    
    public void TakeDamage(int amount)
    {
        Health -= amount;
        Console.WriteLine($"{Name} получил {amount} урона. Здоровье: {Health}");
    }
}

class Warrior : Character
{
    static Random rnd = new Random();
    public Warrior(string name, int health, int damage) : base(name, health, damage) { }
    
    public override void Attack(Character target)
    {
        int dmg = rnd.Next(100) < 30 ? Damage * 2 : Damage;
        string crit = dmg > Damage ? " КРИТИЧЕСКИЙ УДАР!" : "";
        Console.WriteLine($"{Name} атакует.{crit} Урон: {dmg}");
        target.TakeDamage(dmg);
    }
}

class Mage : Character
{
    static Random rnd = new Random();
    public Mage(string name, int health, int damage) : base(name, health, damage) { }
    
    public override void Attack(Character target)
    {
        if (rnd.Next(100) < 40 && Health < 100)
        {
            Health += 20;
            Console.WriteLine($"{Name} ВЫЛЕЧИЛСЯ. Здоровье: {Health}");
        }
        else
        {
            Console.WriteLine($"{Name} атакует. Урон: {Damage}");
            target.TakeDamage(Damage);
        }
    }
}

class Monster : Character
{
    public Monster(string name, int health, int damage) : base(name, health, damage) { }
    public override void Attack(Character target)
    {
        Console.WriteLine($"{Name} атакует. Урон: {Damage}");
        target.TakeDamage(Damage);
    }
}

class Program
{
    static void Main()
    {
        var heroes = new List<Character> { new Warrior("Войнучик", 200, 10), new Mage("Колдунчик", 100, 20) };
        var monsters = new List<Monster> { new Monster("Зомби", 100, 30), new Monster("Скелет", 100, 20) };
        
        while (heroes.Any(h => h.IsAlive) && monsters.Any(m => m.IsAlive))
        {
            foreach (var h in heroes.Where(h => h.IsAlive))
            {
                h.Attack(monsters.First(m => m.IsAlive));
                monsters.RemoveAll(m => !m.IsAlive);
                if (!monsters.Any()) break;
            }
            
            foreach (var m in monsters.Where(m => m.IsAlive))
            {
                m.Attack(heroes.First(h => h.IsAlive));
                heroes.RemoveAll(h => !h.IsAlive);
                if (!heroes.Any()) break;
            }
        }
        
        Console.WriteLine(heroes.Any(h => h.IsAlive) ? "\nПОБЕДА" : "\nПОРАЖЕНИЕ");
    }
}