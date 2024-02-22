using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Zaza.Telegram.CommandSystem;
using Zaza.Telegram.Components;

namespace Zaza.Telegram;

public class TelegramApp(ILogger<TelegramApp> logger, ComponentProvider componentProvider) {
    public async Task Run() {
        var token = await System.IO.File.ReadAllTextAsync("Token.txt") ??
            throw new ArgumentNullException("TelegramToken");
        var botClient = new TelegramBotClient(token);

        using CancellationTokenSource cts = new();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new() {
            AllowedUpdates = [] // receive all update types except ChatMember related updates
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await botClient.GetMeAsync();
        componentProvider.Setup(botClient, cts);

        logger.LogInformation($"Start listening for @{me.Username}");

        Console.ReadLine();

        // Send cancellation request to stop bot
        cts.Cancel();

    }

    private Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return Task.CompletedTask;
        // Only process text messages
        if (message.Text is not { } messageText)
            return Task.CompletedTask;

        var chatId = message.Chat.Id;
        var firstName = update.Message.Chat.FirstName;
        var lastName = update.Message.Chat.LastName;
        if (!string.IsNullOrWhiteSpace(lastName)) {
            lastName = " " + lastName;
        }
        logger.LogInformation($"Received a '{messageText}' message from {firstName}{lastName} in chat {chatId}.");
        ComponentManager.ExecuteAll(update);
        return Task.CompletedTask;
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
        var ErrorMessage = exception switch {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}
