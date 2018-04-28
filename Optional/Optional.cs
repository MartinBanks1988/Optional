using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional
{
    public class Optional<T>
    {
        #region Fields

        private readonly bool _empty;
        private readonly T _item;

        #endregion

        #region Constructors

        public Optional()
        {
            _empty = true;
            _item = default(T);
        }

        public Optional(T item)
        {
            if (item == null) throw new ArgumentNullException("item cannot be null.");

            _empty = false;
            _item = item;
        }

        #endregion

        #region Methods

        public Optional<U> Map<U>(Func<T, U> f)
        {
            if (_empty) return new Optional<U>();
            else return new Optional<U>(f(_item));
        }

        public Optional<U> Bind<U>(Func<T, Optional<U>> f)
        {
            if (_empty) return new Optional<U>();
            else return f(_item);
        }

        public Optional<U> Apply<U>(Optional<Func<T, U>> f)
        {
            return f.Bind(fp => Map(fp));
        }

        public U Maybe<U>(Func<T, U> f, U u)
        {
            if (_empty) return u;
            else return f(_item);
        }

        #endregion
    }
}
