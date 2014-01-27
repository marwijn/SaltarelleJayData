using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JayDataApi
{
    public class AsyncQueryable<T> where T: Entity, new ()
    {
        public AsyncQueryable(object jayDataObject)
        {
            JayDataObject = jayDataObject;
        }

        protected dynamic JayDataObject
        {
            [InlineCode("{this}.jayDataObject")]
            get { return null; }
            [InlineCode("{this}.jayDataObject={value}")]
            set { }
        }

        public Task<IList<T>> ToList()
        {
            var jayDataTask = Task.FromDoneCallback<IList<object>>((object) JayDataObject, "toArray");
            return jayDataTask.ContinueWith(task =>
            {
                IList<T> list = new List<T>();
                foreach (var obj in task.Result)
                {
                    list.Add(Entity.Create<T>(obj));
                }
                return list;
            });
        }

        public AsyncQueryable<T> Where(Func<T, bool> func)
        {
            var expression =  GetCode(func);
            var match = new Regex(@"\s*function\s*\(\s*(.*)\s*\)\s*{\s*(.*)\s*}.*").Exec(expression);
            expression = match[2].Replace(new Regex(@"\.jayDataObject", "g"), "");

            bool changed;
            do
            {
                var indexOfMatch = new Regex(@"indexOf\((.*?)\)[\s\S]*?!==[\s\S]*?-1", "g").Exec(expression);
                if (indexOfMatch != null)
                {
                    expression = expression.Replace(indexOfMatch[0], "contains(" + indexOfMatch[1] + ")");
                    changed = true;
                }
                else
                {
                    changed = false;
                }

            } while (changed);
            return new AsyncQueryable<T>(WhereCore(NewFunction(match[1], expression)));
        }

        [InlineCode("{func}.toString()")]
        public string GetCode(Func<T, bool> func)
        {
            return null;
        }

        [InlineCode("new Function({parameter}, {function})")]
        public object NewFunction(string parameter, string function)
        {
            return null;
        }

        [InlineCode("{this}.jayDataObject.filter({function})")]
        public object WhereCore(object function)
        {
            return null;
        }
    }
}
