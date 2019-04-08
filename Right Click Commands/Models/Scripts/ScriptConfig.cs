using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Scripts
{
    public abstract class ScriptConfig : ViewModelBase, IScriptConfig
    {
        //  Properties
        //  ==========

        public abstract string ID { get; set; }
        public abstract string Name { get; }
        public abstract string ScriptLocation { get; protected set; }
        public abstract string Label { get; set; }
        public abstract string Script { get; set; }
        public abstract bool OnDirectory { get; set; }
        public abstract bool OnBackground { get; set; }
        public abstract bool KeepWindowOpen { get; set; }

        //  Methods
        //  =======

        public abstract bool IsForLocation(MenuLocation location);
        public abstract void LoadScript();
        public abstract void ModifyLocation(MenuLocation location, bool enabled);
        public abstract void SaveScript();
    }

    public static class ScriptConfigUtils
    {
        public static ObservableCollection<ScriptConfig> MoveUpOne(this ObservableCollection<ScriptConfig> collection, int indexToMove)
        {
            if (indexToMove < 1)
            {
                throw new ArgumentException("The givne index must be greater than zero");
            }

            if (indexToMove > collection.Count - 1)
            {
                throw new ArgumentException("The given index must not be greater than the collections size - 1");
            }

            ScriptConfig temp = collection[indexToMove];
            collection.RemoveAt(indexToMove);
            collection.Insert(indexToMove - 1, temp);

            string tempID = collection[indexToMove].ID;
            collection[indexToMove].ID = collection[indexToMove - 1].ID;
            collection[indexToMove - 1].ID = tempID;

            return collection;
        }

        public static ObservableCollection<ScriptConfig> MoveDownOne(this ObservableCollection<ScriptConfig> collection, int indexToMove)
        {
            if (indexToMove < 0)
            {
                throw new ArgumentException("The given index must be greater of equal to zero");
            }

            if (indexToMove > collection.Count - 2)
            {
                throw new ArgumentException("The give index must not be greater than the collections size - 2");
            }

            ScriptConfig temp = collection[indexToMove];
            collection.RemoveAt(indexToMove);
            collection.Insert(indexToMove + 1, temp);

            string tempID = collection[indexToMove].ID;
            collection[indexToMove].ID = collection[indexToMove + 1].ID;
            collection[indexToMove + 1].ID = tempID;

            return collection;
        }

        public static ObservableCollection<ScriptConfig> DeleteAtIndex(this ObservableCollection<ScriptConfig> collection, int indexToDelete)
        {
            if (indexToDelete < 0)
            {
                throw new ArgumentException("The give index must be greater or equal to 0");
            }

            if (indexToDelete > collection.Count - 1)
            {
                throw new ArgumentException("The given index must not be greater than the collections size - 1");
            }

            string nextID = collection[indexToDelete].ID;

            collection.RemoveAt(indexToDelete);

            for (int i = indexToDelete; i <= collection.Count - 1; i++)
            {
                string tempID = collection[i].ID;
                collection[i].ID = nextID;
                nextID = tempID;
            }

            return collection;
        }
    }
}
