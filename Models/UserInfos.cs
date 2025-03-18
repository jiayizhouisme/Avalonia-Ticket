using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;

namespace GetStartedApp.ViewModels
{
    public class UserInfos : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long? UserId { get; set; }

        [Required(ErrorMessage = "姓名不能为空")]
        [StringLength(50, ErrorMessage = "姓名不能超过50个字符")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "手机号不能为空")]
        [StringLength(11, ErrorMessage = "手机号必须是11位", MinimumLength = 11)]
        [RegularExpression(@"^1[3 - 9]\d{9}$", ErrorMessage = "手机号格式不正确")]
        public string? PhoneNumber { get; set; }
        public override string ToString() => Name ?? "未命名游客";

        [Required(ErrorMessage = "身份证号不能为空")]
        [StringLength(18, ErrorMessage = "身份证号必须是18位", MinimumLength = 15)]
        public string? IdCard { get; set; }
        public virtual User User { get; set; }
        private bool _isDetailsVisible;

        [NotMapped]
        public bool IsDetailsVisible//添加成功
        {
            get => _isDetailsVisible;
            set
            {
                if (_isDetailsVisible != value)
                {
                    _isDetailsVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}