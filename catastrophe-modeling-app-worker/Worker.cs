/********************************************************************************************
 * Author: Mr. Sam T. Leal
 * Date: 05/01/2026
 *******************************************************************************************/
namespace catastrophe_modeling_app;

public class Worker(ILogger<Worker> logger, IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            // check the Incoming folder path for new files
            var folderpath = configuration.GetSection("CatastropheModelingApp:IncomingFolderPath").Value!;
            // check if there are any files in the folder
            var files = Directory.GetFiles(folderpath);
            if (files.Length == 0)
                logger.LogInformation("No files found in the Incoming folder");
            if (files.Length == 1)
            {
              logger.LogInformation("1 file found in the Incoming folder");
              if (files[0] != null)
              {
                  List<StormObject> stormObjects = CreateStormObjects(files[0]).Where(s => s.StormYear.Year >= 1980).ToList();
                  Grid grid = new Grid(GenerateFloridaCells());
                  List<StormObject> filterOnFlorida = FilterOnFlorida(stormObjects, grid);
                  File.Delete(files[0]);
              }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }

    public List<Cell> GenerateFloridaCells()
    {
        int[] latitudes = configuration.GetSection("CatastropheModelingApp:FloridaLatitudeRangeArray").Get<int[]>()!;
        int[] longitudes = configuration.GetSection("CatastropheModelingApp:FloridaLongitudeRangeArray").Get<int[]>()!;
        List<Cell> cells = new List<Cell>();
        foreach (var latitude in latitudes)
        {
            for (int i = 0; i < longitudes.Length; i++)
            {
                Cell cell = new Cell(latitude, longitudes[i]);
                cells.Add(cell);
            }
        }
        return cells;
    }

    private static bool IsWaterCell(StormObservation observation, Grid grid)
    {
      if (observation.Latitude >= 24 && observation.Latitude <= 29 &&
          observation.Longitude >= 84 && observation.Longitude <= 88)
          return true;
      return false;
    }

    public static List<StormObject> FilterOnFlorida(List<StormObject> stormObjects, Grid grid)
    {
      List<StormObject> stormsThatHitFlorida = new List<StormObject>();
      foreach (var storm in stormObjects)
      {
        foreach (var observation in storm.StormObservations)
        {
          if (!IsWaterCell(observation, grid))
          {
            foreach (var cell in grid.GetCells())
            {
              if (observation.Latitude >= cell.Latitude && observation.Latitude <= cell.Latitude + 1 &&
                observation.Longitude <= cell.Longitude && observation.Longitude >= cell.Longitude - 1)
              {
                if (!stormsThatHitFlorida.Contains(storm))
                  stormsThatHitFlorida.Add(storm);
                break;
              }
            }
          }
        }
      }
      return stormsThatHitFlorida;
    }

    private static List<StormObject> CreateStormObjects(string filename)
    {
      var index = 0;
      List<StormObject> stormObjects = new List<StormObject>();
      try
      {
        string[] lines = File.ReadAllLines(filename);
        for (int i = index; i < lines.Length; i++)
        {
          // get the next header and observations
          StormHeader stormHeader = new StormHeader(lines[index].Split(","));
          List<StormObservation> stormObservations = new List<StormObservation>();
          for (int j = index+1; j <= i + stormHeader.NumberOfObservations; j++)
          {
            stormObservations.Add(new StormObservation(lines[j]));
            index++;
          }
          stormObjects.Add(new StormObject(stormHeader, stormObservations));
          i = index;
          index+=1;
        }
        return stormObjects;
      }
      catch (IOException ex)
      {
        throw new Exception("Error reading file", ex);
      }
    }
}