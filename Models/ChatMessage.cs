using System;

namespace Newfactjo.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string User { get; set; }  // اسم المستخدم
        public string Message { get; set; }  // نص الرسالة
        public DateTime Timestamp { get; set; } = DateTime.Now;  // وقت الرسالة
    }
}
