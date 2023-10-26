using System;
using System.Threading;

namespace Termodom.Models
{
    /// <summary>
    /// Used to buffer data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Buffer<T>
    {
        /// <summary>
        /// Delegate that returns object that is buffered
        /// </summary>
        /// <returns></returns>
        public delegate T SetBuffer();

        /// <summary>
        /// SetBuffer that returns object that is buffered
        /// </summary>
        private SetBuffer UpdateBuffer;
        private T Value { get; set; }
        /// <summary>
        /// By default it is not blocked.
        /// If you Set() MRE, and try Get() from empty buffer with "updateIfNotFound" parameter set to true,
        /// it will wait for lock and then check again if buffer has been assigned in meantime.
        /// If it hasn't been assigned even after wait it will start SetBuffer delegate.
        /// Buffer will not lock this MRE!
        /// </summary>
        private ManualResetEventSlim _dataLockMRE = new ManualResetEventSlim(true);
        /// <summary>
        /// Determinates if update loop for this buffer has been started
        /// </summary>
        private bool _updateLoopStarted { get; set; } = false;
        private TimeSpan _maximumObselence { get; set; } = TimeSpan.Zero;
        private static bool _isRunning { get; set; } = true;

        /// <summary>
        /// Store precise time at which was buffer last time updated
        /// </summary>
        public DateTime LastUpdate { get; private set; }

        /// <summary>
        /// Initialize new buffer object and stores delegate for updating buffer
        /// </summary>
        /// <param name="updateBuffer">Delegate that returns object being buffered</param>
        public Buffer(SetBuffer updateBuffer)
        {
            UpdateBuffer = updateBuffer;
        }
        /// <summary>
        /// Initialize new buffer object and stores delegate for updating buffer
        /// </summary>
        /// <param name="updateBuffer">Delegate that returns object being buffered</param>
        /// <param name="maximumObselence">Default maximum obselence</param>
        public Buffer(SetBuffer updateBuffer, TimeSpan maximumObselence)
        {
            UpdateBuffer = updateBuffer;
            _maximumObselence = maximumObselence;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~Buffer()
        {
            _isRunning = false;
        }
        /// <summary>
        /// Returns item from buffer. If buffer is outdated by maximumObselence
        /// it will update if updateIfNotFound is set to true and then return
        /// data or if it is set to false it will return default(T).
        /// If buffer need to be updated but is locked, it will first wait
        /// to get unlocked, then check to see if it now has data, and only if then
        /// there is no data it will start updating itself.
        /// </summary>
        /// <param name="updateIfNotFound">Indicates if buffer need to be updated if it has not been found at given period</param>
        /// <returns></returns>
        public T Get(bool updateIfNotFound = true)
        {
            if (_maximumObselence == TimeSpan.Zero)
                throw new Exception("Maximum obselence value is not valid!");

            if (Value != null)
                if ((LastUpdate - DateTime.Now).TotalMilliseconds < _maximumObselence.TotalMilliseconds)
                    return Value;

            if (updateIfNotFound)
            {
                _dataLockMRE.Wait();

                if (Value != null)
                    return Value;

                Value = UpdateBuffer();
                LastUpdate = DateTime.Now;
                return Value;
            }

            return default(T);
        }

        /// <summary>
        /// Returns item from buffer. If buffer is outdated by maximumObselence
        /// it will update if updateIfNotFound is set to true and then return
        /// data or if it is set to false it will return default(T).
        /// If buffer need to be updated but is locked, it will first wait
        /// to get unlocked, then check to see if it now has data, and only if then
        /// there is no data it will start updating itself.
        /// </summary>
        /// <param name="maximumObselence">Maximum obselence for buffered item</param>
        /// <param name="updateIfNotFound">Indicates if buffer need to be updated if it has not been found at given period</param>
        /// <returns></returns>
        public T Get(TimeSpan maximumObselence, bool updateIfNotFound = true)
        {
            if (Value != null)
                if ((LastUpdate - DateTime.Now).TotalMilliseconds < maximumObselence.TotalMilliseconds)
                    return Value;

            if (updateIfNotFound)
            {
                _dataLockMRE.Wait();

                if (Value != null)
                    return Value;

                Value = UpdateBuffer();
                LastUpdate = DateTime.Now;
                return Value;
            }

            return default(T);
        }
        /// <summary>
        /// Sets buffer data
        /// </summary>
        /// <param name="item"></param>
        public void Set(T item)
        {
            Value = item;
            LastUpdate = DateTime.Now;
        }
        /// <summary>
        /// Calls SetBuffer delegate to update buffered data
        /// If another has already been called but not finished,
        /// this call will be ignored since you will already get newest data
        /// By default buffer is not being locked but it can be inside SetBuffer delegate.
        /// </summary>
        public void Update()
        {
            if (_updateLoopStarted)
                return;

            if (!_dataLockMRE.IsSet)
                return;

            _updateLoopStarted = true;

            Value = UpdateBuffer();
            LastUpdate = DateTime.Now;
            GC.Collect();

            _updateLoopStarted = false;
        }
        /// <summary>
        /// Calls SetBuffer delegate asynchronously to update buffered data
        /// If another has already been called but not finished,
        /// this call will be ignored since you will already get newest data.
        /// By default buffer is not being locked but it can be inside SetBuffer delegate.
        /// </summary>
        public void UpdateAsync()
        {
            Thread t1 = new Thread(() =>
            {
                Update();
            });
            t1.IsBackground = true;
            t1.Start();
        }
        /// <summary>
        /// Sets buffer to locked state
        /// </summary>
        public void Lock()
        {
            _dataLockMRE.Wait();
            _dataLockMRE.Reset();
        }
        /// <summary>
        /// Sets buffer to unlocked state
        /// </summary>
        public void Unlock()
        {
            _dataLockMRE.Set();
        }
        /// <summary>
        /// Starts updating loop asynchronously with pause between each update.
        /// It will not run more than once update at the same time.
        /// If you try to start it multiple times it will just ignore your calls.
        /// </summary>
        public void StartUpdatingLoopAsync(TimeSpan pause)
        {
            if (_updateLoopStarted)
                return;

            _updateLoopStarted = true;

            Thread t1 = new Thread(() =>
            {
                while (_isRunning)
                {
                    Update();
                    Thread.Sleep(pause);
                }
            });
            t1.IsBackground = true;
            t1.Start();
        }
        /// <summary>
        /// Checks if buffer has been updated in given interval
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool IsRelative(TimeSpan interval)
        {
            if (Math.Abs((DateTime.Now - LastUpdate).TotalMilliseconds) < interval.TotalMilliseconds)
                return true;

            return false;
        }
    }
}
