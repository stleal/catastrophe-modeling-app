public class StormHeader
{
  public string Basin { get; set; }
  public string NameOfStorm { get; set; }
  public int NumberOfObservations { get; set; }

  public StormHeader()
  {
  }

  public StormHeader(string basin, string nameOfStorm, int numberOfObservations)
  {
    Basin = basin;
    NameOfStorm = nameOfStorm;
    NumberOfObservations = numberOfObservations;
  }

  public StormHeader(string[] headerLine)
  {
    Basin = headerLine[0];
    NameOfStorm = headerLine[1].Trim();
    NumberOfObservations = Convert.ToInt32(headerLine[2].Trim());
  }
}