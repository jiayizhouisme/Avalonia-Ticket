using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Ioc;


namespace GetStartedApp.ViewModels
{
    public class PaymentPageViewModel : ViewModelBase
    {
        private string _pageTitle = "支付页面";
        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetProperty(ref _pageTitle, value); }
        }

        private ObservableCollection<OrderViewModel> _filteredOrders = new ObservableCollection<OrderViewModel>();
        public ObservableCollection<OrderViewModel> FilteredOrders
        {
            get { return _filteredOrders; }
            set { SetProperty(ref _filteredOrders, value); }
        }

        private string _selectedFilter;
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set { SetProperty(ref _selectedFilter, value); }
        }

        public DelegateCommand<object> FilterOrdersCommand { get; private set; }
        public DelegateCommand<OrderViewModel> UseTicketCommand { get; private set; }
        public DelegateCommand<OrderViewModel> RefundTicketCommand { get; private set; }

        public PaymentPageViewModel(IContainerExtension container)
            : base(container)
        {
            FilterOrdersCommand = new DelegateCommand<object>(FilterOrders);
            UseTicketCommand = new DelegateCommand<OrderViewModel>(UseTicket, CanUseTicket);
            RefundTicketCommand = new DelegateCommand<OrderViewModel>(RefundTicket, CanRefundTicket);
            FilterOrders("全部");
        }

        private bool CanUseTicket(OrderViewModel order)
        {
            return order?.Status == OrderStatus.已付款 && order.TicketCount > 0;
        }

        private bool CanRefundTicket(OrderViewModel order)
        {
            return order?.Status == OrderStatus.已付款;
        }

        private async void FilterOrders(object parameter)
        {
            string filter = parameter as string;
            SelectedFilter = filter;

            using var dbContext = new DatabaseContext();
            var orders = await dbContext.Order
                .Include(o => o.Appointment) 
                .ThenInclude(a => a.Exhibition) 
                .ToListAsync();

            switch (filter)
            {
                case "全部":
                    FilteredOrders.Clear();
                    foreach (var o in orders)
                    {
                        FilteredOrders.Add(new OrderViewModel(o));
                    }
                    break;
                case "待付款":
                    FilteredOrders.Clear();
                    var unpaidOrders = orders.Where(o => o.Status == OrderStatus.未付款).ToList();
                    foreach (var o in unpaidOrders)
                    {
                        FilteredOrders.Add(new OrderViewModel(o));
                    }
                    break;
                case "已使用":
                    FilteredOrders.Clear();
                    var usedOrders = orders.Where(o => o.Status == OrderStatus.已付款 && o.Tickets.All(t => t.Status == TicketStatus.已使用)).ToList();
                    foreach (var o in usedOrders)
                    {
                        FilteredOrders.Add(new OrderViewModel(o));
                    }
                    break;
                case "未使用":
                    FilteredOrders.Clear();
                    var unusedOrders = orders.Where(o => o.Status == OrderStatus.已付款 && o.Tickets.All(t => t.Status == TicketStatus.未使用)).ToList();
                    foreach (var o in unusedOrders)
                    {
                        FilteredOrders.Add(new OrderViewModel(o));
                    }
                    break;
                case "已退款":
                    FilteredOrders.Clear();
                    var refundedOrders = orders.Where(o => o.Status == OrderStatus.已退款).ToList();
                    foreach (var o in refundedOrders)
                    {
                        FilteredOrders.Add(new OrderViewModel(o));
                    }
                    break;
            }
        }
        private async void UseTicket(OrderViewModel order)
        {
            if (order == null) return;

            using var dbContext = new DatabaseContext();
            var ticketsToUpdate = dbContext.Ticket.Where(t => t.OrderId == order.Trade_No && t.Status == TicketStatus.未使用);
            foreach (var ticket in ticketsToUpdate)
            {
                ticket.Status = TicketStatus.已使用;
            }
            await dbContext.SaveChangesAsync();
            FilterOrders("全部");
        }

        private async void RefundTicket(OrderViewModel order)
        {
            if (order == null) return;

            using var dbContext = new DatabaseContext();
            var orderToUpdate = await dbContext.Order.FirstOrDefaultAsync(o => o.Trade_No == order.Trade_No);
            if (orderToUpdate != null)
            {
                orderToUpdate.Status = OrderStatus.已退款;
                await dbContext.SaveChangesAsync();
            }
            FilterOrders("全部");
        }
    }
}