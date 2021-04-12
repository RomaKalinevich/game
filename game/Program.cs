using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace game
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] atribut = args;
            //Половина массива
            int count = atribut.Length / 2;
            //Массив содержит элементы которые побеждают
            string[] winItem = new string[count];

            //Проверка корректности данных
            if (checkInput(atribut.Length) == false)
            {
                return;
            }
            string resultMessage = "";

            //Получить ключ
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[16];
            provider.GetBytes(byteArray);

            //Сохранить ключ как строку
            string key = BitConverter.ToString(byteArray).Replace("-", string.Empty);

            //Ход компьютера
            Random rnd = new Random();
            int choosePC = rnd.Next(0, atribut.Length);

            //Элемент следующий за выбранным компьютером
            int startElem = choosePC + 1;

            //Получить побеждающие элементы
            winItem = chooseWinningElem(atribut, count, startElem);

            Console.WriteLine("HMAC:");
            Console.WriteLine(HMACHASH(atribut[choosePC].ToString(), key));

            //Вывод элементов для хода
            printElement(atribut);

            Console.WriteLine("Enter your move: ");
            int userChoose = Convert.ToInt32(Console.ReadLine()) - 1;
            Console.WriteLine($"Your move: {atribut[userChoose].ToString()}");
            Console.WriteLine($"PC move: {atribut[choosePC].ToString()}");

            resultMessage = game(atribut, winItem, userChoose, choosePC, count);
            Console.WriteLine(resultMessage);

            Console.WriteLine($"KEY - {key}");
        }

        //Функция проверяет входные данные
        public static bool checkInput(int countElem)
        {
            if (countElem % 2 == 0 || countElem < 3)
            {
                Console.WriteLine("Incorrect input!");
                Console.WriteLine("Please, enter at least 3 items");
                Console.WriteLine("Example : rock scissors paper ship star");
                return false;
            }
            else
            {
                return true;
            }
        }

        //Функция возвращает элементы которые побеждают в игре
        public static string[] chooseWinningElem(string[] array, int count, int startElem)
        {
            string[] userItem = array;
            string[] winItem = new string[count]; 
            for (int i = 0; i < count; i++)
            {
                if (startElem == userItem.Length)
                {
                    startElem = 0;
                }
                winItem[i] = userItem[startElem];
                startElem++;
            }
            return winItem;
        }
        public static void printElement(string[] atribut)
        {
            for (int i = 0; i < atribut.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {atribut[i]}");
            }
        }

        //Функция возвращает hash от ключа
        public static string HMACHASH(string str, string key)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            using (var hmac = new HMACSHA256(bkey))
            {
                byte[] bstr = Encoding.Default.GetBytes(str);
                var bhash = hmac.ComputeHash(bstr);
                return BitConverter.ToString(bhash).Replace("-", string.Empty);
            }
        }


        //Функция возвращает сообщение с результатом игры
        public static string game(string[] atribut, string[] winItem, int userChoose, int choosePC, int count)
        {
            string resultMessage = "";
            if (atribut[userChoose] == atribut[choosePC])
            {
                resultMessage = "Draw!";
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if (atribut[userChoose] == winItem[i])
                    {
                        resultMessage = "You win!";
                        break;
                    }
                    else
                    {
                        resultMessage = "You lose";
                    }
                }
            }
            return resultMessage;
        }
    }
}
