using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels
{
    public class Appointment
    {
        public Guid Id { get; set; }

        public Guid? ObjectId { get; set; }
        public int Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? CreateTime { get; set; }
      //  public string? StockName { get; set; }
        public int? amount { get; set; }
        public ICollection<Order> Order { get; set; } = new List<Order>();
        public ICollection<Exhibition> Exhibitions { get; set; } = new List<Exhibition>();
    }
}

