using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reservation.Commands
{
    public abstract class AsyncCommandBase : CommandBase
    {
        public bool _isExecuting;
        public bool IsExecuting
        { 
            get => _isExecuting; 
            set
            {
                _isExecuting = value;
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !IsExecuting && base.CanExecute(parameter);
        }
        public override async void Execute(object? parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);
            }
            finally
            { 
               IsExecuting = false;
            }
        }
        public abstract Task ExecuteAsync(object? parameter);
    }
}
