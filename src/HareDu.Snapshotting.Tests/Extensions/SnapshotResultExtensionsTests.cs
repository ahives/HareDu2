// Copyright 2013-2020 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Snapshotting.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fakes;
    using MassTransit;
    using Model;
    using Moq;
    using Moq.Protected;
    using NUnit.Framework;
    using Persistence;
    using Shouldly;
    using Snapshotting.Extensions;

    [TestFixture]
    public class SnapshotResultExtensionsTests
    {
        [Test]
        public void Verify_can_retrieve_most_recent_snapshot_1()
        {
            string identifier1 = NewId.Next().ToString();
            string identifier2 = NewId.Next().ToString();
            string identifier3 = NewId.Next().ToString();
            IReadOnlyList<SnapshotResult<ClusterSnapshot>> results = GetResults(identifier1, identifier2, identifier3).ToList();
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(results);

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
            snapshot.Identifier.ShouldBe(identifier3);
        }

        [Test]
        public void Verify_can_retrieve_most_recent_snapshot_2()
        {
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(null);

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_retrieve_most_recent_snapshot_3()
        {
            SnapshotTimeline<ClusterSnapshot> timeline = null;

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
        }

        [Test]
        public void Verify_can_save_when_directory_exists()
        {
            string identifier1 = NewId.Next().ToString();
            string identifier2 = NewId.Next().ToString();
            string identifier3 = NewId.Next().ToString();
            IReadOnlyList<SnapshotResult<ClusterSnapshot>> results = GetResults(identifier1, identifier2, identifier3).ToList();
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(results);

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
            snapshot.Identifier.ShouldBe(identifier3);

            var mock = new Mock<SnapshotWriter>();

            mock.Protected()
                .As<ISnapshotWriterMock>()
                .Setup(x => x.Write<ClusterSnapshot>(
                    It.IsAny<SnapshotResult<ClusterSnapshot>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            mock.Protected()
                .As<ISnapshotWriterMock>()
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            
            string path = $"{Directory.GetCurrentDirectory()}/snapshots";
            string file = "test.json";
            var writer = mock.Object;
            
            snapshot.Save(writer, file, path).ShouldBeTrue();
            mock.VerifyAll();
            mock.VerifyNoOtherCalls();
        }

        [Test]
        public void Verify_can_save_when_directory_does_not_exists()
        {
            string identifier1 = NewId.Next().ToString();
            string identifier2 = NewId.Next().ToString();
            string identifier3 = NewId.Next().ToString();
            IReadOnlyList<SnapshotResult<ClusterSnapshot>> results = GetResults(identifier1, identifier2, identifier3).ToList();
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(results);

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
            snapshot.Identifier.ShouldBe(identifier3);

            var mock = new Mock<SnapshotWriter>();

            mock.Protected()
                .As<ISnapshotWriterMock>()
                .Setup(x => x.Write<ClusterSnapshot>(
                    It.IsAny<SnapshotResult<ClusterSnapshot>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(true)
                .Verifiable();

            mock.Protected()
                .As<ISnapshotWriterMock>()
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            mock.Protected()
                .As<ISnapshotWriterMock>()
                .Setup(x => x.CreateDirectory(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            
            string path = $"{Directory.GetCurrentDirectory()}/snapshots";
            string file = "test.json";
            var writer = mock.Object;
            
            snapshot.Save(writer, file, path).ShouldBeTrue();
            mock.VerifyAll();
            mock.VerifyNoOtherCalls();
        }

        [Test]
        public void Verify_cannot_save_when_directory_does_not_exists_or_cannot_be_created()
        {
            string identifier1 = NewId.Next().ToString();
            string identifier2 = NewId.Next().ToString();
            string identifier3 = NewId.Next().ToString();
            IReadOnlyList<SnapshotResult<ClusterSnapshot>> results = GetResults(identifier1, identifier2, identifier3).ToList();
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(results);

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
            snapshot.Identifier.ShouldBe(identifier3);

            var mock = new Mock<SnapshotWriter>();

            // mock.Protected()
            //     .As<ISnapshotWriterMock>()
            //     .Setup(x => x.Write<ClusterSnapshot>(
            //         It.IsAny<SnapshotResult<ClusterSnapshot>>(),
            //         It.IsAny<string>(),
            //         It.IsAny<string>()))
            //     .Returns(false)
            //     .Verifiable();

            mock.Protected()
                .As<ISnapshotWriterMock>()
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            mock.Protected()
                .As<ISnapshotWriterMock>()
                .Setup(x => x.CreateDirectory(It.IsAny<string>()))
                .Returns(false)
                .Verifiable();
            
            string path = $"{Directory.GetCurrentDirectory()}/snapshots";
            string file = "test.json";
            var writer = mock.Object;
            
            snapshot.Save(writer, file, path).ShouldBeFalse();
            mock.VerifyAll();
            mock.VerifyNoOtherCalls();
        }

        [Test]
        public void Verify_cannot_write()
        {
            string identifier1 = NewId.Next().ToString();
            string identifier2 = NewId.Next().ToString();
            string identifier3 = NewId.Next().ToString();
            IReadOnlyList<SnapshotResult<ClusterSnapshot>> results = GetResults(identifier1, identifier2, identifier3).ToList();
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(results);

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
            snapshot.Identifier.ShouldBe(identifier3);

            var mock = new Mock<SnapshotWriter>();

            mock.Protected()
                .As<ISnapshotWriterMock>()
                .Setup(x => x.Write<ClusterSnapshot>(
                    It.IsAny<SnapshotResult<ClusterSnapshot>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(false)
                .Verifiable();

            mock.Protected()
                .As<ISnapshotWriterMock>()
                .Setup(x => x.DirectoryExists(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            
            string path = $"{Directory.GetCurrentDirectory()}/snapshots";
            string file = "test.json";
            var writer = mock.Object;
            
            snapshot.Save(writer, file, path).ShouldBeFalse();
            mock.VerifyAll();
            mock.VerifyNoOtherCalls();
        }

        [Test]
        public void Verify_cannot_save_null_param_1()
        {
            string identifier1 = NewId.Next().ToString();
            string identifier2 = NewId.Next().ToString();
            string identifier3 = NewId.Next().ToString();
            IReadOnlyList<SnapshotResult<ClusterSnapshot>> results = GetResults(identifier1, identifier2, identifier3).ToList();
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(results);

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
            snapshot.Identifier.ShouldBe(identifier3);
            
            string path = $"{Directory.GetCurrentDirectory()}/snapshots";
            string file = "test.json";

            Should.Throw<ArgumentNullException>(() => { snapshot.Save(null, file, path).ShouldBeFalse(); });
        }

        [Test]
        public void Verify_cannot_save_null_param_2()
        {
            string identifier1 = NewId.Next().ToString();
            string identifier2 = NewId.Next().ToString();
            string identifier3 = NewId.Next().ToString();
            IReadOnlyList<SnapshotResult<ClusterSnapshot>> results = GetResults(identifier1, identifier2, identifier3).ToList();
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(results);

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
            snapshot.Identifier.ShouldBe(identifier3);
            
            string path = $"{Directory.GetCurrentDirectory()}/snapshots";

            Should.Throw<ArgumentNullException>(() => { snapshot.Save(new SnapshotWriter(), null, path).ShouldBeFalse(); });
        }

        [Test]
        public void Verify_cannot_save_null_param_3()
        {
            string identifier1 = NewId.Next().ToString();
            string identifier2 = NewId.Next().ToString();
            string identifier3 = NewId.Next().ToString();
            IReadOnlyList<SnapshotResult<ClusterSnapshot>> results = GetResults(identifier1, identifier2, identifier3).ToList();
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(results);

            var snapshot = timeline.MostRecent();
            
            snapshot.ShouldNotBeNull();
            snapshot.Identifier.ShouldBe(identifier3);
            
            string path = $"{Directory.GetCurrentDirectory()}/snapshots";
            string file = "test.json";

            Should.Throw<ArgumentNullException>(() => { snapshot.Save(null, file, null).ShouldBeFalse(); });
        }

        [Test]
        public void Verify_cannot_save_null_param_4()
        {
            string identifier1 = NewId.Next().ToString();
            string identifier2 = NewId.Next().ToString();
            string identifier3 = NewId.Next().ToString();
            IReadOnlyList<SnapshotResult<ClusterSnapshot>> results = GetResults(identifier1, identifier2, identifier3).ToList();
            SnapshotTimeline<ClusterSnapshot> timeline = new SnapshotTimelineImpl<ClusterSnapshot>(results);

            SnapshotResult<ClusterSnapshot> snapshot = null;
            
            string path = $"{Directory.GetCurrentDirectory()}/snapshots";
            string file = "test.json";

            Should.Throw<ArgumentNullException>(() => { snapshot.Save(new SnapshotWriter(), file, path).ShouldBeFalse(); });
        }

        IEnumerable<SnapshotResult<ClusterSnapshot>> GetResults(string identifier1, string identifier2, string identifier3)
        {
            yield return new FakeSnapshotResult<ClusterSnapshot>(new FakeClusterSnapshot1(), identifier1);
            yield return new FakeSnapshotResult<ClusterSnapshot>(new FakeClusterSnapshot1(), identifier2);
            yield return new FakeSnapshotResult<ClusterSnapshot>(new FakeClusterSnapshot1(), identifier3);
        }

        
        interface ISnapshotWriterMock
        {
            bool Write<T>(SnapshotResult<T> result, string file, string path)
                where T : Snapshot;

            bool CreateDirectory(string path);
            
            bool DirectoryExists(string path);
        }

        
        class SnapshotTimelineImpl<T> :
            SnapshotTimeline<T>
            where T : Snapshot
        {
            public SnapshotTimelineImpl(IReadOnlyList<SnapshotResult<T>> results)
            {
                Results = results;
            }

            public IReadOnlyList<SnapshotResult<T>> Results { get; }
            public void PurgeAll()
            {
                throw new System.NotImplementedException();
            }

            public void Purge<U>(SnapshotResult<U> result) where U : Snapshot
            {
                throw new System.NotImplementedException();
            }
        }
    }
}