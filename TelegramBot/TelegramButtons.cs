using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    internal class TelegramButtons
    {
        private static List<string> buttonIcons = new List<string>
        {
            Emoji.GrinningFace,
            Emoji.GrinningFace,
            Emoji.GrinningFace,
            Emoji.GrinningFace,
            Emoji.GrinningFace,
            Emoji.GrinningFace,
        };
        private static List<string> buttonTexts = new List<string> { "test1", "test2", "test3", "test4", "test5", "test6" };
        private static List<string> callbackData = new List<string> { "@test1", "@test2", "@test3", "@test4", "@test5", "@test6" };

        public static InlineKeyboardMarkup? CreateInlineKeyboard()
        {
            //amount of icons, button text and callback data must be the same
            if (buttonTexts.Count != callbackData.Count && buttonTexts.Count != buttonIcons.Count)
            {
                return null;
            }
            var rows = new List<InlineKeyboardButton[]>();

            //rows of 3 buttons will be created
            //change i limit to make more or less buttons in a row
            for (int i = 0; i < buttonTexts.Count; i += 3)
            {
                var row = new InlineKeyboardButton[Math.Min(3, buttonTexts.Count - i)];

                for (int j = 0; j < row.Length; j++)
                {
                    row[j] = InlineKeyboardButton.WithCallbackData(buttonIcons[i + j] + " " + buttonTexts[i + j], callbackData[i + j]);
                }
                rows.Add(row);
            }
            return new InlineKeyboardMarkup(rows);
        }
    }
}
