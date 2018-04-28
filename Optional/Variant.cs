using System;

namespace Optional
{
    public class Variant<T, U>
    {
        #region Fields

        internal readonly object _item;

        #endregion

        #region Private Constructor

        private Variant(object item)
        {
            _item = item;
        }

        #endregion

        #region Implicit Conversions

        public static implicit operator Variant<T, U>(T t)
        {
            return new Variant<T, U>(t);
        }

        public static implicit operator Variant<T, U>(U u)
        {
            return new Variant<T, U>(u);
        }

        public static implicit operator Variant<U, T>(Variant<T, U> that)
        {
            return new Variant<U, T>(that._item);
        }

        #endregion

        #region Methods

        public Variant<Tp, Up> Map<Tp, Up>(Func<T, Tp> f, Func<U, Up> g)
        {
            switch (_item)
            {
                case T t: return f(t);
                case U u: return g(u);
                default: return null; // Impossible.
            }
        }

        public Variant<Tp, U> Map<Tp>(Func<T, Tp> f)
        {
            return Map(f, x => x);
        }

        public Variant<T, Up> Map<Up>(Func<U, Up> g)
        {
            return Map(x => x, g);
        }

        public A FlatMap<A>(Func<T, A> f, Func<U, A> g)
        {
            switch (_item)
            {
                case T t: return f(t);
                case U u: return g(u);
                default: return default(A); // Impossible.
            }
        }

        public U FlatMap(Func<T, U> f)
        {
            return FlatMap(f, x => x);
        }

        public T FlatMap(Func<U, T> g)
        {
            return FlatMap(x => x, g);
        }

        #endregion
    }

    public class Variant<T, U, V>
    {
        #region Fields

        internal readonly object _item;

        #endregion

        public Variant(T t)
        {
            _item = t;
        }

        public Variant(U u)
        {
            _item = u;
        }

        public Variant(V v)
        {
            _item = v;
        }

        public static implicit operator Variant<T, U, V>(Variant<T, U> that)
        {
            switch (that._item)
            {
                case T t: return new Variant<T, U, V>(t);
                case U u: return new Variant<T, U, V>(u);
            }

            return null; // Impossible.
        }

        //public static implicit operator Variant<T, U, V>(Variant<T, V> that)
        //{
        //    return that.FlatMap<T, U, V>(t => new Variant<T, U, V>(t), v => new Variant<T, U, V>(v));
        //}

        //public static implicit operator Variant<T, U, V>(Variant<U, V> that)
        //{
        //    return that.FlatMap<T, U, V>(u => new Variant<T, U, V>(u), v => new Variant<T, U, V>(v));
        //}
    }
}
