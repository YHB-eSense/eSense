using EarableLibrary;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTesting.Mocks
{
	public class MockBLEConnection : IDeviceConnection
	{
		public ConnectionState State { get; set; }

		public Guid Id { get; set; }

		public int OperationDelay { get; set; }

		public bool FailOperations { get; set; }

		public Dictionary<Guid, DataReceiver> Subscriptions { get; }

		public Dictionary<Guid, byte[]> Storage { get; }

		public MockBLEConnection()
		{
			Subscriptions = new Dictionary<Guid, DataReceiver>();
			Storage = new Dictionary<Guid, byte[]>();
			State = ConnectionState.Disconnected;
			OperationDelay = 100;
			FailOperations = false;
		}

		public async Task<bool> Close()
		{
			if (FailOperations) return false;
			if (State != ConnectionState.Connected) return false;
			State = ConnectionState.Disconnecting;
			await Task.Delay(OperationDelay);
			State = ConnectionState.Disconnected;
			return true;
		}

		public async Task<bool> Open()
		{
			if (FailOperations) return false;
			if (State != ConnectionState.Disconnected) return false;
			State = ConnectionState.Connecting;
			await Task.Delay(OperationDelay);
			State = ConnectionState.Connected;
			return true;
		}

		public async Task<byte[]> ReadAsync(Guid charId)
		{
			if (FailOperations) return null;
			await Task.Delay(OperationDelay);
			if (!Storage.ContainsKey(charId)) return null;
			return Storage[charId];
		}

		public async Task<bool> WriteAsync(Guid charId, byte[] val)
		{
			if (FailOperations) return false;
			await Task.Delay(OperationDelay);
			Storage[charId] = val;
			return true;
		}

		public async Task<bool> SubscribeAsync(Guid charId, DataReceiver handler)
		{
			if (FailOperations) return false;
			await Task.Delay(OperationDelay);
			Subscriptions[charId] = handler;
			return true;
		}

		public async Task<bool> UnsubscribeAsync(Guid charId, DataReceiver handler)
		{
			if (FailOperations) return false;
			await Task.Delay(OperationDelay);
			Subscriptions.Remove(charId);
			return true;
		}

		public async Task<bool> Validate(Guid[] serviceIds)
		{
			await Task.Delay(OperationDelay);
			return !FailOperations;
		}

		public void InvokeSubscription(Guid charId, byte[] val)
		{
			if (!Subscriptions.ContainsKey(charId)) return;
			Subscriptions[charId](val);
		}
	}
}
