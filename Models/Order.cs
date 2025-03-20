using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace GetStartedApp.ViewModels
{
    public enum OrderStatus
    {
        未付款 = 0,
        已付款 = 1,
        已关闭 = 2,
        已退款 = 3,
        一部分退款 = 4,
        支付中 = 5,
        退款中 = 6
    }
    public class Order
    {

        public DateTime? CreateTime { get; set; }
        [Key]
        public long? Trade_No { get; set; }
        public long? UserId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PayedAmount { get; set; }
        public string? ObjectId { get; set; }
        public DateTime expireDate { get; set; }
        public DateTime payedTime { get; set; }
        public OrderStatus? Status { get; set; }
        // public TicketStatus UseStatus { get; set; }
        [ForeignKey("Appointment")]
        public Guid? AppointmentId { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public virtual Appointment Appointment { get; set; }

    
    }

}