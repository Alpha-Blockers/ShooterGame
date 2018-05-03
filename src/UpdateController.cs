using System.Collections.Generic;

namespace ShooterGame
{
    static class UpdateController
    {

        static private List<IUpdate> _list = new List<IUpdate>();
        static private Queue<IUpdate> _toAdd = new Queue<IUpdate>();
        static private Queue<IUpdate> _toRemove = new Queue<IUpdate>();

        /// <summary>
        /// Add the class to the list of things to be updated.
        /// </summary>
        /// <param name="u">The class to be added</param>
        static public void Add(IUpdate u)
        {
            _toAdd.Enqueue(u);
        }

        /// <summary>
        /// Run the update methods for all linked classes.
        /// Also check if any classes need to be added or removed from the list.
        /// </summary>
        static public void Flush()
        {
            // Add classes to the main update list as required
            while (_toAdd.Count > 0)
            {
                IUpdate u = _toAdd.Dequeue();
                if (u.ShouldUpdate && !_list.Contains(u))
                    _list.Add(u);
            }

            // Loop through list of updateable classes and run their update method
            foreach (IUpdate u in _list)
            {
                if (u.ShouldUpdate)
                    u.Update();
                else
                    _toRemove.Enqueue(u);
            }

            // Remove classes from the main update list as required
            while (_toAdd.Count > 0)
            {
                IUpdate u = _toAdd.Dequeue();
                if (!u.ShouldUpdate)
                    _list.Add(u);
            }
        }
    }
}
