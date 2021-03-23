namespace HareDu.Snapshotting.Tests.Extensions
{
    using HareDu.Model;
    using NUnit.Framework;
    using Shouldly;
    using Snapshotting.Extensions;

    [TestFixture]
    public class ConnectionStateExtensionsTests
    {
        [Test]
        public void Verify_can_convert_1()
        {
            BrokerConnectionState state = "starting".Convert();
            
            state.ShouldBe(BrokerConnectionState.Starting);
        }
        
        [Test]
        public void Verify_can_convert_2()
        {
            BrokerConnectionState state = "tuning".Convert();
            
            state.ShouldBe(BrokerConnectionState.Tuning);
        }
        
        [Test]
        public void Verify_can_convert_3()
        {
            BrokerConnectionState state = "opening".Convert();
            
            state.ShouldBe(BrokerConnectionState.Opening);
        }
        
        [Test]
        public void Verify_can_convert_4()
        {
            BrokerConnectionState state = "flow".Convert();
            
            state.ShouldBe(BrokerConnectionState.Flow);
        }
        
        [Test]
        public void Verify_can_convert_5()
        {
            BrokerConnectionState state = "blocking".Convert();
            
            state.ShouldBe(BrokerConnectionState.Blocking);
        }
        
        [Test]
        public void Verify_can_convert_6()
        {
            BrokerConnectionState state = "blocked".Convert();
            
            state.ShouldBe(BrokerConnectionState.Blocked);
        }
        
        [Test]
        public void Verify_can_convert_7()
        {
            BrokerConnectionState state = "closing".Convert();
            
            state.ShouldBe(BrokerConnectionState.Closing);
        }
        
        [Test]
        public void Verify_can_convert_8()
        {
            BrokerConnectionState state = "closed".Convert();
            
            state.ShouldBe(BrokerConnectionState.Closed);
        }
        
        [Test]
        public void Verify_can_convert_9()
        {
            BrokerConnectionState state = "running".Convert();
            
            state.ShouldBe(BrokerConnectionState.Running);
        }
        
        [Test]
        public void Verify_can_convert_10()
        {
            BrokerConnectionState state = "".Convert();
            
            state.ShouldBe(BrokerConnectionState.Inconclusive);
        }
        
        [Test]
        public void Verify_can_convert_11()
        {
            BrokerConnectionState state = "blah".Convert();
            
            state.ShouldBe(BrokerConnectionState.Inconclusive);
        }
    }
}