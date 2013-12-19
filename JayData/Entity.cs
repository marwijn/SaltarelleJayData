using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace JayDataApi
{
    public abstract class Entity
    {
        protected Entity()
        {
           InitJayData();
        }

        [InlineCode("{this}.$jayDataObject = new {this}.constructor.$jayDataConstructor();")]
        private void InitJayData()
        {
        }
    }
}
