using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Right_Click_Commands.Models.Scripts
{
    public class ScriptCollection : ObservableCollection<IScriptConfig>
    {
        //  Constructors
        //  ============

        public ScriptCollection() : base()
        {

        }

        public ScriptCollection(ICollection<IScriptConfig> scriptConfigs) : base(scriptConfigs)
        {

        }

        //  Methods
        //  =======

        public void MoveUpOne(int indexToMove)
        {
            if (indexToMove < 1)
            {
                throw new ArgumentException("The given index must be greater than 0");
            }

            if (indexToMove > Count - 1)
            {
                throw new ArgumentException("The given index must not be greater than the collections size - 1");
            }

            IScriptConfig temp = this[indexToMove];
            RemoveAt(indexToMove);
            Insert(indexToMove - 1, temp);

            string tempID = this[indexToMove].ID;
            this[indexToMove].ID = this[indexToMove - 1].ID;
            this[indexToMove - 1].ID = tempID;
        }

        public void MoveDownOne(int indexToMove)
        {
            if (indexToMove < 0)
            {
                throw new ArgumentException("The given index must be greater or equal to 0");
            }

            if (indexToMove > Count - 2)
            {
                throw new ArgumentException("The given index must not be greater than the collections size - 2");
            }

            IScriptConfig temp = this[indexToMove];
            RemoveAt(indexToMove);
            Insert(indexToMove + 1, temp);

            string tempID = this[indexToMove].ID;
            this[indexToMove].ID = this[indexToMove + 1].ID;
            this[indexToMove + 1].ID = tempID;
        }

        public void DeleteAtIndex(int indexToDelete)
        {
            if (indexToDelete < 0)
            {
                throw new ArgumentException("The given index must be greater or equal to 0");
            }

            if (indexToDelete > Count - 1)
            {
                throw new ArgumentException("The given index must not be greater than the collections size - 1");
            }

            string nextID = this[indexToDelete].ID;

            RemoveAt(indexToDelete);

            for (int i = indexToDelete; i <= Count - 1; i++)
            {
                string tempID = this[i].ID;
                this[i].ID = nextID;
                nextID = tempID;
            }
        }
    }
}
