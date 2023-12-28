using System;

namespace BattleGameV2
{
    internal class Program
    {
        //Player Section
        private static string playerName = "Player";

        private const int MaxPlayerHp = 100;
        private const int MaxPlayerMana = 50;
        private const int MaxPlayerDamage = 10;
        private const int MaxPlayerMagicDamage = 20;

        //Player inventory
        private const int MaxManaInventorySlots = 5;
        private const int MaxHealthInventorySlots = 5;
        private const int PotionHealthPoints = 20;
        private const int PotionManaPoints = 20;

        //Enemy Section
        private const int MaxEnemyHp = 100;
        private const int MaxEnemyMana = 50;
        private const int MaxEnemyDamage = 7;
        private const int MaxEnemyMagicDamage = 20;
        private const int MinHealAmount = 1;
        private const int MaxHealAmount = 5;

        //General Section
        private const int MinDamage = 1;
        private const int MinMagicDamage = 5;


        private static Random random = new Random();

        private static void Main(string[] args)
        {
            Console.WriteLine("-**Battle Game**-");
            Console.WriteLine("*****************\n");
            Console.WriteLine("Welcome, fighter!");

            while (string.IsNullOrWhiteSpace(playerName) || playerName == "Player")
            {
                Console.WriteLine("What's your name? ");
                playerName = Console.ReadLine();
            }


            Console.WriteLine("Enter 'A' to attack, 'M' for magic attack. 'HP' to use health potion, 'MP' to use mana potion.\n");


            int playerHp = MaxPlayerHp;
            int enemyHp = MaxEnemyHp;
            int playerMana = MaxPlayerMana;
            int enemyMana = MaxEnemyMana;
            int healthInventorySlots = MaxManaInventorySlots;
            int manaInventorySlots = MaxManaInventorySlots;

            while (playerHp > 0 && enemyHp > 0)
            {
                PlayerTurn(ref playerHp, ref enemyHp, ref playerMana, ref enemyMana, ref healthInventorySlots, ref manaInventorySlots);
                if (enemyHp > 0)
                {
                    EnemyTurn(ref playerHp, ref enemyHp, ref enemyMana);
                }
            }

            DisplayGameResult(playerHp);

            Console.WriteLine("Thank you for playing!");
            Console.ReadKey();
        }

        private static void PlayerTurn(ref int playerHp, ref int enemyHp, ref int playerMana, ref int enemyMana, ref int healthInventorySlots, ref int manaInventorySlots)
        {
            Console.WriteLine($"--- {playerName}'s turn ---");
            PrintGameState(playerHp, enemyHp, playerMana, enemyMana, healthInventorySlots, manaInventorySlots);

            string choice;
            bool isValidChoice;

            do
            {
                choice = Console.ReadLine()?.ToUpper();
                isValidChoice = choice == "A" || choice == "M" || choice == "HP" || choice == "MP";

                if (!isValidChoice)
                {
                    Console.WriteLine("Invalid command! Please enter a valid command.\n" +
                        "Enter 'A' to attack, 'M' for magic attack. 'HP' to use health potion, 'MP' to use mana potion.");
                }
            } while (!isValidChoice);
            switch (choice)
            {
                case "A":
                    int playerAttack = random.Next(MinDamage, MaxPlayerDamage);
                    enemyHp -= playerAttack;
                    Console.WriteLine($"{playerName} attacks and deals {playerAttack} damage!\n");
                    break;


                case "HP":
                    if (healthInventorySlots > 0)
                    {
                        playerHp = Math.Min(MaxPlayerHp, playerHp + PotionHealthPoints);
                        healthInventorySlots--;
                        Console.WriteLine($"{playerName} uses a health potion and restores {PotionHealthPoints} health points!\n");
                    }
                    else
                    {
                        Console.WriteLine("No health potions left in the inventory!");
                    }
                    break;

                case "MP":
                    if (manaInventorySlots > 0)
                    {
                        playerMana = Math.Min(MaxPlayerMana, playerMana + PotionManaPoints);
                        manaInventorySlots--;
                        Console.WriteLine($"{playerName} uses a mana potion and restores {PotionManaPoints} mana points!\n");
                    }
                    else
                    {
                        Console.WriteLine("No mana potions left in the inventory!\n");
                    }
                    break;

                case "M":
                    if (playerMana > 0)
                    {
                        int magicAttack = random.Next(MinMagicDamage, MaxPlayerMagicDamage);
                        enemyHp -= magicAttack;
                        playerMana -= 10;
                        Console.WriteLine($"{playerName} casts a magic spell and deals {magicAttack} damage!\n");
                    }
                    else
                    {
                        Console.WriteLine("You don't have enough mana points!\n");
                    }
                    break;

                    default: 
                    Console.WriteLine("This command doesn't exist!");
                    break;
            }
        }

        private static void EnemyTurn(ref int playerHp, ref int enemyHp, ref int enemyMana)
        {
            Console.WriteLine("--- Enemy's turn ---");

            int enemyChoice = random.Next(0, 3);

            switch (enemyChoice)
            {
                case 0:
                    int enemyAttack = random.Next(MinDamage, MaxEnemyDamage);
                    playerHp -= enemyAttack;
                    Console.WriteLine($"Enemy attacks and deals {enemyAttack} damage!\n");
                    break;

                case 1:
                    if (enemyMana > 0)
                    {
                        int magicAttack = random.Next(MinMagicDamage, MaxEnemyMagicDamage);
                        playerHp -= magicAttack;
                        enemyMana -= 10;
                        Console.WriteLine($"Enemy casts a magic spell and deals {magicAttack} damage!\n");
                    }
                    break;

                case 2:
                    int healAmount = random.Next(MinHealAmount, MaxHealAmount);
                    enemyHp = Math.Min(MaxEnemyHp, enemyHp + healAmount);
                    Console.WriteLine($"Enemy restores {healAmount} health points!\n");
                    break;
            }
        }

        private static void PrintGameState(int playerHp, int enemyHp, int playerMana, int enemyMana, int healthInventorySlots, int manaInventorySlots)
        {
            Console.WriteLine($"{playerName} Hp - {playerHp}. Enemy Hp - {enemyHp}.");
            Console.WriteLine($"{playerName} Mp - {playerMana}. Enemy Mp - {enemyMana}.");

            Console.WriteLine($"Health Potions - {healthInventorySlots} of {MaxHealthInventorySlots}");
            Console.WriteLine($"Mana Potions   - {manaInventorySlots} of {MaxManaInventorySlots}.\n");

            if (healthInventorySlots == 0)
            {
                Console.WriteLine("No health potions left in the inventory!");
            }

            if (manaInventorySlots == 0)
            {
                Console.WriteLine("No mana potions left in the inventory!");
            }
        }

        private static void DisplayGameResult(int playerHp)
        {
            if (playerHp > 0)
            {
                Console.WriteLine("Congratulations! You won!");
            }
            else
            {
                Console.WriteLine("You lose!");
            }
        }
    }
}
