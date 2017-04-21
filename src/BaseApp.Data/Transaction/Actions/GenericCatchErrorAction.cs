using System;

namespace BaseApp.Data.Transaction.Actions
{
    public class GenericCatchErrorAction : ActionBase
    {
        private Action Action { get; set; }
        private string CatchErrorMessage { get; set; }

        public GenericCatchErrorAction(Action action, string catchErrorMessage)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (string.IsNullOrWhiteSpace(catchErrorMessage))
                throw new ArgumentNullException(nameof(catchErrorMessage));

            Action = action;
            CatchErrorMessage = catchErrorMessage;
        }

        public override void Execute()
        {
            try
            {
                Action();
            }
            catch (Exception ex)
            {
                //TODO or remove
                //LogHolder.MainLog.ErrorException(CatchErrorMessage, ex);
            }
        }
    }
}
