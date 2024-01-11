using System;
using System.Threading.Tasks;
using BaseApp.Data.Infrastructure;
using BaseApp.Web.Code.Scheduler.DataModels;
using BaseApp.Web.Code.Scheduler.Queue.Workers;
using BaseApp.Web.Code.Scheduler.SchedulerModels;

namespace BaseApp.Web.Code.Scheduler
{
    public class SchedulerService: ISchedulerService
    {
        private readonly ISchedulerWorkerService _schedulerWorkerService;
        private readonly IEmailWorkerService _emailWorkerService;
        private readonly Func<IUnitOfWorkPerCall> _unitOfWorkPerCallFunc;

        public SchedulerService(ISchedulerWorkerService schedulerWorkerService, IEmailWorkerService emailWorkerService, Func<IUnitOfWorkPerCall> unitOfWorkPerCallFunc)
        {
            _schedulerWorkerService = schedulerWorkerService;
            _emailWorkerService = emailWorkerService;
            _unitOfWorkPerCallFunc = unitOfWorkPerCallFunc;
        }

        protected SchedulerService()
        {
        }

        public void ScheduleAction<T>(T schedulerModel) where T : SchedulerModelBase
        {
            SaveSchedulerData(schedulerModel);
            _schedulerWorkerService.WakeUp();
        }

        public async Task EmailSynchronizedAsync<T>(T schedulerModel) where T : SchedulerModelBase
        {
            var schedulerData = SaveSchedulerData(schedulerModel, true);
            await _schedulerWorkerService.ProcessSchedulerSynchronizedAsync(schedulerData);
            await _emailWorkerService.ProcessSchedulerSynchronizedAsync(schedulerData.Id);
        }

        private SchedulerData SaveSchedulerData<T>(T schedulerModel, bool isSync = false)
            where T : SchedulerModelBase
        {
            var schedulerData = schedulerModel.BuildSchedulerData();

            Data.DataContext.Entities.Scheduler scheduler;
            using (var unitOfWork = _unitOfWorkPerCallFunc())
            {
                scheduler = unitOfWork.Schedulers.CreateEmpty();
                MapScheduler(schedulerData, scheduler);
                scheduler.IsSynchronous = isSync;
                unitOfWork.SaveChanges();
            }

            schedulerData.Id = scheduler.Id;

            return schedulerData;
        }

        private void MapScheduler(SchedulerData schedulerData, Data.DataContext.Entities.Scheduler destination)
        {
            destination.CreatedDate = DateTime.Now;
            destination.SchedulerActionType = schedulerData.SchedulerActionType;
            destination.CreatedByUserId = schedulerData.CreatedByUserId;
            destination.OnDate = schedulerData.OnDate;
            destination.EntityId1 = schedulerData.EntityId1;
            destination.EntityId2 = schedulerData.EntityId2;
            destination.EntityId3 = schedulerData.EntityId3;
            destination.EntityId4 = schedulerData.EntityId4;
            destination.EntityData1 = schedulerData.EntityData1;
            destination.EntityData2 = schedulerData.EntityData2;
            destination.EntityData3 = schedulerData.EntityData3;
            destination.EntityData4 = schedulerData.EntityData4;

        }
    }
}