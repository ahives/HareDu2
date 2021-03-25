namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using Snapshotting;

    public interface IScanner
    {
        /// <summary>
        /// Executes a list of predefined diagnostic sensors against the snapshot and returns a concatenated report of the results from executing said sensors.
        /// </summary>
        /// <param name="snapshot"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ScannerResult Scan<T>(T snapshot)
            where T : Snapshot;
        
        /// <summary>
        /// Registers a list of observers that receives the output in real-time as each sensor executes.
        /// </summary>
        /// <param name="observers"></param>
        IScanner RegisterObservers(IReadOnlyList<IObserver<ProbeContext>> observers);

        /// <summary>
        /// Registers an observer that receives the output in real-time as the sensor executes.
        /// </summary>
        /// <param name="observer"></param>
        IScanner RegisterObserver(IObserver<ProbeContext> observer);
    }
}