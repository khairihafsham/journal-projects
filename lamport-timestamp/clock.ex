defmodule Clock do
  defstruct [:timestamp, :process_name]

  def increment(clock = %Clock{timestamp: timestamp}) do
    %Clock{ clock | timestamp: timestamp + 1 }
  end

  def timestamp(clock) do
    Map.get(clock, :timestamp) 
  end
end
