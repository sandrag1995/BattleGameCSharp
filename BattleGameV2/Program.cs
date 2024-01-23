namespace BattleGameV2
{
    internal class Hero
    {
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int MaxMana { get; set; }
        public int MaxDamage { get; set; }
        public int MaxMagicDamage { get; set; }
        public int MinHealAmount { get; set; }
        public int MaxHealAmount { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }

        public Hero(string name, int maxHp, int maxMana, int maxDamage, int maxMagicDamage, int minHealAmount, int maxHealAmount)
        {
            Name = name;
            MaxHp = maxHp;
            MaxMana = maxMana;
            MaxDamage = maxDamage;
            MaxMagicDamage = maxMagicDamage;
            MinHealAmount = minHealAmount;
            MaxHealAmount = maxHealAmount;
            Exp = 0;
            Gold = 50;
        }
    }

    internal class Monster
    {
        public string Name { get; set; }
        public int MaxHp { get; set; }
        public int MaxMana { get; set; }
        public int MaxDamage { get; set; }
        public int MaxMagicDamage { get; set; }
        public int MinHealAmount { get; set; }
        public int MaxHealAmount { get; set; }

        public Monster(string name, int maxHp, int maxMana, int maxDamage, int maxMagicDamage, int minHealAmount, int maxHealAmount)
        {
            Name = name;
            MaxHp = maxHp;
            MaxMana = maxMana;
            MaxDamage = maxDamage;
            MaxMagicDamage = maxMagicDamage;
            MinHealAmount = minHealAmount;
            MaxHealAmount = maxHealAmount;
        }

    }

    internal class Program
    {
        //Player Section
        private static Hero hero = new Hero("", 100, 50, 10, 15, 1, 5);

        //Player inventory
        private const int MaxManaInventorySlots = 5;
        private const int MaxHealthInventorySlots = 5;

        //Potion restore points
        private const int PotionHealthPoints = 20;
        private const int PotionManaPoints = 20;
        
        
        //Enemy Section

        private static Monster[] enemies = new Monster[]
        {
            new Monster("Slime", 20, 0, 5, 0, 0, 0),
            new Monster("Skeleton", 50, 0, 10, 0, 0, 0),
            new Monster("Zombie", 75, 0, 15, 0, 0, 0),
            new Monster("Thief", 100, 20, 20, 20, 1, 5),
            new Monster("Barbarian", 120, 20, 20, 20, 1, 5),
            new Monster("Witch", 150, 50, 25, 35, 1, 10),
            new Monster("Dark Mage", 180, 70, 35, 35, 1, 15),
            new Monster("Demon", 220, 100, 40, 40, 1, 20),
            new Monster("Arc Demon", 250, 100, 45, 40, 1, 20),
            new Monster("Demon King", 350, 100, 50, 50, 1, 30),
        };

        private static int currentEnemyIndex = 0;

       
        private static Random random = new Random();

        private static void Main(string[] args)
        {
            String response;

            Console.WriteLine("-**Battle Game**-");
            Console.WriteLine("*****************\n");
            Console.WriteLine("Welcome, fighter!");

            while (string.IsNullOrWhiteSpace(hero.Name))
            {
                Console.WriteLine("What's your name? ");
                hero.Name = Console.ReadLine();
            }

            Console.WriteLine("");
            Console.WriteLine("Enter 'A' to attack, 'M' for magic attack or 'SMA' to use special magic attack.\n'HP' to use health potion, 'MP' to use mana potion.\n");
            bool playAgain = true;
            response = "";
            int playerExp = hero.Exp;
            int playerGold = hero.Gold;

            //Player's status during next round
            int playerHp = hero.MaxHp;
            int healthInventorySlots = MaxHealthInventorySlots;
            int manaInventorySlots = MaxManaInventorySlots;
            int playerMana = hero.MaxMana;

            while (playAgain)
            {
                int enemyMana = enemies[currentEnemyIndex].MaxMana;
                int enemyHp = enemies[currentEnemyIndex].MaxHp;

                Console.WriteLine("Prepare yourself!");
                Console.WriteLine($"Your challenger is {enemies[currentEnemyIndex].Name}!\n");
                while (playerHp > 0 && enemyHp > 0)
                {
                    PlayerTurn(ref playerHp, ref enemyHp, ref playerMana, ref enemyMana, ref healthInventorySlots, ref manaInventorySlots, ref playerExp, ref playerGold);
                    if (enemyHp > 0)
                    {
                        EnemyTurn(ref playerHp, ref enemyHp, ref enemyMana);
                    }
                }

                DisplayGameResult(playerHp, playerExp, playerGold);
                
                response = Console.ReadLine();
                response = response.ToUpper();
                

                if (response == "Y")
                {
                    currentEnemyIndex++;
                    if (currentEnemyIndex >= enemies.Length)
                    {
                        currentEnemyIndex = 0;
                    }

                    playAgain = true;
                    if (playerHp > 0)
                    {
                        playerExp += 100;
                        playerGold += 10;
                    }
                }
                else
                {
                    playAgain = false;
                }
            }
            Console.WriteLine("Thank you for playing!");
            Console.WriteLine("Press 'ENTER' to exit.");
            Console.ReadKey();
        }

        //Player's Choices

        private static void PerformAttack(ref int enemyHp)
        {
            int playerAttack = random.Next(1, hero.MaxDamage + 1);
            enemyHp -= playerAttack;
            Console.WriteLine($"{hero.Name} attacks and deals {playerAttack} damage!\n");
        }

        private static void UseHealthPotion(ref int playerHp, ref int healthInventorySlots)
        {
            if (healthInventorySlots > 0)
            {
                playerHp = Math.Min(hero.MaxHp, playerHp + PotionHealthPoints);
                healthInventorySlots--;
                Console.WriteLine($"{hero.Name} uses a health potion and restores {PotionHealthPoints} health points!\n");
            }
            else
            {
                Console.WriteLine("No health potions left in the inventory!");
            }
        }

        private static void UseManaPotion (ref int playerMana, ref int manaInventorySlots)
        {
            if (manaInventorySlots > 0)
            {
                playerMana = Math.Min(hero.MaxMana, playerMana + PotionManaPoints);
                manaInventorySlots--;
                Console.WriteLine($"{hero.Name} uses a mana potion and restores {PotionManaPoints} mana points!\n");
            }
            else
            {
                Console.WriteLine("No mana potions left in the inventory!\n");
            }
        }

        private static void PerformMagicAttack(ref int enemyHp, ref int playerMana)
        {
            if (playerMana > 0)
            {
                int magicAttack = random.Next(1, hero.MaxMagicDamage + 1);
                enemyHp -= magicAttack;
                playerMana -= 15;

                FireBallEffect();
                Console.WriteLine($"{hero.Name} casts a magic spell and deals {magicAttack} damage!\n");
            }
            else
            {
                Console.WriteLine("You don't have enough mana points!\n");
            }
        }

        private static void PerformSuperMagicAttack(ref int enemyHp, ref int playerMana)
        {
            if (playerMana >= 50)
            {
                int magicAttack = 50;
                enemyHp -= magicAttack;
                playerMana -= 50;
                SpecialFireEffect();
                Console.WriteLine($"{hero.Name} casts a special magic spell and deals {magicAttack} damage!\n");
            }
            else
            {
                Console.WriteLine("You don't have enough mana points!\n");
            }
        }


        //Player's turn
        private static void PlayerTurn(ref int playerHp, ref int enemyHp, ref int playerMana, ref int enemyMana, ref int healthInventorySlots, ref int manaInventorySlots, ref int playerExp, ref int playerGold)
        {
            Console.WriteLine($"--- {hero.Name}'s turn ---");
            PrintGameState(playerHp, enemyHp, playerMana, enemyMana, healthInventorySlots, manaInventorySlots, playerExp, playerGold);
            Console.WriteLine("What will you do next?");

            string choice;
            bool isValidChoice;

            do
            {
                choice = Console.ReadLine()?.ToUpper();
                isValidChoice = choice == "A" || choice == "M" || choice == "HP" || choice == "MP" || choice == "SMA";

                if (!isValidChoice)
                {
                    Console.WriteLine("Invalid command! Please enter a valid command.\n" +
                        "Enter 'A' to attack, 'M' for magic attack or 'SMA' to use special magic attack.\n'HP' to use health potion, 'MP' to use mana potion.");
                }
            } while (!isValidChoice);

            switch (choice)
            {
                case "A":
                    PerformAttack(ref enemyHp);
                    break;


                case "HP":
                    UseHealthPotion(ref playerHp, ref healthInventorySlots);
                    break;

                case "MP":
                    UseManaPotion(ref playerMana, ref manaInventorySlots);
                    break;

                case "M":
                  PerformMagicAttack(ref  enemyHp, ref playerMana);
                    break;

                case "SMA":
                    PerformSuperMagicAttack(ref enemyHp, ref playerMana);
                    break;

                default: 
                    Console.WriteLine("This command doesn't exist!");
                    break;
            }
        }

        //Enemy's choices
        private static void EnemyAttack(ref int playerHp)
        {
            int enemyAttack = random.Next(1, enemies[currentEnemyIndex].MaxDamage + 1);
            playerHp -= enemyAttack;
            Console.WriteLine($"{enemies[currentEnemyIndex].Name} attacks and deals {enemyAttack} damage!\n");
        }

        private static void EnemyMagicAttack(ref int playerHp, ref int enemyMana)
        {
            if (enemyMana > 0)
            {
                int magicAttack = random.Next(1, enemies[currentEnemyIndex].MaxMagicDamage + 1);
                playerHp -= magicAttack;
                enemyMana -= 10;
                Console.WriteLine($"{enemies[currentEnemyIndex].Name} casts a magic spell and deals {magicAttack} damage!\n");
            }
        }

        private static void EnemyHeal(ref int enemyHp)
        {
            int healAmount = random.Next(enemies[currentEnemyIndex].MinHealAmount, enemies[currentEnemyIndex].MaxHealAmount + 1);
            enemyHp = Math.Min(enemies[currentEnemyIndex].MaxHp, enemyHp + healAmount);
            Console.WriteLine($"{enemies[currentEnemyIndex].Name} restores {healAmount} health points!\n");
        }

        //Enemy's turn

        private static void EnemyTurn(ref int playerHp, ref int enemyHp, ref int enemyMana)
        {
            Console.WriteLine("--- Enemy's turn ---");

            int enemyChoice = random.Next(0, 3);

            switch (enemyChoice)
            {
                case 0:
                    EnemyAttack(ref playerHp);
                    break;

                case 1:
                    EnemyMagicAttack(ref playerHp, ref enemyMana);
                    break;

                case 2:
                    EnemyHeal(ref enemyHp);
                    break;
            }
        }

        //Game status
        private static void PrintGameState(int playerHp, int enemyHp, int playerMana, int enemyMana, int healthInventorySlots, int manaInventorySlots, int playerExp, int playerGold)
        {
            Console.WriteLine($"{hero.Name} Hp - {playerHp}. Enemy Hp - {enemyHp}.");
            Console.WriteLine($"{hero.Name} Mp - {playerMana}. Enemy Mp - {enemyMana}.");

            Console.WriteLine($"Health Potions - {healthInventorySlots} of {MaxHealthInventorySlots}");
            Console.WriteLine($"Mana Potions   - {manaInventorySlots} of {MaxManaInventorySlots}.\n");
            Console.WriteLine($"Your current EXP points - {playerExp} points.");
            Console.WriteLine($"You have {playerGold} gold.");

            if (healthInventorySlots == 0)
            {
                Console.WriteLine("No health potions left in the inventory!");
            }

            if (manaInventorySlots == 0)
            {
                Console.WriteLine("No mana potions left in the inventory!");
            }
        }

        //Game results
        private static void DisplayGameResult(int playerHp, int playerExp, int playerGold)
        {
            if (playerHp > 0)
            {
                Console.WriteLine("Congratulations! You earned 100 EXP!");
                Console.WriteLine("Another enemy wants to fight you? Will you accept the challenge? (Y/N): ");
            }
            else
            {
                Console.WriteLine("Game over. You lose!");
                Console.WriteLine($"Total experience points: {playerExp}");
                Console.WriteLine("Would you like to try again? (Y/N): ");
            }
        }

        //Special effects
        private static void FireBallEffect()
        {
            string[] intro = File.ReadAllLines("fireball.txt");
            for (int i = 0; i < intro.Length; i++)
            {
                Console.WriteLine(intro[i]);
            }
        }

        private static void SpecialFireEffect()
        {
            string[] intro = File.ReadAllLines("fireeffect.txt");
            for (int i = 0; i < intro.Length; i++)
            {
                Console.WriteLine(intro[i]);
            }
        }
    }
}
