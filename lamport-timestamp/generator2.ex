require Event
require Clock

defmodule Generator do
  use GenServer

  def generate_char(name) do
    GenServer.cast(name, :generate_char)
  end

  def get_events(name) do
    GenServer.call name, :events
  end

  @doc """
  func will be function that contains the tasks to be executed
  by the process when run() is called
  """
  def start_process(name, func) do
    GenServer.start_link __MODULE__, %{name: name, func: func}, name: name
  end

  def run(name) do
    GenServer.cast name, :run
  end

  ### GENSERVER IMPLEMENTATIONS ###

  @impl true
  def init(%{name: name, func: func}) do
    {:ok, %{name: name, func: func, events: [], clock: %Clock{process_name: name, timestamp: 0}}}
  end

  @doc """
  This is for kickstarting the execution
  """
  @impl true
  def handle_cast(:run, state = %{ func: func }) do
    func.()

    {:noreply, state}
  end

  @doc """
  Returns all the events stored in the state
  """
  @impl true
  def handle_call(:events, _from, state = %{ events: events }) do
    {:reply, events, state}
  end

  @doc """
  Process use this to generate char internally
  """
  @impl true
  def handle_cast(:generate_char, state = %{clock: clock, events: events}) do
    updated_clock = Clock.increment(clock)
    new_event = %Event{name: :generate_char, clock: updated_clock}
    state = %{ state | clock: updated_clock }

    {:noreply, %{state | events: [ new_event | events] } }
  end

  @doc """
  This is to simulate an API for a process to receive message from other processes to generate chars
  """
  @impl true
  def handle_cast({ :generate, timestamp, from }, state = %{ name: name }) do
    GenServer.cast name, {:received, timestamp, from}
    run(name)

    {:noreply, state}
  end
end
