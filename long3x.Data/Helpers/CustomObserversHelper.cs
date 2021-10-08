using System.Collections.Generic;
using long3x.Data.Interfaces;

namespace long3x.Data.Helpers
{
    public class CustomObserversHelper: ICustomObserversHelper
    {
        private readonly IEnumerable<ICustomObserver> observers;

        public CustomObserversHelper(IEnumerable<ICustomObserver> observers)
        {
            this.observers = observers;
        }

        public void UpdateAll()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }
    }
}
