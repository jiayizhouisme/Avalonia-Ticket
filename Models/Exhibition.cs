using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GetStartedApp.ViewModels
{
    public class Exhibition
    {
        public Guid id { get; set; }
        [Comment("景区名")]
        public string name { get; set; }
        [Comment("景区介绍")]
        public string description { get; set; }
        [Comment("景区封面")]
        public string imgs { get; set; }
        public int status { get; set; }

        [Comment("景区提前预约天数")]
        public int? beforeDays { get; set; }

        public bool isDeleted { get; set; }
        [Comment("创建日期")]
        public DateTime CreateTime { get; set; }
        [Comment("价格")]
        public decimal basicPrice { get; set; }
        public int totalAmount { get; set; }
        public string exhibitions { get; set; }

        public bool isMultiPart { get; set; }
        public string forbiddenRule { get; set; }
        [NotMapped]
        public DateTime MaxBookingDate => DateTime.Today.AddDays((beforeDays ?? 7) - 1);
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        [NotMapped]
        public string FullDescription => description;
        public string ImagePath => $"avares://GetStartedApp/Assets/{imgs}";
        [NotMapped]
        public Bitmap DisplayImage
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(imgs))
                    {
                        var uri = new Uri($"avares://GetStartedApp/Assets/{imgs}");
                        Console.WriteLine($"尝试加载图片: {uri}");
                        using var stream = AssetLoader.Open(uri);
                        return new Bitmap(stream);
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"图片加载失败: {ex.Message}");
                    return null;
                }
            }
        }
    }
}



