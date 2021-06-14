# csharp-async-lab
> Async programming is fun and easy...

Not.

## Multithreading: parallel vs concurrent vs async

|Model|Info|Diagram|
|---|---|---|
|Parallel|Threads shouldn't communicate, just do their own job on separate CPUs cores.|![image](https://user-images.githubusercontent.com/3278804/121891024-46c41a00-cd1b-11eb-82e5-fe4a099315c2.png)|
|Concurrent|Threads have to fight for CPU, do communicate and wait for each other.|![image](https://user-images.githubusercontent.com/3278804/121891217-796e1280-cd1b-11eb-9dec-60d2118ac783.png)|
|Asynchronous|No CPU is involved through most of the time, just waiting for a signal that I/O operation finished|![image](https://user-images.githubusercontent.com/3278804/121891582-eda8b600-cd1b-11eb-86e6-f9cef4b5ff52.png)|

## The concept of Tasks
- Implement what is known as the *Promise Model of Concurrency*, they offer you a "promise" that work will be completed at a later point in time.
- `Task` != `Thread`, it is more than just an abstraction on top of a `Thread`
- Language integration, with the `await` keyword, provides a higher-level abstraction for using tasks

## TPL vs async Tasks
- Asynchronous tasks are not the same as parallel tasks (TPL)
  - They are the same type `Task`, but their purpose is completely different and thus their proper usage is also completely different
- Task splits into two types:
  - CPU-bound **delegate** task: TPL world, has code to run, can be scheduled and executed - `Task.Run`, `Parallel.For`, `PLINQ`
  - I/O-bound **promise** task: async world, signals completion of sth –  **real async code**, `Task.FromResult()`, `Task.Delay()`, `Task.Yield()`
- `await task != task.Wait()`
  - `await task` yields control to its caller and then jumps back as soon as the operation is completed (continuations and coroutines)
  - `task.Wait()`/`task.Result` is **blocking** because it has to wait for completion
- Common mistake is to try to make asynchronous wrappers around existing synchronous methods, aka **fake-asynchronous methods**
  (code: [AvoidFakeAsync](src/ToAsyncOrNotToAsync/AvoidFakeAsync.cs))
  
## Except Task remember those concepts
- `Thread` – is a limited resource, memory footprint aroun 1MB, managed by...
- `ThreadPool` – one (global) per process, local pool per core, distinguish `Tasks`,  managed by...
- `TaskScheduler` - handles the scheduling and execution of `Tasks`, uses...
- `SynchronizationContext` – framework dependent, global variable, `Current` is per `Thread`, `Current==null` defaults to `ThreadPool`

|`TaskScheduler` and `SynchronizationContext`|`SynchronizationContext` and `async`|
|--|--|
| ![image](https://user-images.githubusercontent.com/3278804/121893137-e97d9800-cd1d-11eb-9425-12bdf749e3bf.png) | ![image](https://user-images.githubusercontent.com/3278804/121893340-2184db00-cd1e-11eb-8e13-f05082daef91.png) |

## Asynchronous execution - facts
- `async` keyword means nothing, it only instructs the compiler to create a state machine (code: ToAsyncOrNotToAsync + ILSpy)
- It is all about commitment – we get better response (= scalability) in exchange for bigger memory usage and generated code (code: [AsyncRequestInAsp](src/AsyncRequestInAsp))
- `await`able type must be able to expose `GetAwaiter()` (code: [AvaitablePrimitive](src/ToAsyncOrNotToAsync/AvaitablePrimitive.cs))
- `async` operations are best if performed by the hardware!



