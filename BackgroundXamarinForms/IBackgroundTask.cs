using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundXamarinForms
{
    public interface IBackgroundTask
    {
        Task StartJob();
    }
}
