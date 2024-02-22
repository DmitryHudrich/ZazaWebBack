using Telegram.Bot;

namespace Zaza.Telegram.CommandSystem.Components; 
public interface IZazaTgComponent {
    void Setup(TelegramBotClient botClient, CancellationTokenSource cts);
}