using Microsoft.AspNetCore.SignalR;
using Newfactjo.Data;
using Newfactjo.Models;
using System.Threading.Tasks;

namespace Newfactjo.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            // ✅ حفظ الرسالة في قاعدة البيانات
            var chat = new ChatMessage
            {
                User = user,
                Message = message
            };

            _context.ChatMessages.Add(chat);
            await _context.SaveChangesAsync();

            // ✅ بث الرسالة لجميع المستخدمين
            await Clients.All.SendAsync("ReceiveMessage", user, message, chat.Timestamp);

        }
    }
}
