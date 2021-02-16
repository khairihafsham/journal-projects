require Event
require Clock

defmodule Generator do
  use GenServer

  def start_process_k do
    fun = fn() ->
      Generator.generate_char(:k)
      Generator.send_generate_message(:k, :j)
      Generator.generate_char(:k)
    end

    Generator.start_process(:k, fun)
  end

  def start_process_j do
    fun = fn() ->
      Generator.generate_char(:j)
      Generator.send_generate_message(:j, :i)
      Generator.generate_char(:j)
    end

    Generator.start_process(:j, fun)
  end

  def start_process_i do
    fun = fn() ->
      Generator.generate_char(:i)
    end

    Generator.start_process(:i, fun)
  end

  def generate_char(name) do
    GenServer.cast(name, :generate_char)
  end

  def get_events(name) do
    GenServer.call name, :events
  end

  def get_all_events do
    get_events(:k)
    |> Enum.concat(get_events(:j))
    |> Enum.concat(get_events(:i))
  end

  def start_process(name, func) do
    GenServer.start_link __MODULE__, %{name: name, func: func}, name: name
  end

  def run(name) do
    GenServer.cast name, :run
  end

  def send_generate_message(from, to) do
    GenServer.cast from, {:send_generate_message, to}
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
  def handle_cast({ :generate, from }, state = %{ name: name }) do
    GenServer.cast name, {:received, from}
    run(name)

    {:noreply, state}
  end

  @doc """
  A process uses this function to send a message to other process
  """
  @impl true
  def handle_cast({ :send_generate_message, to}, state = %{ name: name, events: events, clock: clock }) do
    updated_clock = Clock.increment(clock)
    new_event = %Event{name: "sent to #{to}", clock: updated_clock }
    state = %{ state | events: [ new_event | events ] }

    GenServer.cast to, { :generate, name }

    {:noreply, %{ state | clock: updated_clock }}
  end

  @doc """
  Here is the logic for creating an event to indicate the process has received a message.
  """
  @impl true
  def handle_cast({ :received, from }, state = %{ events: events, clock: clock }) do
    updated_clock = Clock.increment(clock)
    new_event = %Event{name: "received from #{from}", clock: updated_clock}
    state = %{ state | events: [ new_event | events] }

    {:noreply, %{ state | clock: updated_clock }}
  end
end
