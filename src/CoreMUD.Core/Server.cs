using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace CoreMUD.Core
{
    public class Server
    {
        private Stopwatch _gameTimer;
        private bool _initialized = false;
        private bool _isRunning;
        private TimeSpan _accumulatedElapsedtime;
        private long _previousTicks;
        private readonly GameTime _gameTime = new GameTime();
        private int _updateFrameLag;
        private TimeSpan _targetElapsedTime = TimeSpan.FromTicks(83333); // 30pfs
        private TimeSpan _inactiveSleepTime = TimeSpan.FromSeconds(0.02);
        private TimeSpan _maxElapsedTime = TimeSpan.FromMilliseconds(500);

        private static Server _instance;

        private readonly ObservableCollection<ISystem> _systems;

        public Server()
        {
            _systems = new ObservableCollection<ISystem>();
            _systems.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        OnSystemsAdded(e.NewItems);
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        OnSystemsRemoved(e.OldItems);
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        OnSystemsRemoved(e.OldItems);
                        OnSystemsAdded(e.NewItems);
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        OnSystemsReset();
                        break;
                    default:
                        break;
                }
            };

            if (_instance != null)
            {
                throw new InvalidOperationException(
                    $"Only one instance of {typeof(Server).FullName} can exist at a time");
            }

            IsFixedTimeStep = true;

            _instance = this;
        }

        private void OnSystemsReset()
        {
            //foreach (var system in _turnBasedSystems)
            //{
            //    system.ResetTurns();
            //}
            //_turnBasedSystems.Clear();
        }

        private void OnSystemsRemoved(IList oldItems)
        {
            foreach (var systemToRemove in oldItems.OfType<ITurnBased>())
            {
                systemToRemove.ResetTurns();
                //_turnBasedSystems.Remove(systemToRemove);
            }
        }

        private void OnSystemsAdded(IList newItems)
        {
            foreach (var systemToAdd in newItems.OfType<ITurnBased>())
            {
                systemToAdd.ResetTurns();
                //_turnBasedSystems.Add(systemToAdd);
            }
        }

        internal static Server Instance { get { return _instance; } }

        public bool IsFixedTimeStep { get; set; }
        public TimeSpan TargetElapsedTime { get; set; }
        public TimeSpan InactiveSleeptime
        {
            get
            {
                return _inactiveSleepTime;
            }
            set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        $"The value of {nameof(InactiveSleeptime)} must be positive");
                }

                _inactiveSleepTime = value;
            }
        }

        public TimeSpan MaxElapsedTime
        {
            get
            {
                return _maxElapsedTime;
            }
            set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        $"The value of {nameof(MaxElapsedTime)} must be positive");
                }

                _maxElapsedTime = value;
            }
        }

        public void Run()
        {
            if (!_initialized)
            {
                DoInitialize();
                _gameTimer = Stopwatch.StartNew();
            }

            _isRunning = BeginRun();

            while (_isRunning)
            {
                Tick();
            }

            EndRun();
        }

        private void DoInitialize()
        {
            BeginInitialize();

            Initialize();

            EndInitialize();
        }

        private void BeginInitialize()
        {

        }

        protected virtual void Initialize() { }

        private void EndInitialize()
        {
            //foreach (var system in _turnBasedSystems)
            //{
            //    system.ResetTurns();
            //}
        }

        protected virtual bool BeginRun()
        {
            return true;
        }

        protected virtual void EndRun()
        {
           
        }

        private void Tick()
        {
            RetryTick:

            // Advance the accumulated elapsed time.
            var currentTicks = _gameTimer.Elapsed.Ticks;
            _accumulatedElapsedtime += TimeSpan.FromTicks(currentTicks - _previousTicks);
            _previousTicks = currentTicks;

            // If we're in the fixed timestep mode and not enough time has elapsed
            // to perform an update, we sleep off the remaining time to save battery
            // life or release CPU time to other threads and processes.
            if (IsFixedTimeStep && _accumulatedElapsedtime < TargetElapsedTime)
            {
                var sleeptime = (int)(TargetElapsedTime - _accumulatedElapsedtime).TotalMilliseconds;
#if WINRT
                Task.Delay(sleepTime).Wait();
#else
                System.Threading.Thread.Sleep(sleeptime);
#endif
                goto RetryTick;
            }

            // Do not allow any update to take longer than our maximum.
            if (_accumulatedElapsedtime > _maxElapsedTime)
                _accumulatedElapsedtime = _maxElapsedTime;

            if (IsFixedTimeStep)
            {
                _gameTime.ElapsedGameTime = TargetElapsedTime;
                var stepCount = 0;

                // Perform as many full fixed length time steps as we can.
                while (_accumulatedElapsedtime >= TargetElapsedTime)
                {
                    _gameTime.TotalGameTime += TargetElapsedTime;
                    _accumulatedElapsedtime -= TargetElapsedTime;
                    ++stepCount;

                    DoUpdate(_gameTime);
                }

                // Every update after the first accumulates lag
                _updateFrameLag += Math.Max(0, stepCount - 1);

                // If we think we are running slowly, wait until the lag clears before resetting it
                if (_gameTime.IsRunningSlowly)
                {
                    if (_updateFrameLag == 0)
                        _gameTime.IsRunningSlowly = false;
                }
                else
                {
                    if (_updateFrameLag >= 5)
                    {
                        // If we lag more than 5 frames, start thinking we're running slowly
                        _gameTime.IsRunningSlowly = true;
                    }
                }

                // Every time we just do one update, we are not running slowly, so decrease the lag
                if (stepCount == 1 && _updateFrameLag > 0)
                {
                    _updateFrameLag--;
                }

                _gameTime.ElapsedGameTime = TimeSpan.FromTicks(TargetElapsedTime.Ticks * stepCount);
            }
            else
            {
                // Perform a single variable length update.
                _gameTime.ElapsedGameTime = _accumulatedElapsedtime;
                _gameTime.TotalGameTime += _accumulatedElapsedtime;
                _accumulatedElapsedtime = TimeSpan.Zero;

                DoUpdate(_gameTime);
            }
        }

        private void DoUpdate(GameTime gameTime)
        {
            if (BeforeUpdate(gameTime))
            {
                Update(gameTime);
                AfterUpdate();
            }

        }

        protected virtual bool BeforeUpdate(GameTime gameTime)
        {
            return true;
        }

        private void Update(GameTime gameTime)
        {
            //if (_turnBasedSystems.All(s => !s.IsInTurn))
            //{
            //    foreach (var system in _turnBasedSystems)
            //    {
            //        system.StartTurn();
            //    }
            //}

            foreach (var system in _systems)
            {
                system.Update(gameTime);
            }

            //// Let the turn-based systems end their own turns?
            //if (_turnBasedSystems.All(s => !s.IsInTurn))
            //{
            //    foreach (var system in _turnBasedSystems)
            //    {
            //        system.EndTurn();
            //    }
            //}
        }

        protected virtual void AfterUpdate()
        {

        }
    }
}
