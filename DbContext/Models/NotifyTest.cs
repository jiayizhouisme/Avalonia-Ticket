using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Context.Models
{
    public class NotifyTest : INotifyCompletion
    {
        private Action _continuation = null;
        public bool IsCompleted => _isCompleted;
        private bool _isCompleted = false;
        public int i = 0;
        public NotifyTest GetResult()
        {
            Console.WriteLine("调用GetResult以获取结果");
            i = 1;
            return this;
        }

        public void Complete()
        {
            _continuation();
        }

        public void OnCompleted(Action continuation)
        {
            this._isCompleted = true;
            this._continuation = continuation;
        }

        public NotifyTest GetAwaiter()
        {
            Console.WriteLine("获得Awaiter");
            return this;
        }
    }
}
