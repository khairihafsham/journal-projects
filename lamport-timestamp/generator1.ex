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

  def start_process(name) do
    GenServer.start_link __MODULE__, %{name: name}, name: name 
  end

  ### GENSERVER IMPLEMENTATIONS ###

  @impl true
  def init(%{name: name}) do
    {:ok, %{name: name, events: [], clock: %Clock{process_name: name, timestamp: 0}}} 
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
end
