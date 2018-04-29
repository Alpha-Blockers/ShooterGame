using System.Collections.Generic;

namespace ShooterGame
{
    static class UpdateManager
    {

        static private List<IUpdate> _list = new List<IUpdate>();
        static private Queue<IUpdate> _changed = new Queue<IUpdate>();
        
        /// <summary>
        /// Inform that update manager that the ShouldUpdate status of the class has changed.
        /// </summary>
        /// <param name="u">The class to be checked</param>
        static public void Changed(IUpdate u)
        {
            _changed.Enqueue(u);
        }

        static public void Flush()
        {

            // Loop through list of changed updateable classes
            while (_changed.Count > 0)
            {
                IUpdate u = _changed.Dequeue();
                if (!u.ShouldUpdate)
                {
                    // Remove class from update list fi it should no longer be updated
                    _list.Remove(u);
                }
                else if (!_list.Contains(u))
                {
                    // Add the class to the update list if it is not already listed
                    _list.Add(u);
                }
            }
            
            // Loop through list of updateable classes and run their update method
            foreach (IUpdate u in _list)
            {
                u.Update();
            }
        }
    }
}
