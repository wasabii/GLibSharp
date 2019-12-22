namespace GObject.Introspection
{

    /// <summary>
    /// Represents a generic heap-allocated value. Used to represent managed pointers to value type records.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Ref<T>
        where T : struct
    {

        /// <summary>
        /// Converts the given <see cref="Ref{T}"/> to a <see cref="T"/>.
        /// </summary>
        /// <param name="r"></param>
        public static implicit operator T(Ref<T> r) => r.value;

        /// <summary>
        /// Converts the given <see cref="T"/> to a <see cref="Ref{T}"/>.
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator Ref<T>(T v) => new Ref<T>(v);

        internal T value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public Ref(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public T Value
        {
            get => this.value;
            set => this.value = value;
        }

    }

}
