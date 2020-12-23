using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace Millionare
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 40;
            Console.BufferHeight = 40;
            Console.WindowWidth = 140;
            Console.BufferWidth = 140;
            Console.Clear();
            string loopgame = "Y";
            while (loopgame.ToLower() == "y")
            {
                bool GamerOver = false;
                int lvl = 1;
                int difficulty = 1;
                int money = 50000;
                int multiply = 1;
                int chance = 0;
                int NumAns = 4;
                int NumAnsStore = NumAns;
                bool Shop = false;
                string AnsVar1 = "";
                string AnsVar2 = "";
                string TheAnswer = "";
                int half_chance = 1;
                while (!GamerOver && lvl<=15)
                {
                    Console.Clear();
                    TheQuestionCl current_quest = TheQuestions(lvl);
                    if (Shop)
                    {
                        ItemShop(ref money, ref  multiply, ref chance, ref NumAns, ref half_chance);
                        Shop = false;
                    }
                    MainScreen();
                    Console.WriteLine($"Money: {money}          Level: {lvl}            Difficulty: {difficulty}            Multiplier: {multiply}          Chances left: {chance}          Half Chances left: {half_chance}\n");
                    current_quest.PrintQuestion();
                    if (TheAnswer.ToLower() == "helper")
                    {
                        current_quest.HalfChanceMethod(ref AnsVar1);
                    }
                    else if (NumAns == 4)
                    {
                        current_quest.PrintAnswers(); 
                    }
                    else if (NumAns == 3)
                    {
                        current_quest.AfterBuyAns(ref AnsVar1, ref AnsVar2);
                    }

                    Console.WriteLine("\nType in your final Answer:");
                    TheAnswer = Convert.ToString(Console.ReadLine());
                    if (TheAnswer.ToLower() == "cheat")
                    { 
                        current_quest.CheatAns();
                        TheAnswer = current_quest.CorrAns;
                    }
                    if (TheAnswer.ToLower() == "helper")
                    {
                        if (half_chance < 0)
                        {
                            Console.WriteLine("You've ran out of option");
                        }
                        else
                        {
                            half_chance--;
                            continue;
                        }
                                   
                    }
                    while (!AnsCheck(TheAnswer, current_quest, NumAns, AnsVar1, AnsVar2) || TheAnswer.ToLower() == "cheat" || TheAnswer.ToLower() == "helper")
                    {
                        Console.WriteLine("\nThis answear doesn't exist!\nType a valid answear!\n");
                        TheAnswer = Console.ReadLine();
                    }
                    if (!current_quest.CorrChecker(TheAnswer, lvl, money, multiply, ref Shop, ref money))
                    {
                        if (chance == 0)
                        {
                            GamerOver = true;
                        }
                        else
                        {
                            chance--;
                            Console.WriteLine($"Your answer was wrong. \nYou have: {chance} chances left");
                            Console.WriteLine("Press any key to resume the game!");
                            Console.ReadKey(true);
                            Console.Clear();
                        }
                    }
                    else if (lvl % 5 == 0 && lvl <= 15)
                    {
                        difficulty++;
                        lvl++;
                        if (NumAns == 2)
                        {
                            NumAns = NumAnsStore;
                        }
                    }
                    else
                            lvl++;
                    if (NumAns == 2)
                    {
                        NumAns = NumAnsStore;
                    }
                }
                if (lvl == 16)
                {
                    Console.WriteLine("Congratulations!");
                    Console.WriteLine($"You've won - {money}");
                    loopgame = Loopgamer();
                }
                else if (GamerOver == true) { Console.WriteLine("Game Over! You answer was wrong. \nYou have wasted all your chances!"); loopgame = Loopgamer(); }
                Console.Write("Press any key to continue");
                Console.ReadKey(true);
            }
        }
        public static bool AnsCheck(string TheAnswer, TheQuestionCl ans_var, int NumAns, string AnsVar1, string AnsVar2)
        {
            switch (NumAns)
            {
                case 4:
                    if (TheAnswer.ToUpper() == ans_var.CorrAns.ToUpper() || TheAnswer.ToUpper() == ans_var.Answervar1.ToUpper() || TheAnswer.ToUpper() == ans_var.Answervar2.ToUpper() || TheAnswer.ToUpper() == ans_var.Answervar3.ToUpper())
                    {
                        return true;
                    }
                    else if (TheAnswer.ToUpper() == "A" || TheAnswer.ToUpper() == "B" || TheAnswer.ToUpper() == "C" || TheAnswer.ToUpper() == "D")
                    {
                        return true;
                    }
                    return false;
                case 3:
                    if (TheAnswer.ToUpper() == ans_var.CorrAns.ToUpper() || TheAnswer.ToUpper() == AnsVar1.ToUpper() || TheAnswer.ToUpper() == AnsVar2.ToUpper())
                    {
                        return true;
                    }
                    else if (TheAnswer.ToUpper() == "A" || TheAnswer.ToUpper() == "B" || TheAnswer.ToUpper() == "C")
                    {
                        return true;
                    }
                    return false;
                case 2:
                    if (TheAnswer.ToUpper() == ans_var.CorrAns.ToUpper() || TheAnswer.ToUpper() == AnsVar1.ToUpper())
                    {
                        return true;
                    }
                    else if (TheAnswer.ToUpper() == "A" || TheAnswer.ToUpper() == "B")
                    {
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }
        public static string FilePath_1(string file)
        {
             string nohd = Directory.GetCurrentDirectory() + "\\Questions\\" + file;
             return nohd;
        }
        public static TheQuestionCl TheQuestions(int level)
        {
            int difficulty = 1;
            int num = 1;
            if (level <= 5)
            {
                difficulty = 1;
                num = level;
            }
            else if (level <= 10)
            {
                difficulty = 2;
                num = level - 5;
            }
            else if (level <= 15)
            {
                difficulty = 3;
                num = level - 10;
            }
            string[] quest = ThePath(FilePath_1($"{difficulty}_{num}.txt"));
            return new TheQuestionCl(quest[0], quest[1], quest[2], quest[3], quest[4]);
        }
        public static string[] ThePath(string file)
        {
            string[] Arr = {};
            StreamReader sr = new StreamReader(file);
            while (!sr.EndOfStream)
            {
                Array.Resize(ref Arr, Arr.Length + 1);
                Arr[Arr.Length - 1] = sr.ReadLine();
            }
            sr.Close();
            return Arr;
        }
        public static void ItemShop(ref int money, ref int multiply, ref int chances, ref int NumAns, ref int half_chance)
        {
            if (money >= 5000)
            { 
                Console.WriteLine("\n                \nSHOP MENU\n               ");
                Console.WriteLine($"Money: {money}$\n");
                Console.WriteLine("Option		Items		    Price");
                Console.WriteLine("1.	    Buy 1 Chance            5000$");
                Console.WriteLine("2.	    Buy 2 Chances           7000$");
                Console.WriteLine("3.	    Buy 3 Chances           10000$");
                Console.WriteLine("4. 	    Add +1 50/50                12000$");
                Console.WriteLine("5.	    Money multiplier(2x)       15000$");
                Console.WriteLine("6.	    Money multiplier(3x)       17000$");
                Console.WriteLine("7. 	    Money multiplier(4x)       20000$");
                Console.WriteLine("8. 	    Remove 1 answer         25000$");
                Console.WriteLine("\nPress 0 to quit the shop!");
                while (true)
                {
                    int old_money_shop = money;
                    Console.WriteLine("Choose an option");
                    var optionChoice = Console.ReadLine();
                    switch (optionChoice)
                    {
                        case "1":
                            money -= 5000;
                            chances += 1;
                            break;
                        case "2":
                            money -= 7000;
                            chances += 2;
                            break;
                        case "3":
                            money -= 10000;
                            chances += 3;
                            break;
                        case "4":
                            money -= 12000;
                            half_chance += 1;
                            break;
                        case "5":
                            if (multiply >= 2)
                            {
                                Console.WriteLine("You cannot buy this pachage anymore. You either have it, or a better one.");
                            }
                            else
                            {
                                money -= 15000;
                                multiply = 2;
                            }
                            break;
                        case "6":
                            if (multiply >= 3)
                            {
                                Console.WriteLine("You cannot buy this pachage anymore. You either have it, or a better one.");
                            }
                            else
                            {
                                money -= 17000;
                                multiply = 3;
                            }
                            break;
                        case "7":
                            if (multiply >= 4)
                            {
                                Console.WriteLine("You cannot buy this pachage anymore. You either have it, or a better one.");
                            }
                            else
                            {
                                money -= 20000;
                                multiply = 4;
                            }
                            break;
                        case "8":
                            if (NumAns > 3)
                            {
                                money -= 25000;
                                NumAns = 3;
                            }
                            else
                            {
                                Console.WriteLine("You cannot buy this pachage anymore. You either have it, or a better one.");
                            }
                            break;
                        case "0":
                            Console.WriteLine("You have quit the Item Shop!");
                            break;
                        default:
                            Console.WriteLine("Please Answer only with one of the options!");
                            break;
                    }
                    if (money < 0)
                    {
                        money = old_money_shop;
                        Console.WriteLine("One of purchases was done wrong. Plz make a normal one");
                    }
                    else
                    {
                        if (optionChoice == "0")
                        {
                            Console.WriteLine("Thx for visiting");
                            Console.WriteLine("Press any key to resume the game!");
                            Console.ReadKey(true);
                            break;
                        }
                        Console.WriteLine($"\nYour current bank is - {money}");
                        Console.WriteLine("Thx for a purchase");
                    }
                    
                }
                Console.Clear();
            }
        }

        static void MainScreen()
        {
            Console.Write("                                                              ────────────────");
            string text = "Welcome To \nWho Wants To Be\nA Millionare";
            string[] lines = Regex.Split(text, "\r\n|\r|\n");
            int left = 0;
            int top = (Console.WindowHeight / 10) - (lines.Length / 2) - 2;
            int center = Console.WindowWidth / 2;
            for (int i = 0; i < lines.Length; i++)
            {
                left = center - (lines[i].Length / 2);
                Console.SetCursorPosition(left, top);
                Console.Write($"{lines[i]}\n");
                top = Console.CursorTop;
            }
            Console.WriteLine("                                                              ────────────────");
        }

        public static string Loopgamer()
        {
            Console.WriteLine("Do you wish to play again? Y/N");
            string loopgame = Console.ReadLine();
            while (loopgame.ToLower() != "y" && loopgame.ToLower() != "n")
            {
                Console.WriteLine("Please Answer only with Y/N!");
                loopgame = Console.ReadLine();
            }
            return loopgame; 
        }
    }


    public class TheQuestionCl
    {
        public string question;
        public string CorrAns;
        public string Answervar1;
        public string Answervar2;
        public string Answervar3;
        string CorrChar;

        public TheQuestionCl(string newQues, string correctansw, string answer1, string answer2, string answer3)
        {
            question = newQues;
            CorrAns = correctansw;
            Answervar1 = answer1;
            Answervar2 = answer2;
            Answervar3 = answer3;
        }
        
        public void PrintQuestion()
        {
            int left = 0;
            int top = (Console.WindowHeight / 10) + 4;
            int center = (Console.WindowWidth / 2) - question.Length/2 ;
            Console.SetCursorPosition(center, top);
            Console.WriteLine($"{question}\n");
        }

        public void PrintAnswers()
        {
            Random variantion = new Random();
            int i = variantion.Next(1, 9);
            switch (i)
            {
                case 1: { Console.WriteLine($"A.{CorrAns}		    B.{Answervar1}		        C.{Answervar2}		    D.{Answervar3}"); CorrChar = "A"; break; }
                case 2: { Console.WriteLine($"A.{CorrAns}		    B.{Answervar2}		        C.{Answervar3}		    D.{Answervar1}"); CorrChar = "A"; break; }
                case 3: { Console.WriteLine($"A.{Answervar2}		    B.{CorrAns}		        C.{Answervar1}		    D.{Answervar3}"); CorrChar = "B"; break; }
                case 4: { Console.WriteLine($"A.{Answervar3}		    B.{CorrAns}		        C.{Answervar2}		    D.{Answervar1}"); CorrChar = "B"; break; }
                case 5: { Console.WriteLine($"A.{Answervar1}		    B.{Answervar3}		     C.{CorrAns}		    D.{Answervar2}"); CorrChar = "C"; break; }
                case 6: { Console.WriteLine($"A.{Answervar2}		    B.{Answervar3}		     C.{CorrAns}		    D.{Answervar1}"); CorrChar = "C"; break; }
                case 7: { Console.WriteLine($"A.{Answervar1}		    B.{Answervar3}		     C.{Answervar2}		    D.{CorrAns}"); CorrChar = "D"; break; }
                case 8: { Console.WriteLine($"A.{Answervar2}		    B.{Answervar3}		     C.{Answervar1}		    D.{CorrAns}"); CorrChar = "D";  break; }
            }
        }
        public bool CorrChecker(string Answer, int level, int oldmoney, int multiplier, ref bool Shop, ref int newmoney)
        {
            newmoney = oldmoney;
            if (Answer.ToUpper() == CorrAns.ToUpper() || Answer.ToUpper() == CorrChar)
            {
                if (level < 5)
                {
                    newmoney = level * 500 * multiplier + oldmoney;
                }
                else if (level >= 5 && level < 10)
                {
                    newmoney = level * 1000 * multiplier + oldmoney;
                }
                else if (level >= 10 && level < 15)
                {
                    newmoney = level * 5000 * multiplier + oldmoney;
                }
                Console.WriteLine($"You won: {newmoney - oldmoney}$");
                Console.WriteLine("\nPress a key: \n");
                if (newmoney >= 5000)
                {
                    Console.WriteLine("B -> Open the Item Shop");
                }
                Console.WriteLine("Any other key to resume!");
                string ShopAnswer = Convert.ToString(Console.ReadLine());
                if (ShopAnswer.ToUpper() == "B" && newmoney >= 5000)
                {
                    Shop = true;
                }
                else
                {
                    Shop = false;
                }
                Console.Clear();
                return true;
            }
            return false;
        }
        public void AfterBuyAns(ref string AnsVar1, ref string AnsVar2)
        {
            Random i = new Random();
            int inner_ran = i.Next(1, 4);
            switch (inner_ran)
            {
                case 1:
                        AnsVar1 = Answervar1;
                        AnsVar2 = Answervar3;
                        Console.WriteLine("A.{0}		B.{1}		C.{2}", CorrAns, AnsVar1, AnsVar2);
                        CorrChar = "A";
                        break;
                case 2:
                        AnsVar1 = Answervar1;
                        AnsVar2 = Answervar2;
                        Console.WriteLine("A.{0}		B.{1}		C.{2}", AnsVar1, CorrAns, AnsVar2);
                        CorrChar = "B";
                        break;
                case 3:
                        AnsVar1 = Answervar2;
                        AnsVar2 = Answervar3;
                        Console.WriteLine("A.{0}		B.{1}		C.{2}", AnsVar1, AnsVar2, CorrAns);
                        CorrChar = "C";
                        break;
            }
        }
        public string CheatAns()
        {
            Console.WriteLine(CorrAns);
            return CorrAns;
        }
        public void HalfChanceMethod(ref string AnsVar1)
        {
            Random i = new Random();
            int inner_ran = i.Next(1, 4);
            int print_answ = i.Next(1, 3);
            switch (inner_ran)
            {
                case 1:
                    AnsVar1 = Answervar1;
                    break;
                case 2:
                    AnsVar1 = Answervar2;
                    break;
                case 3:
                    AnsVar1 = Answervar3;
                    break;
            }
            switch (print_answ)
            {
                case 1:
                    Console.WriteLine($"A.{CorrAns}		B.{AnsVar1}");
                    CorrChar = "A";
                    break;
                case 2:
                    Console.WriteLine($"A.{AnsVar1}		B.{CorrAns}") ;
                    CorrChar = "B";
                    break;
            }
        }
    }
}
