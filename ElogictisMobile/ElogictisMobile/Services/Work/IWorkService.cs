using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ElogictisMobile.Models;

namespace ElogictisMobile.Services.Work
{
    public interface IWorkService
    {
        Task<bool> LogWorkAsync(WorkItem item);
        Task<ObservableCollection<WorkItem>> GetTodaysWorkAsync();
        Task<List<WorkItem>> GetWorkForThisPeriodAsync();
    }
}
