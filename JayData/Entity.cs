using System;
using System.Runtime.CompilerServices;

namespace JayDataApi
{
    public abstract class Entity
    {
        protected Entity()
        {
           InitJayData();
        }

        [InlineCode("{this}.jayDataObject = new {this}.constructor.jayDataConstructor();")]
        private void InitJayData()
        {
        }

        [InlineCode("{this}.jayDataObject = {jayDataObject}")]
        private void InitJayData(dynamic jayDataObject)
        {
        }

        internal static T Create<T>(dynamic jayDataObject) where T :Entity, new()
        {
            var entity = new T();
            entity.InitJayData(jayDataObject);
            return entity;
        }

        public override string ToString()
        {
            return CoreToString();
        }

        [InlineCode("JSON.stringify({this}.jayDataObject.toJSON())")]
        private string CoreToString()
        {
            return null;
        }
    }
}
