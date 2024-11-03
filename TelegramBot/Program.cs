using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    class Program
    {
        #region properties

        public static TelegramBotClient? Client;

        //properties can be stored in code, but there is an example how to store them in xml file
        //get strored in XML telegram key
        private static readonly string? telegramBotKey = SourceConnection.LoadPropertiesFromXML("telegramBotKey");

        //get stored in XML telegram chat id
        private static readonly string? yourTelegramChannelId = SourceConnection.LoadPropertiesFromXML("yourTelegramChannelId");

        //create buttons
        private static InlineKeyboardMarkup? inlineButtons = TelegramButtons.CreateInlineKeyboard();

        #endregion

        #region main
        static void Main(string[] args)
        {
            Client = new TelegramBotClient(telegramBotKey);
            Client.StartReceiving(Update, Error);

            //possible daily task
            ExecuteDailyTask();

            //infinite waiting period
            Thread.Sleep(-1);
        }

        #endregion

        #region methods
        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;

            if (message != null && message.Text != null)
            {
                try
                {
                    switch (message.Text)
                    {
                        case "@bot":
                            await botClient.SendTextMessageAsync(
                                message.Chat.Id,
                                "bot message\r\n" +
                                "second bot message",
                                replyMarkup: inlineButtons);
                            break;
                        //add here more cases if needed

                        case var text when text.StartsWith("@botcommand") && text.Length > 12:
                            // remove first 12 characters
                            //if commands are shriter or longer, change the amount of removed characters
                            string messagetobot = message.Text[12..];
                            //handle here commands. add more cases if needed
                            break;

                    }
                }
                catch (Exception)
                {
                    //add your code to catch errors and write logs
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Error");
                }
            }

            //handle here button clicks
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
            {
                try
                {
                    //get button callback command
                    var callbackText = update.CallbackQuery.Data;

                    //get telegram chat id for answer
                    var currentCallbackChatId = update.CallbackQuery.Message.Chat.Id;

                    var textCallbacks = new Dictionary<string, Func<string>>
                        {
                            { "@test1", () => WorkerCollection.GetFirstTestMessage() },
                            { "@test2", () => WorkerCollection.GetSecondTestMessage() },
                            { "@test3", () => WorkerCollection.GetThirdTestMessage() },
                            { "@test4", () => WorkerCollection.GetFourthTestMessage() },
                            { "@test5", () => WorkerCollection.GetFifthTestMessage() },
                            { "@test6", () => WorkerCollection.GetSixthTestMessage() },
                        };

                    foreach (var entry in textCallbacks)
                    {
                        //get the function call
                        if (callbackText.Contains(entry.Key))
                        {
                            await botClient.SendTextMessageAsync(currentCallbackChatId, entry.Value());
                            return;
                        }
                    }
                }
                catch (Exception)
                {
                    //add your code to catch errors and write logs
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Error");
                }
            }
        }

        public static void ExecuteDailyTask()
        {
            DateTime now = DateTime.Now;
            int[] scheduledHours = { 12, 18 }; // Scheduled times: 12:00 and 18:00.add/remove/change if needed

            foreach (int hour in scheduledHours)
            {
                DateTime scheduledTime = new DateTime(now.Year, now.Month, now.Day, hour, 0, 0, 0);
                if (now > scheduledTime)
                {
                    scheduledTime = scheduledTime.AddDays(1);
                }
                double timeToGo = (scheduledTime - now).TotalMilliseconds;
                System.Timers.Timer timer = new System.Timers.Timer(timeToGo);
                timer.AutoReset = false;

                timer.Elapsed += (_, _) =>
                {
                    PossibleDailyTask();

                    timer.Interval = TimeSpan.FromDays(1).TotalMilliseconds;
                    timer.Start();
                };

                timer.Start();
            }
        }

        private static void PossibleDailyTask()
        {
            Client.SendTextMessageAsync(yourTelegramChannelId, WorkerCollection.GetDailyMessage()).Wait();
        }

        async static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            //empty
            //add your code to catch errors and write logs
            return;
        }

        #endregion
    }
}
