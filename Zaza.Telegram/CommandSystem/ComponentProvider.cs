using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Zaza.Db.Repository;
using Zaza.Telegram.CommandSystem.Components;

namespace Zaza.Telegram.CommandSystem {
    public class ComponentProvider(UserRepository repository) {
        public List<IZazaTgComponent> Components { get; set; } = [
                new AdminComponentSetup(repository),
            ];

        public void Setup(TelegramBotClient telegramBotClient, CancellationTokenSource cancellationToken) {
            foreach (var component in Components) {
                component.Setup(telegramBotClient, cancellationToken);
            }
        }
    }
}
