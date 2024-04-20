using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace tdl界面
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")//属性名称作为参数传输进去
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// 异步 observable collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// The _synchronization context
        /// </summary>
        private readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncObservableCollection{T}"/> class.
        /// </summary>
        public AsyncObservableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncObservableCollection{T}"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public AsyncObservableCollection(IEnumerable<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// Inserts the item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void InsertItem(int index, T item)
        {
            this.ExecuteOnSyncContext(() => base.InsertItem(index, item));
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="index">The index.</param>
        protected override void RemoveItem(int index)
        {
            this.ExecuteOnSyncContext(() => base.RemoveItem(index));
        }

        /// <summary>
        /// Sets the item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void SetItem(int index, T item)
        {
            this.ExecuteOnSyncContext(() => base.SetItem(index, item));
        }

        /// <summary>
        /// Moves the item.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            this.ExecuteOnSyncContext(() => base.MoveItem(oldIndex, newIndex));
        }

        /// <summary>
        /// Clears the items.
        /// </summary>
        protected override void ClearItems()
        {
            this.ExecuteOnSyncContext(() => base.ClearItems());
        }

        /// <summary>
        /// Executes the on synchronize context.
        /// </summary>
        /// <param name="action">The action.</param>
        private void ExecuteOnSyncContext(Action action)
        {
            if (SynchronizationContext.Current == this.synchronizationContext)
            {
                action();
            }
            else
            {
                this.synchronizationContext.Send(_ => action(), null);
            }
        }
    }
}
