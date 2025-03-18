using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels
{
    public enum TicketStatus
    {
        未使用 = 0,
        部分使用,
        已使用,
        未激活,
        已冻结
    }
    public class Ticket
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int TUserId { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreateTime { get; set; }

        [ForeignKey("UserInfo")]
        public long UserInfoId { get; set; }

        public virtual UserInfos UserInfo { get; set; }
        public Guid? AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }
    }
}

