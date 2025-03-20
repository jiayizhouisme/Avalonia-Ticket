using Prism.Mvvm;
using System.Linq;

namespace GetStartedApp.ViewModels
{
    public class OrderViewModel : BindableBase
    {
        private long _tradeNo;
        public long Trade_No
        {
            get { return _tradeNo; }
            set { SetProperty(ref _tradeNo, value); }
        }

        private string _objectName;
        public string ObjectName
        {
            get { return _objectName; }
            set { SetProperty(ref _objectName, value); }
        }

        private int _ticketCount;
        public int TicketCount
        {
            get { return _ticketCount; }
            set { SetProperty(ref _ticketCount, value); }
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }

        private OrderStatus? _status;
        public OrderStatus? Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private string _ticketStatusText;
        public string TicketStatusText
        {
            get { return _ticketStatusText; }
            set { SetProperty(ref _ticketStatusText, value); }
        }
        public OrderViewModel(Order order)
        {
            Trade_No = order.Trade_No ?? 0;
            ObjectName = order.Appointment?.Exhibition?.name ?? "未知景区"; 
            TicketCount = order.Tickets?.Count ?? 0;
            TotalPrice = order.Amount ?? 0;
            Status = order.Status;

            TicketStatusText = GetTicketStatusText(order);
        }
        private string GetTicketStatusText(Order order)
        {
            if (order.Status == OrderStatus.未付款)
                return "待付款";

            if (order.Status == OrderStatus.已退款)
                return "已退款";

            if (order.Tickets == null || !order.Tickets.Any())
                return "无票";

            if (order.Tickets.All(t => t.Status == TicketStatus.已使用))
                return "已使用";

            if (order.Tickets.All(t => t.Status == TicketStatus.未使用))
                return "未使用";

            return "全部";
        }
    }
}