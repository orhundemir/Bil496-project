using UnityEngine;
using System.Collections.Generic;
using System;

public class MainThreadExecutor : MonoBehaviour {

	private static readonly Queue<Action> executionQueue = new Queue<Action>();
	private static MainThreadExecutor instance = null;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	public void Update() {
		lock(executionQueue) {
			while (executionQueue.Count > 0) {
				executionQueue.Dequeue().Invoke();
			}
		}
	}

	public static bool Exists()
	{
		return instance != null;
	}

	public static MainThreadExecutor Instance()
	{
		if (!Exists())
		{
			throw new Exception("MainThreadExecutor could not find the MainThreadExecutor object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
		}
		return instance;
	}

	// Locks the queue and adds the Action to the queue
	public void Enqueue(Action action) {
		lock (executionQueue)
		{
			executionQueue.Enqueue(action);
		}
	}
}
