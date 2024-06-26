﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject
{
    internal interface IDTOContainer
    {
        /**
         * Finds an entry of the container by its identifier and returns it.
         */
        public object Get(Type id);

        /**
         * Returns true if the container can return an entry for the given identifier.
         * Returns false otherwise.
         */
        public bool Has(Type id);
    }
}
