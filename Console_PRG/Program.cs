using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Console_PRG
{
    [Serializable]
    abstract class Hero
    {
        public abstract string Name { get; set; }
        public abstract string Lastname { get; set; }

    }
    [Serializable]
    abstract class Item
    {
        public abstract string Name { get; set; }
        public abstract int Points { get; set; }
    }
    interface IAttacks
    {
        int Attack();
        int UltAttack();
    }
    [Serializable]
    class ChoiceException : Exception
    {
        private char choice;
        public ChoiceException(char choice)
        {
            this.choice = choice;
        }
        public override string Message => "Неверный ввод выбора, поробуйте ещё раз!";
    }
    [Serializable]
    class Warrior : Hero, IAttacks
    {
        private int strenght = 10;
        private int intellegent = 2;
        private int healthPoint = 100;
        public Warrior(string name, string lastname)
        {
            Name = name;
            Lastname = lastname;
        }

        public override string Name { get; set; }

        public override string Lastname { get; set; }
        public int Strenght { get => strenght; set => strenght = value; }
        public int Intellegent { get => intellegent; set => intellegent = value; }
        public int HealthPoint { get => healthPoint; set => healthPoint = value; }

        public int Attack()
        {
            return strenght;
        }

        public int UltAttack()
        {
            return strenght * intellegent;
        }
    }
    [Serializable]
    class Enemy : Hero
    {
        int healthPoint = 30;
        int attack = 2;
        public Enemy(string name, string lastname)
        {
            Name = name;
            Lastname = lastname;
        }

        public override string Name { get; set; }
        public override string Lastname { get; set; }
        public int HealthPoint { get => healthPoint; set => healthPoint = value; }

        public int enemyAttack()
        {
            return attack;
        }
    }
    [Serializable]
    class EnemyBoss : Hero, IAttacks
    {
        int strenght = 7;
        int intellegent = 1;
        int healthPoint = 70;
        public EnemyBoss(string name, string lastname)
        {
            Name = name;
            Lastname = lastname;
        }

        public override string Name { get; set; }
        public override string Lastname { get; set; }
        public int HealthPoint { get => healthPoint; set => healthPoint = value; }

        public int Attack()
        {
            return strenght;
        }

        public int UltAttack()
        {
            return strenght / 2 * intellegent;
        }
    }
    [Serializable]
    class Potions : Item
    {
        public Potions(string name, int points)
        {
            Name = name;
            Points = points;
        }

        public override string Name { get; set; }
        public override int Points { get; set; }

        public int restoreHP()
        {
            return Points;
        }
    }
    [Serializable]
    class Game
    {
        Warrior hero = new Warrior("Бравый", "Воин");
        Enemy enemy = new Enemy("Лесной", "Тролль");
        Potions item = new Potions("Бутыль здоровья", 100);
        Random random = new Random();
        EnemyBoss boss = new EnemyBoss("Вождь", "Троллей");
        private bool endGame = true;
        public void GameStart()
        {
            char choice = 'a';
            Console.WriteLine("Это небольшая история про Бравого Воина, который всегда ищет приключения. \n" +
                "Однажды он шёл по дремучему лесу в поисках добычи, но тут он встретил кое-что страшное. \n" +
                "Это был {0}, и он жаждет драться с вами \n" +
                "У вас есть выбор либо бежать, либо сражаться! Что выбираете Вы? \n " +
                "f - Сражаться, r - Бежать", enemy.Lastname);
            
            while (choice != 'f' && choice != 'r' || choice == ' ')
            {
                choice = char.Parse(Console.ReadLine());
                try
                {
                    if (choice == 'f')
                        Fight();
                    else if (choice == 'r')
                        Run();
                    else
                        throw new ChoiceException(choice);
                }
                catch (ChoiceException ce)
                {
                    Console.WriteLine(ce.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Но на этом приключения не закончились...");
        }
        public void GameTime()
        {
            int countEnemy = random.Next(1, 5);
            int countLoss = 0;
            char choice = 'a';
            while (endGame)
            {
                Console.WriteLine("Вы увидели старого врага вашего поселения, это {0} {1}, \n" +
                    "Настал Ваш час. Вы сможете его победить, но вам преграждают путь его предспешники..", boss.Name, boss.Lastname);
                
                while (countEnemy >= 0 && hero.HealthPoint > 0)
                {
                    Console.WriteLine("Тут подбегает {0} {1}, что Вы решитесь сделать? \n" +
                        "f - Сражаться, r - Бежать", enemy.Name, enemy.Lastname);
                    while (choice != 'f' && choice != 'r' || choice == ' ')
                    {
                        choice = char.Parse(Console.ReadLine());
                        try
                        {
                            if (choice == 'f')
                            {
                                Fight();
                            } 
                            else if (choice == 'r')
                            {
                                Run();
                                countLoss += 1;
                            }
                            else
                            {
                                throw new ChoiceException(choice);
                            }
                            countEnemy -= 1;
                        }
                        catch (ChoiceException ce)
                        {
                            Console.WriteLine(ce.Message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    choice = 'a';
                }
                if (countLoss == countEnemy)
                {
                    Console.WriteLine("Вы убежали от всех врагов! Теперь вы не сможете сразиться с Вашим врагом...");
                    Console.WriteLine("Игра окончена. Желаете попробовать ещё раз? \n" +
                        "y - Да n - Нет" );
                    while(choice != 'y' && choice != 'n' || choice == ' ')
                    {
                        choice = char.Parse(Console.ReadLine());
                        try
                        {
                            if (choice == 'y')
                                GameTime();
                            else if (choice == 'n')
                                endGame = false;
                            else
                                throw new ChoiceException(choice);
                        }
                        catch (ChoiceException ce)
                        {
                            Console.WriteLine(ce.Message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    } 
                }
                else
                {
                    Console.WriteLine("Теперь настало время, поквитаться с Главным врагом...");
                    Console.WriteLine("На данный момент у вашего героя {0} hp \n" +
                        "У вас есть возможность использовать {1}, желаете использовать? \n" +
                        " y - Да, n - Нет"
                        , hero.HealthPoint, item.Name);
                    while (choice != 'y' && choice != 'n' || choice == ' ')
                    {
                        choice = char.Parse(Console.ReadLine());
                        try
                        {
                            if (choice == 'y')
                            {
                                hero.HealthPoint = item.restoreHP();
                                FightWithBoss();
                            }
                            else if (choice == 'n')
                            {
                                FightWithBoss();
                            }
                            else
                                throw new ChoiceException(choice);
                        }
                        catch (ChoiceException ce)
                        {
                            Console.WriteLine(ce.Message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    endGame = false;
                    Console.WriteLine("Поздравляю, Вы смогли победить всех противников! \n" +
                        "Это была славная битва!");
                }
            }
            
        }
        private void Fight()
        {
            Console.WriteLine("Сражение началось!");
            while (enemy.HealthPoint > 0)
            {
                enemy.HealthPoint -= hero.Attack();
                Console.WriteLine("Противник {0} наносит {1} {2}у {3} урона", enemy.Lastname,
                    hero.Name, hero.Lastname, enemy.enemyAttack());
                hero.HealthPoint -= enemy.enemyAttack();
                Console.WriteLine("{0} {1} наносит {2} урона противнику {3} ", hero.Name,
                    hero.Lastname, hero.Attack(), enemy.Lastname);
            }
            Console.WriteLine("Сражение, окончилось...\n" +
                "У вашего героя осталось {0} hp", hero.HealthPoint);
            enemy = new Enemy("Лесной", "Тролль");
        }
        private void FightWithBoss()
        {
            int randult = random.Next(0, 10);
            char choice = 'a';
            Console.WriteLine("Перед началом боя, вы можете использовать свою ультимативную способность, \n" +
                    "которая нанесёт {0} урона Вождю, желаете использовать? \n" +
                    " y - Да, n - Нет", hero.UltAttack());
            while (choice != 'y' && choice != 'n' || choice == ' ')
            {
                choice = char.Parse(Console.ReadLine());
                try
                {
                    if (choice == 'y')
                    {
                        Console.WriteLine("{0} {1} наносит {2} урона противнику {3} ", hero.Name,
                                    hero.Lastname, hero.UltAttack(), boss.Name);
                        boss.HealthPoint -= hero.UltAttack();
                        Console.WriteLine("Сражение началось!");
                        while (boss.HealthPoint != 0)
                        {

                            if (boss.HealthPoint == 10)
                            {
                                Console.WriteLine("О нет!, {0} использует ультимативную способность, она наносит вам {1} урона",
                                    boss.Name, boss.UltAttack());
                                hero.HealthPoint -= boss.UltAttack();
                            }
                            Console.WriteLine("{0} {1} наносит {2} урона противнику {3} ", hero.Name,
                                    hero.Lastname, hero.Attack(), boss.Name);
                            boss.HealthPoint -= hero.Attack();
                            Console.WriteLine("Противник {0} наносит {1} {2}у {3} урона", boss.Name,
                                hero.Name, hero.Lastname, boss.Attack());
                            hero.HealthPoint -= boss.Attack();

                        }
                    }
                    else if (choice == 'n')
                    {
                        Console.WriteLine("Сражение началось!");
                        while (boss.HealthPoint != 0)
                        {

                            if (boss.HealthPoint == 10)
                            {
                                Console.WriteLine("О нет!, {0} использует ультимативную способность, она наносит вам {1} урона",
                                    boss.Name, boss.UltAttack());
                                hero.HealthPoint -= boss.UltAttack();
                            }
                            Console.WriteLine("{0} {1} наносит {2} урона противнику {3} ", hero.Name,
                                    hero.Lastname, hero.Attack(), boss.Name);
                            boss.HealthPoint -= hero.Attack();
                            Console.WriteLine("Противник {0} наносит {1} {2}у {3} урона", boss.Name,
                                hero.Name, hero.Lastname, boss.Attack());
                            hero.HealthPoint -= boss.Attack();

                        }
                    }
                    else
                        throw new ChoiceException(choice);
                }
                catch (ChoiceException ce)
                {
                    Console.WriteLine(ce.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Сражение, окончилось...\n" +
                "У вашего героя осталось {0} hp", hero.HealthPoint);
        } 
        private void Run()
        {
            Console.WriteLine("Пока вы убегали, Вы начаянно подскользнулись и потеряли 10 hp");
            hero.HealthPoint -= 10;
            Console.WriteLine("У вашего героя осталось {0} hp", hero.HealthPoint);
        }
    }
    class SaveGame
    {
        public void StartSaveGame(Game game)
        {
            char choice = 'a';
            Console.WriteLine("Сохранить игру? \n" +
                    "y - Да n - Нет");
            while (choice != 'y' && choice != 'n' || choice == ' ')
            {
                choice = char.Parse(Console.ReadLine());
                try
                {
                    if (choice == 'y')
                    {
                        if (File.Exists("savegame.dat"))
                            File.Delete("savegame.dat");

                        FileStream filetosave = new FileStream("savegame.dat", FileMode.Create);
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(filetosave, game);
                        filetosave.Close();
                        Console.WriteLine("Игра сохранена!");
                    }
                    else if (choice == 'n')
                    {
                        break;
                    }
                    else
                    {
                        throw new ChoiceException(choice);
                    }
                }
                catch(ChoiceException ce)
                {
                    Console.WriteLine(ce.Message);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public void RunSavedGame (Game game)
        {
            char choice = 'a';
            Console.WriteLine("Загрузить игру? \n" +
                    "y - Да n - Нет");
            while (choice != 'y' && choice != 'n' || choice == ' ')
            {
                choice = char.Parse(Console.ReadLine());
                try
                {
                    if (choice == 'y')
                    {
                        FileStream filetoload = new FileStream("savegame.dat", FileMode.Open);
                        BinaryFormatter formatter = new BinaryFormatter();
                        game = (Game)formatter.Deserialize(filetoload);
                        Console.WriteLine("Игра загружена");
                    }
                    else if (choice == 'n')
                    {
                        break;
                    }
                    else
                    {
                        throw new ChoiceException(choice);
                    }
                }
                catch (ChoiceException ce)
                {
                    Console.WriteLine(ce.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            SaveGame save = new SaveGame();
            save.RunSavedGame(game);
            game.GameStart();
            save.StartSaveGame(game);
            game.GameTime();
        }
    }
}
