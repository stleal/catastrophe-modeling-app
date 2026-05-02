using System.Globalization;

public class StormObject
{
  public StormHeader StormHeader { get; set; }
  public List<StormObservation> StormObservations { get; set; }
  public DateTime StormYear { get; set; }

  public StormObject()
  {
  }

  public StormObject(StormHeader stormHeader, List<StormObservation> stormObservations)
  {
    StormHeader = stormHeader;
    StormObservations = stormObservations;
    if (!string.IsNullOrEmpty(stormHeader.Basin) && stormHeader.Basin.Length == 8)
      StormYear = stormHeader.Basin.Substring(4, 4).Equals("") ? DateTime.MinValue : DateTime.ParseExact(stormHeader.Basin.Substring(4, 4), "yyyy", CultureInfo.InvariantCulture);
  }

}