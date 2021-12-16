using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using FileIO = System.IO.File;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {
        
        private static string _token = "";
        public static TelegramBotClient telegramBotClient;
        private static Counter _counter;
        static void Main(string[] args)
        {
            string a = GetToken();

            //string a = System.IO.Path.GetDirectoryName($"./Assembly.GetExecutingAssembly().Location");
            _counter = new Counter();
            int timeLeft;
            _counter.SetDefaultTargetTime();
            telegramBotClient = new TelegramBotClient(_token);

            telegramBotClient.OnMessage+= BotOnMessageRecived;
            telegramBotClient.StartReceiving();
            Console.WriteLine("Бот запущен");
            Console.ReadKey();
            telegramBotClient.StopReceiving();
            Console.WriteLine("Бот остановлен");
            //while (true)
            {
                // Console.Clear();
                // _counter.UpdateTime();
                // timeLeft  = _counter.GetSecondsLeft();         
                // Console.WriteLine($"Осталось: {timeLeft}");
                // Thread.Sleep(500);

            }
        }
        private static async void BotOnMessageRecived(object sender, MessageEventArgs messageEventArgs)
        {
            string responseText = "";
            var message = messageEventArgs.Message;
            
            Console.WriteLine($"Сообщение боту ");
            if(message?.Type == MessageType.Text)
            {
                Console.WriteLine("Текстовое сообщение боту");
                
                Console.WriteLine($"Текст сообщения:{message.Text}");
                if (message.Text.Equals("/time"))
                {
                    _counter.UpdateTime();
                    int timeLeft  = _counter.GetSecondsLeft();         
                    
                    responseText = $"Осталось: {timeLeft} секунд.";
                    await telegramBotClient.SendTextMessageAsync(messageEventArgs.Message.Chat.Id,$"{responseText}");
                }
                else if (message.Text.Equals("/mood"))
                {
                    
                    responseText = $"Хорошо";
                    await telegramBotClient.SendTextMessageAsync(messageEventArgs.Message.Chat.Id,$"{responseText}");
                }
                else if(message.Text.Equals("/fulltime"))
                {
                    _counter.UpdateTime();
                    
                    responseText = $"{_counter.ToString()}";
                    await telegramBotClient.SendTextMessageAsync(messageEventArgs.Message.Chat.Id,$"{responseText}");
                }
                else
                {
                    responseText = $"Нет ответа!";
                    await telegramBotClient.SendTextMessageAsync(messageEventArgs.Message.Chat.Id,$"{responseText}");
                }
                Console.WriteLine($"ответ бота:{responseText}");
            }
        }
        public class Tokens
        {
            public string Token;
        }
        public static string GetToken()
        {
            string result;
            var assembly = Assembly.GetExecutingAssembly();
            var resourseName = "ConsoleApp1.Token2.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourseName))
            using (StreamReader reader = new StreamReader(stream)) 
            {
                result = reader.ReadToEnd();
            }
            //string jsonFile = FileIO.ReadAllText(result);
            var token = JsonConvert.DeserializeObject<Tokens>(result);
            return token.Token.ToString();

        }
    }

    public class Firday
    {
        private bool _isFriday;
        private DateTime _lastDay;



    }


    class Counter
    {
        private DateTime _currentTime = new DateTime();
        private DateTime _targetTime = new DateTime();

        public TimeSpan Time {get;private set;}
        

        public Counter()
        {
            UpdateTime();
        }
        private void SetCurrentTime()
        {
            _currentTime = DateTime.Now;
        }
        public void SetDefaultTargetTime()
        {
            _targetTime = new DateTime(2021,10,19);
        }
        public TimeSpan GetTimeToHome()
        {
            return _targetTime.Subtract(_currentTime);
        } 
        public void UpdateTime()
        {
            SetCurrentTime();
            Time = GetTimeToHome();
        }
        public int GetSecondsLeft()
        {
            int hoursLeft = (Time.Days * 24)+Time.Hours;
            int minutesLeft = hoursLeft * 60+Time.Minutes;
            int secondsLeft = minutesLeft*60+ Time.Seconds;
                return secondsLeft;
        }
        public override string ToString()
        {
            string fullTime = $"Дней осталось: {Time.Days}\nЧасов осталось: {Time.Hours}\nМинут осталось: {Time.Minutes}\nСекунд осталось: {Time.Seconds}";
            return fullTime;
        }
    }
    
}
