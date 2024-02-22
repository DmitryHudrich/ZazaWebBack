using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Zaza.Db.Repository;
using Zaza.Telegram.Components;

namespace Zaza.Telegram.CommandSystem.Components;

internal class AdminComponentSetup(UserRepository repository) : IZazaTgComponent {
    public void Setup(TelegramBotClient botClient, CancellationTokenSource cts) {
        ComponentManager
            .Begin("/start", async (Update update) => {
                await botClient.SendTextMessageAsync(
                    chatId: update.Message!.Chat.Id,
                    text: "секс робот",
                    parseMode: ParseMode.MarkdownV2,
                    disableNotification: true,
                    replyToMessageId: update.Message.MessageId,
                    replyMarkup: new ReplyKeyboardMarkup(
                        ["/add", "/get"]) { ResizeKeyboard = true },
                    cancellationToken: cts.Token);
            })
            .End();

        /*
         * 
         * не работает!
         * 
         * 
         */
        string name = string.Empty;
        string login = string.Empty;
        string password = string.Empty;
        ComponentManager
            .Begin("/add", async (Update update) => {
                await botClient.SendTextMessageAsync(
                    chatId: update.Message!.Chat.Id,
                    text: "Имя",
                    disableNotification: true,
                    cancellationToken: cts.Token);
            })
            .Then((Update update) => {
                name = update.Message.Text;
            })
            .Then(async (Update update) => {
                await botClient.SendTextMessageAsync(
                    chatId: update.Message!.Chat.Id,
                    text: "Логин",
                    disableNotification: true,
                    cancellationToken: cts.Token);
            })
            .Then((Update update) => {
                login = update.Message.Text;
            })
            .Then(async (Update update) => {
                await botClient.SendTextMessageAsync(
                    chatId: update.Message!.Chat.Id,
                    text: "Пароль",
                    disableNotification: true,
                    cancellationToken: cts.Token);
            })
            .Then(async (Update update) => {
                password = update.Message.Text;
                await repository.InsertUserAsync(new Notes.Entities.User(Guid.NewGuid(), name, login, new Notes.Entities.Password(password)));
            })
            .End();
    }
}
