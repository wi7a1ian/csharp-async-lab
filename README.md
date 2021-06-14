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

| Asynchronous execution | Flow |
|--|--|
|![image](https://user-images.githubusercontent.com/3278804/121895554-a83ab780-cd20-11eb-8623-1066deb5bb0f.png)|![image](https://user-images.githubusercontent.com/3278804/121895571-abce3e80-cd20-11eb-9fa9-92e933202a88.png)|

## Exceptions in asynchronous execution
- Exceptions from `async void` are propagated immediately as unhandled even in a `try`/`catch`, **usually** killing the app
  - Code: [ExceptionsInAsync](src/ExceptionsInAsync)
  - Code: [AsyncEventsInWpf > `AsyncException_Click`](src/AsyncEventsInWpf/MainWindow.xaml.cs#L71)
- Exceptions from `async Task` are stored in the context of a `Task` and propagated when `await`ed/`Wait()` 
  - or when cleaned up by the `GC` if nobody awaits it (surpressed since NET 4.5)
  - Code: [ExceptionsInAsync](src/ExceptionsInAsync)

## Top Issues with Asynchronous Execution
- Deadlocks – when blocking (`.Result`, `.Wait()`) and `async` code in some `SynchronizationContexts`
  - Code: [DeadlocksInWpf](src/DeadlocksInWpf/MainWindow.xaml.cs)
  - Code: [AsyncRequestInAsp](src/AsyncRequestInAsp/Controllers/FunkyController.cs)
  - Code: [AsyncRequestInAspCore](src/AsyncRequestInAspCore/Controllers/FunkyController.cs)
- Access violation - accessing UI elements has to happen from UI execution context
  - Code: [DeadlocksInWpf](src/DeadlocksInWpf/MainWindow.xaml.cs)
- `AggregateException` – when blocking on `async` (except `GetAwaiter().GetResult()`)
  - Code: [AsyncTaskIsBetterWithWait](src/ExceptionsInAsync/AsyncTaskIsBetterWithWait.cs)
- Exceptions – unhandled may terminate the app soon or when closing
- Blocking UI thread = blocked event pipe - slow synchronous or blocking code inside `async` call chain will make your app less responsive
- Performance - `async` is an overhead, consider running synchronously (don’t confuse performance with scalability!)
  - Code: [AsyncAllTheWay](src/ToAsyncOrNotToAsync/AsyncAllTheWay.cs) + ILSpy

## Guidelines for working with `async`

| what  | instead of | use |
|:--|---|---|
| Retrieve the result of a background task | `Task.Wait()` or `Task.Result` | `await` |
| Wait for any task to complete | `Task.WaitAny()` | `await Task.WhenAny()` |
| Retrieve the results of multiple tasks | `Task.WaitAll()` | `await Task.WhenAll()` |
| Wait a period of time | `Thread.Sleep()` | `await Task.Delay` |
| I/O bound operation | synchronous or parallel | `async` |
| CPU boud operation | async | `Task.Run()` |

- ***Async all the way*** - don’t mix blocking (`Thread.Sleep()`, `.Result`, `.Wait()`) and `async` code without carefully considering the consequences
  - Add `async` only where it is needed (usually at one of the ends of the call chain) and **let it naturally grow**
  - Code: [AsyncAllTheWay](src/ToAsyncOrNotToAsync/AsyncAllTheWay.cs)
- Avoid *async over sync* or *sync over async*...
  - Mixing can cause deadlocks, more-complex error handling and unexpected blocking of context threads
  - Avoid `Task.Run` inside `async` methods
  - Code: [AvoidFakeAsync](src/ToAsyncOrNotToAsync/AvoidFakeAsync.cs)
- Avoid `async void`, prefer `async Task` method
  - Exception: event handlers
- Use `ConfigureAwait(false)` when blocking is unavoidable or **when designing library**
  - Exception: methods that require context like UI elements, ASP request context...
- Avoid `return await` - it generates overhead in form of async state machine (100b), where returning a `Task` would be enough
  - Exception: in `try`/`catch` blocks, in `using(...)` blocks
  - Code: [AvoidReturnAwait](src/ToAsyncOrNotToAsync/AvoidReturnAwait.cs)
- Always `await Tasks` and handle all the exceptions
- Always add handlers to *unobserved exceptions* and *unhandled exceptions*
  - Code: [AsyncEventsInWpf](src/AsyncEventsInWpf/MainWindow.xaml.cs)
- Use a helper when doing fire-and-forget (i.e: in a ctor) to hande exceptions
  - Code: [AsyncTaskIsBetterWithHelper](src/ExceptionsInAsync/AsyncTaskIsBetterWithHelper.cs)
- Remember to pass `CancellationToken` like you would to in case of parallel Tasks
  - Code: [AsyncRequestInAspCore](src/AsyncRequestInAsp/Controllers/FunkyController.cs)
- Be carefull for implicit `async void`
  - Code: [AvoidImplicitAsyncVoid](src/ToAsyncOrNotToAsync/AvoidImplicitAsyncVoid.cs)
- Use `SemaphoreSlim` for locking resources
- Use `FlushAsync` for `Stream`/`StreamWriter`
  - Code: [DoFlushAsyncOnStreams](src/ToAsyncOrNotToAsync/DoFlushAsyncOnStreams.cs)
- Avoid *sync-over-async* in ctor - it's better to provide asynchronous factory that is going to call `async` operation and then return newly created object
  - Code: [AvoidAsyncInCtor](src/ToAsyncOrNotToAsync/AvoidAsyncInCtor.cs)

## Guidelines for working with `async` in libraries
- Define a synchronous method for CPU-bound operations
- Define an async method for I/O-bound operations
- Define an async method if and only if we are not thread-bound
- Lib users seeing synchronous method will assume that they can safely parallelize it using the `ThreadPool`
- Lib users seeing async method will assume that spawning extra threads would be wasteful and instead parallelize the work by invoking the function in a tight loop on a single thread
- **Never mix async and sync code in a library** - do not call them from each other for code reuse!
