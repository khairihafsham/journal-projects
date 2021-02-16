defmodule Event do
  defstruct [:clock, :name]

  def timestamp(event) do
    Map.get(event, :clock)
    |> Map.get(:timestamp)
  end

  def process_name(event) do
    Map.get(event, :clock)
    |> Map.get(:process_name)
  end
end
