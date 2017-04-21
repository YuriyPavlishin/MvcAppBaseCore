using System;

namespace BaseApp.Data.Transaction.Actions
{
    public class GenericAction : ActionBase
    {
        private Action Action { get; set; }

        public GenericAction(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            Action = action;
        }

        public override void Execute()
        {
            Action();
        }
    }
}
