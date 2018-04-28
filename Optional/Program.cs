using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optional
{
    class Program
    {
        static void Main(string[] args)
        {
            //var v = new Variant<int, float>(1);
            //v = v.Map(i => i + 1, VariantHelper.Identity);
            Variant<int, string> v = "3";
            Variant<string, int> v2 = v;
            //Console.WriteLine(v2.FlatMap(t => t.ToString(), u => u.ToString()));
            Console.WriteLine(v2.Map(t => t * 1).FlatMap(t => int.Parse(t)));
            Console.Read();
        }
    }
    
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

    public static class VariantHelper
    {
        public static X Identity<X>(X x) { return x; }
    }

    public class Variant<T, U>
    {
        #region Fields

        internal readonly object _item;

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

        private Variant(object item)
        {
            _item = item;
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
            return Map(f, VariantHelper.Identity);
        }

        public Variant<T, Up> Map<Up>(Func<U, Up> g)
        {
            return Map(VariantHelper.Identity, g);
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
            return FlatMap(f, VariantHelper.Identity);
        }

        public T FlatMap(Func<U, T> g)
        {
            return FlatMap(VariantHelper.Identity, g);
        }

        public Variant<A, B> FlatMap<A, B>(Func<T, Variant<A, B>> f, Func<U, Variant<A, B>> g)
        {
            switch (_item)
            {
                case T t: return f(t);
                case U u: return g(u);
                default: return null; // Impossible.
            }
        }

        #endregion
    }

    public class Variant<T, U, V>
    {
        internal readonly object _item;

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
