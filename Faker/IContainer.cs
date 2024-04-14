using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    interface IContainer
    {
        /**
         * Finds an entry of the container by its identifier and returns it.
         */
        public object get(Type id);

        /**
         * Returns true if the container can return an entry for the given identifier.
         * Returns false otherwise.
         */
        public bool has(Type id);
    }
}
